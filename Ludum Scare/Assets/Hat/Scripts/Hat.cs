using UnityEngine;

namespace Hat
{
    // Class for player's hat which causes game to start on collision
    public class Hat : MonoBehaviour
    {
        public GameObject resourceGeneratorPrefab;
        public GameObject resourceCollectorPrefab;
        public GameObject enemyGeneratorPrefab;
        public RuntimeAnimatorController playerWithHatAnimation;
    
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Start game when player interacts with hat
            if (collision.gameObject.name == "Player") {
                // Instantiate the primary game objects
                Instantiate(resourceCollectorPrefab);
                Instantiate(resourceGeneratorPrefab);
                Instantiate(enemyGeneratorPrefab);
            
                // Update player animation to include hat
                Player.Player player = GameObject.FindWithTag("Player").GetComponent<Player.Player>();
                player.ChangePlayerAnimation(playerWithHatAnimation);
                Destroy(gameObject);
            }
        }
    }
}
