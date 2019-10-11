using System;
using UnityEngine;

namespace Player
{
    // Class which manages player and its movements
    public class Player : MonoBehaviour
    {
        private const float UpperGameBoundsY = 22;
        private const float LowerGameBoundsY = -22;
        private const float UpperGameBoundsX = 29;
        private const float LowerGameBoundsX = -29;
        private const float MoveSpeed = 12f;
        private const float TopSpeed = 0.2f;
        private const float JoyChop = 0.8f; // Amount of deadzone so that you don't have to hold the analog stick fully out for max speed
        private const float MaxAccel = 1f;
        private const float CastDuration = 1f;
        private const float AnimationMinimumVelocity = 0.01f; 
        
        // Exposed to Unity editor
        public Rigidbody2D rb;
        private CircleCollider2D _circleCollider2D;
        private static readonly int Direction = Animator.StringToHash("direction");
        
        private Animator _mAnimator;
        private bool _inAWall;
        private Vector2 _input;
        private Vector2 _accel;
        private Vector2 _movement;
        private int _direction;
        private int _timeBeforeCanMove;
        private float _castingTime;

        // Start is called before the first frame update
        private void Start()
        {
            _circleCollider2D = GetComponent<CircleCollider2D>();
            _movement = new Vector2(0, 0);
            _mAnimator = GetComponent<Animator>();
            _direction = 0;
            _inAWall = false;
        }

        // Update is called once per frame
        private void Update()
        {
            _input.x = Input.GetAxisRaw("Horizontal");
            _input.y = Input.GetAxisRaw("Vertical");
        }

        private void FixedUpdate()
        {
            KeepWithinBounds(UpperGameBoundsX, UpperGameBoundsY, LowerGameBoundsX, LowerGameBoundsY);
            _circleCollider2D.enabled = !_inAWall;
            
            // If player is immobilized, decrease immobilization timer
            if (_timeBeforeCanMove > 0)
            {
                _timeBeforeCanMove -= 1;
            }
            else
            {
                MovePlayer();
                MovementAnimations();
            }
            //CastingAnimations();
        }
        
        // Adding a delay to player's ability to move
        public void AddMovementDelay(int n)
        {
            _timeBeforeCanMove = n;
        }

        // Set whether or not player is in a wall
        public void SetInWall(bool inWall)
        {
            this._inAWall = inWall;
        }
        
        // Set player to casting for n seconds
        public void Cast(float n=CastDuration)
        {
            _castingTime = n;
        }

        private void KeepWithinBounds(float upperX, float upperY, float lowerX, float lowerY)
        {
            var selfTransform = transform;
            var position = selfTransform.position;
            // Contain player within screen bounds
            if (position.x < lowerX)
            {
                position = new Vector3(lowerX, position.y, position.z);
                selfTransform.position = position;
            }

            if (position.y < lowerY)
            {
                position = new Vector3(position.x, lowerY, position.z);
                selfTransform.position = position;
            }

            if (transform.position.x > upperX)
            {
                position = new Vector3(upperX, position.y, position.z);
                selfTransform.position = position;
            }

            if (transform.position.y > upperY)
            {
                position = new Vector3(position.x, upperY, position.z);
                selfTransform.position = position;
            }
        }
        
        // Changes player's animation controller
        public void ChangePlayerAnimation (RuntimeAnimatorController newAnimation)
        {
            Animator animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = newAnimation;
        }
        
        // Handles variables which tell animation controller if player is currently casting
        private void CastingAnimations()
        {
            if (_castingTime > CastDuration) 
            {
                _mAnimator.SetInteger("state", 1);
                _castingTime -= Time.fixedDeltaTime;
            } else
            {
                _mAnimator.SetInteger("state", 0);
            }
        }

        // Handles variables which tell animation controller which direction player is facing
        private void MovementAnimations()
        {
            if (_movement.magnitude > AnimationMinimumVelocity)
            {
                _mAnimator.speed = 1;
                if (Math.Abs(_movement.x) - Math.Abs(_movement.y) >= 0)
                {
                    _direction = _movement.x < 0 ? 2 : 1;
                }
                else
                {
                    _direction = _movement.y < 0 ? 3 : 0;
                }
            } else
            {
                _mAnimator.speed = 0;
            }
            _mAnimator.SetInteger(Direction, _direction);
        }

        private void MovePlayer()
        {
            // Input is, on a scale of 0 to 1, how much we want to move in a direction. Directional (has negatives)
            // We change _movement by up to _maxAccel towards _input
            // Then we accel the _movement vector.
            // If no accel was added to the vector in an axis (within a tolerance for joystick) we add drag to return to 0.
            var chop = Math.Min(_input.magnitude, JoyChop);
            _input.Normalize();
            _input.Scale(new Vector2(chop, chop));

            var delta = (Time.fixedDeltaTime * MoveSpeed * _input) - _movement;
            var magnitude = Math.Min(delta.magnitude, MaxAccel);
            delta.Normalize();
            delta.Scale(new Vector2(magnitude, magnitude));

            _movement += Time.fixedDeltaTime * MoveSpeed * delta;
            var speed = Math.Min(_movement.magnitude, TopSpeed);
            _movement.Normalize();
            _movement.Scale(new Vector2(speed, speed));
            rb.MovePosition(rb.position + _movement);
        }
    }
    
    
    
}
  