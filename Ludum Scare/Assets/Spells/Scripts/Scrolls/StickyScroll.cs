using UnityEngine;

namespace Spells
{
    // Class detailing effects of sticky trap spell
    public class StickyScroll : SpellInventory.Spell
    {
        public GameObject stickyPrefab;
        private GameObject _indicator;
        private GameObject _player;
        public override string Name { get; } = "StickySpell";

        // Start is called before the first frame update
        void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _indicator = GameObject.FindWithTag("Indicator");
            AddCost(0, 5, 25, 0);
        }

        // Defines the result of casting the sticky spell
        public override void Cast()
        {
            GameObject sticky = Instantiate(stickyPrefab, _player.transform.position, Quaternion.identity) as GameObject;
            Vector3 goalPosition = new Vector3(_indicator.transform.position.x, _indicator.transform.position.y, 0);
            sticky.GetComponent<Slider>().setGoal(goalPosition);
        }

        // Defines the result of selecting the sticky spell
        public override void Selected()
        {
            _indicator.GetComponent<Indicator>().maxRange = 4;
        }
    }
}
