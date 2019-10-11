using System;
using Enemies;
using Spells;
using UnityEngine;

namespace Player
{
    // Class which handles player's Punch attack
    public class Punch : MonoBehaviour
    {
        private const float Duration = 0.5f;
        private const float Cooldown = 0.5f;
        
        private float _input;
        private float _cooldownAfter;
        private float _cooldownVisible;
        private SpriteRenderer _spriteRenderer;
        private Indicator _indicator;
        private Vector2 _punchVector;
        private bool _held;
        private Vector3 _lastPlace;

        // Start is called before the first frame update
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.enabled = false;
            var selfTransform = transform;
            _indicator = selfTransform.parent.GetComponentInChildren<Indicator>();
            var newPos = new Vector2(0.7f, 0f);
            selfTransform.localPosition = new Vector3(newPos.x, newPos.y, selfTransform.position.z);
            selfTransform.right = newPos;
            _punchVector = newPos;
            _cooldownAfter = 0;
            _cooldownVisible = 0;
            var position = transform.position;
            _lastPlace = new Vector3(position.x, position.y, position.z);
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            // Get input
            _input = Input.GetAxisRaw("Punch");
            if (Input.GetButton("Punch")) _input = 1;

            // Calculate position of punch sprite
            var newPos = new Vector2(_indicator.input.x, _indicator.input.y);
            if (newPos.magnitude > 0.1)
            {
                newPos.Normalize();
                newPos *= 0.7f; // Distance from player
                _lastPlace = newPos; // new Vector3(newPos.x, newPos.y, transform.position.z);
            }

            // Disable when duration ends
            if (_cooldownVisible < 0.1f)
            {
                _spriteRenderer.enabled = false;
            } else
            {
                _cooldownVisible -= 1 * Time.fixedDeltaTime;
            }
            if (_cooldownAfter < 0.1f)
            {
                // Enable when player punches
                if (_input > 0.99 && _held == false)
                {
                    _cooldownAfter = Cooldown;
                    _cooldownVisible = Duration;
                    _spriteRenderer.enabled = true;
                    
                    var selfTransform = transform;
                    selfTransform.localPosition = new Vector3 (_lastPlace.x, _lastPlace.y, transform.localPosition.z);
                    selfTransform.right = _lastPlace;
                    _punchVector = _lastPlace;
                }
            } else
            {
                _cooldownAfter -= Time.fixedDeltaTime;
            }

            _held = Math.Abs(_input - 1) < 0.01f;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                if (_spriteRenderer.enabled)
                {
                    other.GetComponent<Enemy>().Stun(_punchVector);
                }
            }
        }
    }
}
