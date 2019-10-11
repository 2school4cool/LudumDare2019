using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    // Class which handles the death of player
    public class PlayerKiller : MonoBehaviour
    {
        public GameObject deadPlayerPrefab;
        // Called by an enemy that collides with the player.
        public void Die()
        {
            Instantiate(deadPlayerPrefab, transform.position, Quaternion.identity);
            
            // Enable restart game objects
            GameObject.FindWithTag("RestartText").GetComponent<Text>().enabled = true;
            var otherRestartItems = GameObject.FindGameObjectsWithTag("RestartElement");
            foreach (GameObject item in otherRestartItems)
            {
                if (item.name == "RestartText")
                {
                    item.GetComponent<Text>().enabled = true;
                } else
                {
                    item.GetComponent<Image>().enabled = true;
                }
            }
            Destroy(gameObject);
        }
    }
}
