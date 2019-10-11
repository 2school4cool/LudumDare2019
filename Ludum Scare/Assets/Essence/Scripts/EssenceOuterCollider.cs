using UnityEngine;

namespace Essence
{
    // Class for outer collider circle which detects nearby player for essence to move towards
    public class EssenceOuterCollider : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "Player")
            {
                this.GetComponentInParent<Essence>().SetMoveTowardsPlayer(true);
            }
        }
    }
}
