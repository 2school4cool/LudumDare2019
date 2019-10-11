using UnityEngine;

namespace Spells
{
    // Class detailing effects of shockwave spell
    public class ShockScroll : SpellInventory.Spell

    {
        private GameObject _indicator;
        private GameObject _player;
        public GameObject shockParticlePrefab;

        public override string Name { get; } = "ShockSpell";

        // Start is called before the first frame update
        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _indicator = GameObject.FindWithTag("Indicator");
            AddCost(0, 5, 5, 40);
        }
        public override void Cast()
        {
            Instantiate(shockParticlePrefab, _player.transform.position, Quaternion.identity);
            _player.GetComponent<Player.Player>().AddMovementDelay(10);
        }
        public override void Selected()
        {
            _indicator.GetComponent<Indicator>().maxRange = 3;
        }
    }
}
