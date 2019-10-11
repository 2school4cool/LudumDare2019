using ResourceSystem;
using UnityEngine;

namespace Spells
{
    // Class detailing generic spell scroll 
    public class SpellScroll : MonoBehaviour
    {
        private const float HoverSpeed = 3f;
        private const float HoverDepth = 0.55f;

        public SpellInventory.Spell spell;
        private GameObject _spellInventory;
        private ResourceGenerator _resourceGenerator;
        private float _timer;
        private Vector3 _initialPosition;
        private int _spawnNumber;
    
        // Start is called before the first frame update
        private void Start()
        {
            _spellInventory = GameObject.FindWithTag("SpellInventory");
            _initialPosition = transform.position;
            _timer = Random.Range(0, 200);
        }

        // FixedUpdate is called once per frame
        private void FixedUpdate()
        {
            _timer += Time.fixedDeltaTime;
            Vector3 positionDelta = new Vector3(0, 0, 0);
            positionDelta.y = Mathf.Cos(_timer * HoverSpeed) * HoverDepth;
            transform.position = _initialPosition + positionDelta;
        }

        // Called by the resource generator when creating a new scroll 
        public void SetSpawnDetails(GameObject resourceGenerator, int spawnNumber)
        {
            _resourceGenerator = resourceGenerator.GetComponent<ResourceGenerator>();
            _spawnNumber = spawnNumber;
        }

        // Called when entering a collision with another object
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "Player" && _spellInventory.GetComponent<SpellInventory>().AddSpell(spell))
            {
                _resourceGenerator.NotifyCollectedScroll(_spawnNumber);
                Destroy(gameObject);
            }
        }

    }
}
