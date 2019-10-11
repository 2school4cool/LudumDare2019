using UnityEngine;

namespace Spells
{
    // Class which disables the collider on an object after 1 frame
    public class CollideRemover : MonoBehaviour
    {
        private CircleCollider2D _collider;

        // Start is called before the first frame update
        void Start()
        {
            _collider = GetComponent<CircleCollider2D>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            _collider.enabled = false;
        }
    }
}
