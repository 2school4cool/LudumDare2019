using System.Collections.Generic;
using ResourceSystem;
using UnityEngine;

namespace Essence
{
    // Class for small essence pieces which orbit around an essence
    public class EssencePiece : MonoBehaviour
    {
        private const float OrbitRadiusModifier = 0.5f;
        private const float MoveTowardsPlayerSpeed = 3f;
        
        public Sprite greenSprite;
        public Sprite blueSprite;
        public Sprite redSprite;
        public Sprite yellowSprite;

        private float _timeCounter;
        private float _orbitSpeed;
        private float _orbitRadius;
        private bool _movingTowardsPlayer;
        private GameObject _resourceCollector;
        private GameObject _player;
        private string _color;

        // Start is called before the first frame update
        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _resourceCollector = GameObject.FindWithTag("ResourceCollector");
            _movingTowardsPlayer = false;

            // Randomize orbit variables
            System.Random rnd = new System.Random();
            _orbitSpeed = Random.Range(3f, 6f);
            _orbitRadius = Random.Range(0.2f, 0.4f);
            _timeCounter = 0;
            
            // Set color of essence piece
            _color = transform.parent.gameObject.GetComponent<global::Essence.Essence>().GetColor();
            IDictionary<string, Sprite> spriteMap = new Dictionary<string, Sprite>
            {
                {"green", greenSprite },
                {"blue", blueSprite },
                {"red", redSprite },
                {"yellow", yellowSprite }
            };
            GetComponent<SpriteRenderer>().sprite = spriteMap[_color];
        }
        
        public void SetMovingTowardsPlayer(bool mtp)
        {
            this._movingTowardsPlayer = mtp;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (!_movingTowardsPlayer)
            {
                // Orbit around parent essence shard
                _timeCounter += _orbitSpeed * Time.fixedDeltaTime;
                var x = Mathf.Sin(_timeCounter) * OrbitRadiusModifier;
                var y = Mathf.Cos(_timeCounter) * _orbitRadius * OrbitRadiusModifier;
                var z = Mathf.Cos(_timeCounter) * _orbitRadius * OrbitRadiusModifier;
                transform.localPosition = new Vector3(x, y, z);
            }
            else
            {
                // Move towards player
                if (!_player) return;
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, MoveTowardsPlayerSpeed * Time.fixedDeltaTime);
            }
        }
        private void OnTriggerEnter2D (Collider2D other)
        {
            if (other.gameObject.name == "Player")
            {
                _resourceCollector.GetComponent<ResourceCollector>().AddEssence(_color, 1);
                Destroy(gameObject);
            }
        }
    }
}
