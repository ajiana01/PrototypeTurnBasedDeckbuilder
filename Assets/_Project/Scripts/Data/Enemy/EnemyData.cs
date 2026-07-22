using UnityEngine;

namespace _Project.Scripts.Data.Enemy
{
    [CreateAssetMenu(fileName = "NewEnemy", menuName = "Data/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [Header("Basic Info")]
        public string enemyName;

        public Sprite enemySprite;
    
        [Header("Stats")]
        public int maxHealth;
        public int baseDamage;
    }
}
