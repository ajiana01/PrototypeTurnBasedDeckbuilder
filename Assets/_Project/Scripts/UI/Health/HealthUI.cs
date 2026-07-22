using _Project.Scripts.Gameplay.Health;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Health
{
    public class HealthUI : MonoBehaviour
    {
        [Header("Core Reference")]
        public HealthSystem healthSystem;

        [Header("UI Elements")]
        public Slider healthBarSlider;
        public TextMeshProUGUI healthText;
        
        private void Start()
        {
            if (healthSystem != null)
            {
                healthSystem.OnHealthChanged += UpdateHealthDisplay;
                UpdateHealthDisplay();
            }
            else
            {
                Debug.LogError($"HealthUI on {gameObject.name} has no reference to HealthSystem!\"");
            }
        }

        private void OnDestroy()
        {
            if (healthSystem != null)
            {
                healthSystem.OnHealthChanged -= UpdateHealthDisplay;
            }
        }

        /// <summary>
        /// Updated Slider and Text visuals based on latest HP data.
        /// </summary>
        private void UpdateHealthDisplay()
        {
            if (healthBarSlider != null)
            {
                healthBarSlider.value = (float)healthSystem.currentHealth / healthSystem.maxHealth;
            }

            if (healthText != null)
            {
                healthText.text = $"{healthSystem.currentHealth}/{healthSystem.maxHealth}";
            }
        }
    }
}
