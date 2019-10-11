using UnityEngine;

namespace Spells
{
    // Class for projectile which turns into sticky trap
    public class Slider : MonoBehaviour
    {
        private const float SpeedDecay = 10f;
        private const float Rotation = 180;
        private const float TimeUntilDetonation = 0.4f;
        private const float SpawnDistanceFromPlayer = 0.7f;
        
        private float _input;
        private float _cooldown;
        private SpriteRenderer _spriteRenderer;
        private Indicator _indicator;
        private Vector3 _goalPos;
        public GameObject stickyTrapPrefab;
        private float _timer;

        private void Start()
        {
            // Set position to be in direction of aim
            _indicator = GameObject.FindWithTag("Indicator").GetComponent<Indicator>();
            var newPos = new Vector2(_indicator.input.x, _indicator.input.y);
            if (newPos.magnitude < 0.1)
            {
                newPos = _indicator.lastGoodInput;
            }
            newPos.Normalize();
            newPos *= SpawnDistanceFromPlayer; // Distance from player
            var selfTransform = transform;
            transform.position += new Vector3(newPos.x, newPos.y, selfTransform.position.z);
            selfTransform.right = newPos;
            
            //Initialize timer
            _timer = 0;
        }

        // Setting goal for where projectile will travel towards
        public void setGoal(Vector3 goal)
        {
            _goalPos = goal;
        }
        
        private void FixedUpdate()
        {
            // Rotating projectile
            transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z + Rotation * Time.fixedDeltaTime);
            transform.position = transform.position + ( _goalPos - transform.position)  / SpeedDecay;
            
            // Detonating after X amount of time at position reached
            if ((transform.position - _goalPos).magnitude < 0.5f) // :')
            {
                _timer += 1f * Time.fixedDeltaTime;
                if (_timer > TimeUntilDetonation)
                {
                    Vector3 spawnPos = transform.position;
                    spawnPos.z = 10;
                    Instantiate(stickyTrapPrefab, spawnPos, transform.rotation);
                    Destroy(gameObject);
                }
            }
        }
    }
}
