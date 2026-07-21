using _Project.Scripts.Data.Card;
using _Project.Scripts.Gameplay.Card;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Card
{
    public class CardView : MonoBehaviour
    {
        [Header("UI References")]
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI costText;
        public TextMeshProUGUI valueText;

        // Stores the data currently represented by this UI card
        public CardInstance CurrentInstance { get; private set; }
    
        private DeckUIManager _uiManager;

        /// <summary>
        /// Initializes the visual representation of the card based on the provided data.
        /// </summary>
        public void Initialize(CardInstance instance, DeckUIManager manager)
        {
            CurrentInstance = instance;
            _uiManager = manager;

            nameText.text = instance.BaseData.cardName;
            costText.text = instance.BaseData.manaCost.ToString();
            valueText.text = instance.BaseData.effectValue.ToString();
        }

        /// <summary>
        /// Called when the player clicks or plays this specific UI card.
        /// (Usually hooked up to a Unity UI Button component on the prefab).
        /// </summary>
        [ContextMenu("PlayCard")]
        public void OnClickPlay()
        {
            // Tell the UI Manager that this specific card instance is being played
            _uiManager.PlayCardFromUI(this);
        }
    }
}
