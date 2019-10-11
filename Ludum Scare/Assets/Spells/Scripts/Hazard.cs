using Enemies;
using UnityEngine;

namespace Spells
{
    // Class for any objects which are hazardous for enemies
    public class Hazard : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyKiller>().Die();
            }
        }
    }
}
