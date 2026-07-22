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
