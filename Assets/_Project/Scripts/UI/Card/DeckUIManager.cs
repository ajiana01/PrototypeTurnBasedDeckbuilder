using System.Collections.Generic;
using _Project.Scripts.Gameplay.Card;
using _Project.Scripts.Gameplay.CardDeck;
using UnityEngine;

namespace _Project.Scripts.UI.Card
{
    public class DeckUIManager : MonoBehaviour
    {
        [Header("Core Logic Reference")]
        [Tooltip("Reference to the logic manager. Keep this separate to maintain clean architecture.")]
        public DeckManager deckManager;

        [Header("UI & Pooling Setup")]
        public GameObject cardPrefab;
        public Transform handContainer;
        public int poolSize = 10;

        // The Object Pool
        private readonly Queue<CardView> _cardPool = new Queue<CardView>();
        // Tracks cards currently active in the player's hand
        private readonly List<CardView> _activeCardsInHand = new List<CardView>();


        #region Unity Lifecycle

        private void Start()
        {
            InitializePool();

            deckManager.OnCardDrawn += HandleCardDrawn;
            deckManager.OnCardDiscarded += HandleCardDiscarded;
        }

        private void OnDestroy()
        {
            if (deckManager != null)
            {
                deckManager.OnCardDrawn -= HandleCardDrawn;
                deckManager.OnCardDiscarded -= HandleCardDiscarded;
            }
        }

        #endregion

        #region UI Functions

        /// <summary>
        /// Pre-instantiates the card UI objects and disables them, storing them in the pool.
        /// </summary>
        private void InitializePool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject cardObj = Instantiate(cardPrefab, handContainer);
                cardObj.SetActive(false);
                
                CardView cardView = cardObj.GetComponent<CardView>();
                _cardPool.Enqueue(cardView);
            }
        }

        /// <summary>
        /// Event listener for when the logic layer draws a card.
        /// </summary>
        private void HandleCardDrawn(CardInstance instance)
        {
            if (_cardPool.Count == 0)
            {
                Debug.LogWarning("Card UI Pool is empty! Consider increasing the poolSize.");
                return;
            }

            // Get an inactive card from the pool
            CardView availableCard = _cardPool.Dequeue();
            
            // Prepare its data and turn it on
            availableCard.Initialize(instance, this);
            availableCard.gameObject.SetActive(true);
            availableCard.transform.SetAsLastSibling(); // Ensure it appears on the right side of the layout
            
            _activeCardsInHand.Add(availableCard);
        }

        /// <summary>
        /// Event listener for when the logic layer discards a card.
        /// </summary>
        private void HandleCardDiscarded(CardInstance instance)
        {
            // Find the UI card that matches the exact unique InstanceID
            CardView cardToReturn = _activeCardsInHand.Find(c => c.CurrentInstance.InstanceID == instance.InstanceID);
            
            if (cardToReturn != null)
            {
                ReturnCardToPool(cardToReturn);
            }
            else
            {
                Debug.LogWarning($"Failed to find UI for card '{instance.BaseData.cardName}' with ID {instance.InstanceID}");
            }
        }

        /// <summary>
        /// Deactivates the card UI and puts it back into the queue for future use.
        /// </summary>
        private void ReturnCardToPool(CardView cardView)
        {
            cardView.gameObject.SetActive(false);
            _activeCardsInHand.Remove(cardView);
            _cardPool.Enqueue(cardView);
        }

        /// <summary>
        /// Called by a specific CardView when the player clicks it.
        /// </summary>
        public void PlayCardFromUI(CardView cardViewPlayed)
        {
            // Pass the request down to the logic layer
            deckManager.DiscardCard(cardViewPlayed.CurrentInstance);
        }

        #endregion
        
    }
}
