using UnityEngine;

namespace Hat
{
    // Class for opening hat object's mouth when player is proximal
    public class OpenMouthScript : MonoBehaviour
    {
        public GameObject hat;
        public Sprite openMouth;
        public Sprite closedMouth;
    
        private SpriteRenderer _hatRenderer;
        // Start is called before the first frame update
        private void Start()
        {
            _hatRenderer = hat.GetComponent<SpriteRenderer>();
        }

        // Open hat mouth on player proximity
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name == "Player")
            {
                _hatRenderer.sprite = openMouth;
            }
        }

        // Close hat mouth on player exit proximity
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.name == "Player")
            {
                _hatRenderer.sprite = closedMouth;
            }
        }
    }
}
