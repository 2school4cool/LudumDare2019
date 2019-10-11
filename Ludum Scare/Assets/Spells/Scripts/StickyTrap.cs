using UnityEngine;

namespace Spells
{
    // Class for sticky trap object from sticky trap spell
    public class StickyTrap : MonoBehaviour
    {
        private const float TimeUntilDestroy = 10;
        private const float InitialRotation = 90;
        private const float RotationPerUnitTime = 90;
        private const float FinalRotation = 10;

        private float _rotation;
        private float _timer;
    
        // Start is called before the first frame update
        void Start()
        {
            _rotation = InitialRotation;
            _timer = 0;
        }

        // FixedUpdate is called once per frame
        void FixedUpdate()
        {
            _timer += Time.fixedDeltaTime;

            if (_rotation > FinalRotation) {
                transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z + _rotation * Time.fixedDeltaTime);
                _rotation -= RotationPerUnitTime * Time.fixedDeltaTime;
            }
        
            if (_timer > TimeUntilDestroy)
            {
                Destroy(gameObject);
            }
        }
    }
}
