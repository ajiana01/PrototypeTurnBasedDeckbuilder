using System;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Health
{
    public class HealthSystem : MonoBehaviour
    {
        [Header("Health Settings")]
        public int maxHealth = 50;
        public int currentHealth { get; private set; }
        
        public event Action OnHealthChanged;
        public event Action OnDied;

        private void Start()
        {
            currentHealth = maxHealth;
            OnHealthChanged?.Invoke();
        }
        
        /// <summary>
        /// Initializes health dynamically. Used by Spawners to apply EnemyData stats.
        /// </summary>
        public void InitializeHealth(int newMaxHealth)
        {
            maxHealth = newMaxHealth;
            currentHealth = maxHealth;
        
            // Trigger event so the UI updates immediately
            OnHealthChanged?.Invoke();
        
            Debug.Log($"{gameObject.name} initialized with {maxHealth} HP.");
        }

        public void TakeDamage(int damageAmount)
        {
            if (currentHealth <= 0) return;

            currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
            OnHealthChanged?.Invoke();
            
            if (currentHealth == 0)
            {
                Die();
            }
        }

        public void Heal(int healAmount)
        {
            if (currentHealth <= 0) return;

            currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
            OnHealthChanged?.Invoke();
        }

        private void Die()
        {
            OnDied?.Invoke();
        }
    }
}
