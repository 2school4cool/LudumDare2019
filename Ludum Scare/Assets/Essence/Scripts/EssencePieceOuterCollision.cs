using UnityEngine;

namespace Essence
{
    // Class for outer collision circle which detects proximal player for esssence pieces to move towards
    public class EssencePieceOuterCollision : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "Player" )
            {
                this.GetComponentInParent<EssencePiece>().SetMovingTowardsPlayer(true);
            }
        }
    }
}
