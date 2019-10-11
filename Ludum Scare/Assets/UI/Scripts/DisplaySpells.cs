using System.Collections.Generic;
using Spells;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    // Class which handles displaying user's current spells in their inventory
    public class DisplaySpells : MonoBehaviour
    {
        public GameObject redSpellIconPrefab;
        public GameObject blueSpellIconPrefab;
        public GameObject greenSpellIconPrefab;
        public GameObject yellowSpellIconPrefab;
        public GameObject selectIndicator;
        public GameObject leftHand;
        public GameObject rightHand;
        
        private IList<GameObject> _spells;
        private int _selectedSpot;

        private void Start()
        {
            _spells = new List<GameObject> { null, null, null };
            var position = selectIndicator.transform.localPosition;
            selectIndicator.transform.localPosition = new Vector3(-200, position.y, position.z);
            var localPosition = leftHand.transform.localPosition;
            localPosition = new Vector3(-25, localPosition.y, localPosition.z);
            leftHand.transform.localPosition = localPosition;
            var localPosition1 = rightHand.transform.localPosition;
            localPosition1 = new Vector3(150, localPosition1.y, localPosition1.z);
            rightHand.transform.localPosition = localPosition1;
        }

        public bool SetSelectedSpell(int position)
        {
            if (position < 0 || position >= _spells.Count)
            {
                return false;
            }

            _selectedSpot = position;
            var indicatorPosition = selectIndicator.transform.localPosition;
            selectIndicator.transform.localPosition = new Vector3(
                -200 + 175 * position,
                indicatorPosition.y,
                indicatorPosition.z);

            var leftHandIndex = 0;
            for (var i = 0; i < _spells.Count; i++)
            {
                if (i != position)
                {
                    var pos = leftHand.transform.localPosition;
                    leftHand.transform.localPosition = new Vector3(-200 + 175 * i, pos.y, pos.z);
                    leftHandIndex = i;
                    break;
                }
            }

            for (var i = leftHandIndex + 1; i < _spells.Count; i++)
            {
                if (i != position)
                {
                    var pos = rightHand.transform.localPosition;
                    rightHand.transform.localPosition = new Vector3(-200 + 175 * i, pos.y, pos.z);
                    break;
                }
            }

            return true;
        }

        public bool DisplaySpellAt(SpellInventory.Spell spell, int position)
        {
            if (position < 0 || position >= _spells.Count)
            {
                return false;
            }

            GameObject spellIconPrefab;
            switch (spell.Name)
            {
                case "FireSpell":
                    spellIconPrefab = redSpellIconPrefab;
                    break;
                case "FlashSpell":
                    spellIconPrefab = blueSpellIconPrefab;
                    break;
                case "StickySpell":
                    spellIconPrefab = yellowSpellIconPrefab;
                    break;
                case "ShockSpell":
                    spellIconPrefab = greenSpellIconPrefab;
                    break;
                default:
                    spellIconPrefab = redSpellIconPrefab;
                    break;
            }

            _spells[position] = Instantiate(spellIconPrefab, new Vector3(0, 0, 0), Quaternion.identity); // Choose which display here
            _spells[position].transform.SetParent(transform);
            _spells[position].GetComponent<Image>().SetNativeSize();
            _spells[position].GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
            _spells[position].transform.localScale = new Vector3(2, 2, 1);
            _spells[position].transform.localPosition = new Vector3(-200 + 175 * position, 50, 0);
            return true;
        }

        public bool RemoveSpellAt(int position)
        {
            if (position < 0 || position >= _spells.Count || _spells[position] == null)
            {
                return false;
            }

            Destroy(_spells[position]);
            _spells[position] = null;
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            ShiftSpellsRight(position);
            return true;
        }

        private bool ShiftSpellsRight(int missingPos)
        {
            if (missingPos < 0 || missingPos >= _spells.Count)
            {
                return false;
            }

            for (var i = missingPos; i < _spells.Count - 1; i++)
            {
                _spells[i] = _spells[i + 1];
                _spells[i + 1] = null;
                // Adjust visual position here
                if (_spells[i] != null)
                {
                    _spells[i].transform.localPosition = new Vector3(-200 + 175 * i, 50, 0);
                }
            }

            return true;
        }
    }
}
