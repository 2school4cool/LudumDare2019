using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    // Class which generates enemies in random positions every X units of time
    public class EnemyGenerator : MonoBehaviour
    {
        private const float SpawnRate = 5f; // 1 Enemy per X seconds
        
        public GameObject enemy;
        
        private IList<Vector2> _spawnPoints;
        private float _spawnCountdown;
        private UnityEngine.Camera _camera;
        private GameObject _gameCamera;
        private float _cameraHeight;
        private float _cameraWidth;

        // Start is called before the first frame update
        private void Start()
        {
            // Map of spawn points for enemies on the map
            _spawnPoints = new List<Vector2>
            {
                new Vector2(-20.82f, 13.8f),
                new Vector2(-22.7f, -1.9f),
                new Vector2(-17.6f, -14.6f),
                new Vector2(-5.4f, -11.6f),
                new Vector2(-3.1f, 16.7f),
                new Vector2(-10.4f, 7.3f),
                new Vector2(0.7f, 0.7f),
                new Vector2(14.9f, -9.4f),
                new Vector2(24f, -17.2f),
                new Vector2(24f, 0.9f),
                new Vector2(13.9f, 5.7f),
                new Vector2(27.1f, 19.6f),
                new Vector2(-2.73f, -7.15f),
                new Vector2(-2.73f, -7.15f),
            };

            _gameCamera = GameObject.FindWithTag("MainCamera");
            _camera = UnityEngine.Camera.main;
            _spawnCountdown = 0f;
            // Bounds check
            _cameraHeight = _camera.orthographicSize;
            _cameraWidth = _cameraHeight * _camera.aspect;
        }

        // Update is called once per frame
        public void FixedUpdate()
        {
            _spawnCountdown -= Time.fixedDeltaTime;
            if (_spawnCountdown < 0)
            {
                // Spawn an enemy
                var index = Random.Range(0, _spawnPoints.Count);
                var spawnPoint = _spawnPoints[index];
                if (spawnPoint.x > _gameCamera.transform.position.x - _cameraWidth
                    && spawnPoint.x < _gameCamera.transform.position.x + _cameraWidth
                    && spawnPoint.y > _gameCamera.transform.position.y - _cameraHeight
                    && spawnPoint.y < _gameCamera.transform.position.y + _cameraHeight) return;
                SpawnEnemy(spawnPoint);
                _spawnCountdown += SpawnRate;
            }
        }

        // Spawns an enemy at specified location
        private void SpawnEnemy(Vector2 spawnLocation)
        {
            Instantiate(enemy, spawnLocation, Quaternion.identity);
        }
    }
}
