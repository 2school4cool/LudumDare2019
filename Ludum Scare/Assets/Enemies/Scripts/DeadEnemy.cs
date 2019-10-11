using UnityEngine;

namespace Enemies
{
    // Class for dead enemy 
    public class DeadEnemy : MonoBehaviour
    {
        private const float TimerStart = 4f;
        private float _timer;

        // Start is called before the first frame update
        void Start()
        {
            _timer = TimerStart;
        }

        // Update is called once per frame
        void Update()
        {
            _timer -= Time.fixedDeltaTime;

            // Disappear after X amount of time
            if (_timer < 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
