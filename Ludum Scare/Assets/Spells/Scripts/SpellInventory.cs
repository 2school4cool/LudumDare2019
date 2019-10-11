using System.Collections.Generic;
using ResourceSystem;
using UI;
using UnityEngine;

namespace Spells
{
    // The class which keeps track of which spells the player currently has in their inventory
    public class SpellInventory : MonoBehaviour
    {
        private const int MaxSpells = 3;
        
        public GameObject spellHolder;
        public GameObject indicator;
        public int currentSelection;
        public float inputCast;
        
        private float _inputRight;
        private float _inputLeft;
        private bool _rightHold, _leftHold;
        private Indicator _indicator;
        private GameObject _player;
        private IList<Spell> _spells;
        private float _previousInput;
        private bool _fireHeld;
        private DisplaySpells _spellHolder;
        private bool _inputDiscard;
        private bool _discardHeld;
        private float _inputScroll;
        private ResourceCollector _resourceCollector;

        // Class for specific spells which the inventory can contain
        public abstract class Spell : MonoBehaviour
        {
            // Spell cost by essence color and number
            public Dictionary<string, int> cost;
            public abstract string Name { get; }

            // Defines behaviour of spell when cast
            public abstract void Cast();

            // Defines behaviour of spell when selected
            public abstract void Selected();

            // Method for adding an essence cost to a spell
            public void AddCost(int red, int blue, int green, int yellow)
            {
                cost = new Dictionary<string, int>{
                    { "red", red},
                    {"blue", blue},
                    {"green", green},
                    { "yellow", yellow}
                };
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            _indicator = indicator.GetComponent<Indicator>();
            _spells = new List<Spell>();
            _spellHolder = spellHolder.GetComponent<DisplaySpells>();
            _rightHold = false;
            _leftHold = false;
            inputCast = 0;
            _fireHeld = false;
            currentSelection = 0;
            _player = GameObject.FindWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
            _inputScroll = Input.GetAxisRaw("Mouse ScrollWheel");
            _inputRight = Input.GetAxisRaw("RotateSpellRight");
            _inputLeft = Input.GetAxisRaw("RotateSpellLeft");
            inputCast = Input.GetAxisRaw("Fire3");
            _inputDiscard = Input.GetButton("DiscardSpell");
            if (Input.GetButton("Fire3"))
            {
                inputCast = 1;
            }
        }

        private bool CanCast(Spell spell)
        {
            if (!_resourceCollector) _resourceCollector = GameObject.FindWithTag("ResourceCollector").GetComponent<ResourceCollector>();
            int red = _resourceCollector.essences["red"] - spell.cost["red"];
            int blue = _resourceCollector.essences["blue"] - spell.cost["blue"];
            int green = _resourceCollector.essences["green"] - spell.cost["green"];
            int yellow = _resourceCollector.essences["yellow"] - spell.cost["yellow"];
            if (red < 0 || blue < 0 || green < 0 || yellow < 0)
            {
                return false;
            }
            return true;
        }

        private void SubtractCost(Spell spell)
        {
            if (!_resourceCollector) _resourceCollector = GameObject.FindWithTag("ResourceCollector").GetComponent<ResourceCollector>();
            _resourceCollector.AddEssence("red", -spell.cost["red"]);
            _resourceCollector.AddEssence("blue", -spell.cost["blue"]);
            _resourceCollector.AddEssence("green", -spell.cost["green"]);
            _resourceCollector.AddEssence("yellow", -spell.cost["yellow"]);
        }

        private void FixedUpdate()
        {
            // Discard spells 
            if (!_discardHeld && _inputDiscard && _spells.Count > 0)
            {
                RemoveSpell(currentSelection);
                if (_spells.Count > 0)
                {
                    if (currentSelection >= _spells.Count)
                    {
                        currentSelection = _spells.Count - 1;
                    }
                    SelectSpell(currentSelection);
                }
                else
                {
                    currentSelection = 0;
                    SelectSpell(currentSelection);
                    DefaultSelected();
                }
            }
            _discardHeld = _inputDiscard;

            // Scrolling through spells
            if (_spells.Count > 1)
            {
                if (_inputRight > 0.9 || _inputScroll > 0)
                {
                    if (!_rightHold)
                    {
                        currentSelection += 1;
                        currentSelection %= _spells.Count;
                        SelectSpell(currentSelection);
                    }
                    _rightHold = true;
                }
                else
                {
                    _rightHold = false;
                }
                if (_inputLeft > 0.9 || _inputScroll < 0)
                {
                    if (!_leftHold)
                    {
                        currentSelection -= 1;
                        currentSelection %= _spells.Count;
                        if (currentSelection < 0)
                        {
                            currentSelection += _spells.Count;
                        }
                        SelectSpell(currentSelection);
                    }
                    _leftHold = true;
                }
                else
                {
                    _leftHold = false;
                }
            }
            
            // Casting spells
            if (_spells.Count > 0)
            {
                if (inputCast > 0)
                {
                    if (_fireHeld == false && CanCast(_spells[currentSelection]))
                    {
                        if (!_player) return;
                        // Cast spell
                        _spells[currentSelection].Cast();
                        GameObject.FindWithTag("Player").GetComponent<Player.Player>().Cast();
                        SubtractCost(_spells[currentSelection]);
                        RemoveSpell(currentSelection);
                        GameObject.FindWithTag("ScoreCounter").GetComponent<ScoreCounter>().UpdateText(1);
                        
                        // Modify inventory accordingly
                        if (_spells.Count > 0)
                        {
                            if (currentSelection >= _spells.Count)
                            {
                                currentSelection = _spells.Count - 1;
                            }
                            SelectSpell(currentSelection);
                        }
                        else
                        {
                            currentSelection = 0;
                            SelectSpell(currentSelection);
                            DefaultSelected();
                        }
                    }
                    _fireHeld = true;
                }
                else
                {
                    _fireHeld = false;
                }
            }
        }

        // True if inventory has room to add spells, false otherwise
        public bool AddSpell (Spell spell)
        {
            if (_spells.Count >= MaxSpells)
            {
                return false;
            }
            _spells.Add(spell);
            _spellHolder.DisplaySpellAt(spell, _spells.Count - 1);
            if (_spells.Count == 1)
            {
                currentSelection = 0;
                SelectSpell(currentSelection);
            }
            return true;
        }

        // This function describes behaviour for when no spell is selected
        public void DefaultSelected()
        {
            _indicator.maxRange = 3;
        }

        // Selects spell at position
        private void SelectSpell(int position)
        {
            if (position >= _spells.Count)
            {
                return;
            }
            _spells[position].Selected();
            _spellHolder.SetSelectedSpell(position);
        }

        // Drops spell from inventory
        private void RemoveSpell(int position)
        {
            _spells.RemoveAt(currentSelection);
            _spellHolder.RemoveSpellAt(currentSelection);
        }
    }
}
