using _Project.Scripts.Data.Enemy;
using _Project.Scripts.Gameplay.Health;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [Header("Enemy Core References")]
        public HealthSystem enemyHealth;

        public void Initialize(EnemyData data)
        {
            if (enemyHealth != null)
            {
                enemyHealth.InitializeHealth(data.maxHealth);
            }
            else
            {
                Debug.LogError("Enemy Prefab is missing a HealthSystem component!");
            }
        }

        public HealthSystem GetEnemyHealthSystem()
        {
            return enemyHealth;
        }
    }
}
