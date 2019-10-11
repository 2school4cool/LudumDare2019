using ResourceSystem;
using UnityEngine;

namespace Essence
{
    // Class for essence collected by player
    public class Essence : MonoBehaviour
    {
        private const float MovespeedIncreaseOverTime = 0.2f;
        private const float HoverDepth = 0.055f;
        private const float HoverSpeed = 3;
        private const float DefaultMoveTowardsPlayerSpeed = 1f;

        public GameObject essencePrefab;
        public string color;
        
        private int _essenceNumber;
        private float _moveTowardsPlayerSpeed;
        private bool _movingTowardsPlayer;
        private int _spawnNumber;
        private float _timeCounter;
        private Vector3 _basePosition;
        private ResourceGenerator _resourceGenerator;
        private ResourceCollector _resourceCollector;
        private GameObject _player;

        // Start is called before the first frame update
        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _resourceCollector = GameObject.FindWithTag("ResourceCollector").GetComponent<ResourceCollector>();
            _resourceGenerator = GameObject.FindWithTag("ResourceGenerator").GetComponent<ResourceGenerator>();
            
            // Spawn random number of essence pieces
            System.Random rnd = new System.Random();
            _essenceNumber = rnd.Next(8, 14);
            SpawnEssencePieces(_essenceNumber);
            
            // Set initial values for hover effect
            _timeCounter = Random.Range(0, 200);
            _basePosition = transform.position;
            _movingTowardsPlayer = false;
            _moveTowardsPlayerSpeed = DefaultMoveTowardsPlayerSpeed;
        }

        // Spawns n essence pieces at current pieces
        private void SpawnEssencePieces(int n)
        {
            for (int i = 0; i < n; i++)
            {
                Vector3 newPosition = transform.position;
                newPosition.z -= 0.1f;
                GameObject essencePiece = Instantiate(essencePrefab, newPosition, Quaternion.identity);
                essencePiece.transform.parent = transform;
            }
        }

        // Sets the spawn number of the essence object (which spawn point it is currently placed at)
        public void SetSpawnNumber(int spawnNumber)
        {
            _spawnNumber = spawnNumber;
        }

        // Sets whether or not the essence is moving towards the player
        public void SetMoveTowardsPlayer(bool moveTowardsPlayer)
        {
            this._movingTowardsPlayer = moveTowardsPlayer;
        }
        
        // Returns essence color
        public string GetColor()
        {
            return this.color;
        }

        // FixedUpdate is called once per frame
        private void FixedUpdate()
        {
             if (_movingTowardsPlayer)
             {
                 if (!_player) return;
                 transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _moveTowardsPlayerSpeed * Time.fixedDeltaTime);
                 _moveTowardsPlayerSpeed += MovespeedIncreaseOverTime;
             } else
             {
                 _timeCounter +=  Time.fixedDeltaTime;
                 var newPosition = new Vector3(0, 0, 0) {y = Mathf.Cos(HoverSpeed * _timeCounter) * HoverDepth};
                 transform.position = _basePosition + newPosition;
             }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "Player")
            {
                _resourceCollector.AddEssence(color, transform.childCount - 1);
                _resourceGenerator.NotifyCollectedEssence(_spawnNumber);
                Destroy(gameObject);
            }
        }
    }
}
