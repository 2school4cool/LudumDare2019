using UnityEngine;

namespace Enemies
{
    // Class for enemy which steals player's hat if player dies
    public class HatStealer : MonoBehaviour
    {
        // Update is called once per frame
        void FixedUpdate()
        {
            transform.position = transform.position - new Vector3(Time.fixedDeltaTime, 0.25f * Time.fixedDeltaTime, 0);
        }
    }
}
