using UnityEngine;

namespace Enemies
{
    // Class containing death related methods for enemy
    public class EnemyKiller : MonoBehaviour
    {
        public GameObject deadEnemyLeftPrefab;
        public GameObject deadEnemyRightPrefab;
        private Enemy _enemy;
        
        public void Start()
        {
            _enemy = GetComponent<Enemy>();
        }
        
        // Triggers enemy death
        public void Die()
        {
            Instantiate(_enemy.left ? deadEnemyLeftPrefab : deadEnemyRightPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}