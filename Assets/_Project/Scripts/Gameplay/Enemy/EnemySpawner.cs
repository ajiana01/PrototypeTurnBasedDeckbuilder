using System.Collections.Generic;
using _Project.Scripts.Data.Enemy;
using _Project.Scripts.Gameplay.Health;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [Tooltip("The base prefab for the enemy containing HealthSystem and UI")]
        public GameObject enemyPrefab;
    
        [Tooltip("The parent transform (e.g., a HorizontalLayoutGroup) where enemies will spawn")]
        public Transform spawnContainer;

        [Header("Debug Settings")] public List<EnemyData> enemiesToSpawn;
        
        /// <summary>
        /// Spawns a list of enemies based on the provided data and returns their HealthSystems.
        /// </summary>
        public List<HealthSystem> SpawnEnemies(List<EnemyData> enemiesToSpawn)
        {
            List<HealthSystem> spawnedEnemies = new List<HealthSystem>();

            // 1. Clear any existing enemies in the container (useful for moving to the next stage)
            foreach (Transform child in spawnContainer)
            {
                Destroy(child.gameObject);
            }

            // 2. Spawn new enemies
            foreach (EnemyData data in enemiesToSpawn)
            {
                GameObject enemyObj = Instantiate(enemyPrefab, spawnContainer);
                enemyObj.name = $"Enemy_{data.enemyName}";

                // Initialize the HealthSystem with data from the ScriptableObject
                EnemyController enemy = enemyObj.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.Initialize(data);
                } else Debug.LogError("Enemy Prefab is missing a EnemyController component!");
                
                spawnedEnemies.Add(enemy.GetEnemyHealthSystem());
            
                // TODO: pass data.enemyName and data.baseDamage to an EnemyController script later
            }

            return spawnedEnemies;
        }

        [ContextMenu("Spawn Enemies")]
        private void SpawnEnemyDebug()
        {
            SpawnEnemies(enemiesToSpawn);
        }
    }
}
