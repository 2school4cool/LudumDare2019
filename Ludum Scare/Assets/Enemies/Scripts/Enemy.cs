using System;
using Player;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Enemies
{
    // Class for generic enemy type which moves towards player
    public class Enemy : MonoBehaviour
    {
        private const float TopSpeed = 1f;
        private const float DisperseSpeed = 5f;
        private const float DefaultSpeed = 5f;
        private const float StunDuration = 1f;
        private const float StunRecoil = 0.5f;
        private const float StunDecreasePerFrame = 0.9f;

        private float _remainingStun;
        private Vector2 _stunVector;
        private UnityEngine.Camera _camera;
        private Animator _animator;
        private static readonly int Left = Animator.StringToHash("left");
        private GameObject _player;
        public GameObject hatStealerPrefab;
        public bool left;

        // Start is called before the first frame update
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _camera = UnityEngine.Camera.main;
            _player = GameObject.FindGameObjectWithTag("Player");
            _remainingStun = 0f;
        }
        
        // Update is called once per frame
        private void FixedUpdate()
        {
            // Disperse from center of screen when no alive player
            if (!_player)
            {
                DisperseFromCenter();
                return;
            }

            // Decrease stun if stunned
            if (_remainingStun > 0)
            {
                DecreaseStun();
                return;
            }
            
            // Calculate movement and animations if player alive and self not stunned
            _animator.SetBool(Left, !((transform.position - _player.transform.position).x < 0));
            CalculateMovement();
        }

        // Causes enemy to disperse from the side of the screen
        private void DisperseFromCenter()
        {
            Vector3 position = transform.position;
            Vector3 delta = (position - _camera.transform.position);
            delta.Normalize();
            delta.z = 0;
            position += Time.fixedDeltaTime * DisperseSpeed * delta;
            transform.position = position;
        }

        // Calculate where the enemy should move based on player position and nearby enemies
        private void CalculateMovement()
        {
            var selfPosition = transform.position;
            var selfVector = new Vector2(selfPosition.x, selfPosition.y);
            var playerPosition = _player.transform.position;
            var targetVector = new Vector2(playerPosition.x, playerPosition.y) - selfVector;
            targetVector.Normalize();
            targetVector *= DefaultSpeed * Time.fixedDeltaTime;

            // Distance self from other enemies
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            var force = Vector2.zero;
            foreach (var enemy in enemies)
            {
                if (enemy == this.gameObject)
                {
                    continue;
                }

                var position = enemy.transform.position;
                var enemyVector = new Vector2(position.x, position.y);
                var deltaVector = selfVector - enemyVector;

                if (deltaVector.magnitude > 10f)
                {
                    continue;
                }

                // Avoid 0 issues by adding random offset
                if (Math.Abs(deltaVector.magnitude) < 0.001)
                {
                    enemy.transform.position += new Vector3(
                        UnityEngine.Random.Range(1, 2) / 1000f,
                        UnityEngine.Random.Range(1, 2) / 1000f,
                        0);
                    deltaVector = selfVector - enemyVector;
                }
                force += 0.1f / (deltaVector.magnitude * deltaVector.magnitude) * deltaVector;
            }

            targetVector += force;

            var topSpeed = TopSpeed;
            var speed = Math.Min(targetVector.magnitude, topSpeed);
            targetVector.Normalize();
            targetVector.Scale(new Vector2(speed, speed));

            if (float.IsNaN(targetVector.x) || float.IsNaN(targetVector.y))
            {
                return;
            }

            transform.position += new Vector3(targetVector.x, targetVector.y, 0);
        }

        // Handles killing player on player collision
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "Player")
            {
                other.GetComponent<PlayerKiller>().Die();
                var position = transform.position;
                Instantiate(hatStealerPrefab, new Vector3(position.x, position.y, -24), Quaternion.identity);
                Destroy(gameObject);
            }
        }

        // Stuns the enemy 
        public void Stun(Vector2 stunVector)
        {
            _remainingStun = StunDuration;
            _stunVector = stunVector * StunRecoil;
        }

        // Decreases amount of stun on enemy 
        public void DecreaseStun()
        {
            _remainingStun -= Time.fixedDeltaTime;
            transform.position += new Vector3(_stunVector.x, _stunVector.y, 0);
            _stunVector *= StunDecreasePerFrame;
        }
    }
}
