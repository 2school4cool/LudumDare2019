using UnityEngine;

namespace Spells
{
    // Class detailing effects of fire spell
    public class FireScroll : SpellInventory.Spell
    {
        public GameObject fireBallPrefab;
        private GameObject _indicator;
        private GameObject _player;

        // Start is called before the first frame update
        void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _indicator = GameObject.FindWithTag("Indicator");
            AddCost(20, 0, 0, 0);
        }

        public override string Name { get; } = "FireSpell";

        // Spell behaviour on cast
        public override void Cast()
        {
            Instantiate(fireBallPrefab, _player.transform.position, Quaternion.identity);
        }

        // Spell behaviour on selection
        public override void Selected()
        {
            _indicator.GetComponent<Indicator>().maxRange = 6;
        }
    }
}
