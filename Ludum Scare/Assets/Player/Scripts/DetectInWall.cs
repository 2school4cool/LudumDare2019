using UnityEngine;

namespace Player
{
    // Class which detects whether or not player's inner hitbox is inside a wall
    public class DetectInWall : MonoBehaviour
    {
        private const float XOffsetFromCenter = 0.084f;
        private GameObject _player;
    
        // Start is called before the first frame update
        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            var selfTransform = transform;
            selfTransform.position = new Vector3(_player.transform.position.x - XOffsetFromCenter, _player.transform.position.y, selfTransform.position.z);
        }
    
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.name.Contains("Wall"))
            {
                _player.GetComponent<global::Player.Player>().SetInWall(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.name.Contains("Wall"))
            {
                _player.GetComponent<global::Player.Player>().SetInWall(false);
            }
        }
    }
}
