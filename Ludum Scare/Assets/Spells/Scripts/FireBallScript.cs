using Enemies;
using UnityEngine;

namespace Spells
{
    // Class containing the fireball object and its movement
    public class FireBallScript : MonoBehaviour
    {
        private const float SpawnDistanceFromPlayer = 0.7f;
        private const float IndicatorInputTolerance = 0.1f;
        private const float FireballSpeed = 13f;
        private const float MaxTravelDistance = 100;

        private float _input;
        private float _cooldown;
        private SpriteRenderer _spriteRenderer;
        private Indicator _indicator;
        private Vector3 _movementVector;

        private void Start()
        {
            _indicator = GameObject.FindWithTag("Indicator").GetComponent<Indicator>();
    
            // Get where the player is aiming their attack
            Vector2 directionVector = new Vector2(_indicator.input.x, _indicator.input.y);
            if (directionVector.magnitude < IndicatorInputTolerance) directionVector = _indicator.lastGoodInput;
            directionVector.Normalize();
            directionVector *= SpawnDistanceFromPlayer;

            // Move to aimed position
            Transform selfTransform = transform;
            selfTransform.position += new Vector3(directionVector.x, directionVector.y, selfTransform.position.z);
            selfTransform.right = directionVector;

            // Determine movement of fireball per unit time
            _movementVector = new Vector3(directionVector.x, directionVector.y, selfTransform.position.z / FireballSpeed) * FireballSpeed;
        }

        private void FixedUpdate()
        {
            // Move by _movementVector every frame until destruction
            transform.position += _movementVector * Time.fixedDeltaTime;
            if (transform.position.magnitude > MaxTravelDistance) Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Destruct on enemy collision
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyKiller>().Die();
                Destroy(gameObject);
            }
        }
    }
}
