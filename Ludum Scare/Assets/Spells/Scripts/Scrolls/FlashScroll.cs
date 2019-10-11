using UnityEngine;

namespace Spells
{
    // Class detailing effects of flash spell
    public class FlashScroll : SpellInventory.Spell
    {
        public GameObject flashParticlesPrefab;
        private GameObject _indicator;
        private GameObject _player;
    
        void Start()
        {
            _indicator = GameObject.FindWithTag("Indicator");
            _player = GameObject.FindWithTag("Player");
            AddCost(0, 15, 0, 0);
        }

        public override string Name { get; } = "FlashSpell";

        // Behaviour on spell cast
        public override void Cast()
        {
            Vector3 newPosition = _indicator.transform.position;
            newPosition.z = 0;
            Instantiate(flashParticlesPrefab, _player.transform.position, Quaternion.identity);
            _player.transform.position = newPosition;
        }

        // Behaviour on spell selection
        public override void Selected()
        {
            _indicator.GetComponent<Indicator>().maxRange = 6;
        }
    }
}
