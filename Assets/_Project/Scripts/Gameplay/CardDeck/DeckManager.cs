using System;
using System.Collections.Generic;
using _Project.Scripts.Data.Card;
using _Project.Scripts.Gameplay.Card;
using UnityEngine;

namespace _Project.Scripts.Gameplay.CardDeck
{
    public class DeckManager : MonoBehaviour
    {
        [Header("Deck Settings")]
        [Tooltip("List of starting cards when the game starts")]
        public List<CardData> startingDeck = new List<CardData>();

        [Header("Runtime Piles")]
        public List<CardInstance> drawPile = new List<CardInstance>();
        public List<CardInstance> hand = new List<CardInstance>();
        public List<CardInstance> discardPile = new List<CardInstance>();

        [Header("Rules")]
        public int maxHandSize = 5;

        public event Action<CardInstance> OnCardDrawn;
        public event Action<CardInstance> OnCardDiscarded;
        public event Action OnDeckReshuffled;


        /// <summary>
        /// Set up the Draw Pile at the start of the game.
        /// </summary>
        public void InitializeDeck()
        {
            drawPile.Clear();
            hand.Clear();
            discardPile.Clear();

            // Convert the base ScriptableObjects into unique CardInstances
            foreach (CardData data in startingDeck)
            {
                drawPile.Add(new CardInstance(data));
            }
            
            ShuffleDeck(drawPile);
            
            Debug.Log("The deck was successfully initialized and shuffled.");
        }

        /// <summary>
        /// Draw a number of cards into your hand.
        /// </summary>
        public void DrawCards(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                if (hand.Count >= maxHandSize)
                {
                    Debug.Log("Hands full! Can't draw any more cards.");
                    break;
                }

                if (drawPile.Count == 0)
                {
                    Debug.LogWarning("The Draw Pile is empty! There are no cards left to draw.");
                    break;
                }

                // Take the top card (last index) from the Draw Pile.
                CardInstance drawnCard = drawPile[^1];
                drawPile.RemoveAt(drawPile.Count - 1);
                
                hand.Add(drawnCard);
                OnCardDrawn?.Invoke(drawnCard);
                
                Debug.Log($"Draw a card: {drawnCard.BaseData.cardName}");
            }
        }

        /// <summary>
        /// Discard a card from your hand to the Discard Pile.
        /// </summary>
        public void DiscardCard(CardInstance cardToDiscard)
        {
            if (hand.Contains(cardToDiscard))
            {
                hand.Remove(cardToDiscard);
                discardPile.Add(cardToDiscard);
                
                OnCardDiscarded?.Invoke(cardToDiscard);
                Debug.Log($"Discard cards: {cardToDiscard.BaseData.cardName}");
            }
            else
            {
                Debug.LogError("Trying to throw away a card that is not in hand!");
            }
        }
        
        /// <summary>
        /// Discards a specific card based on its exact position (index) in the hand.
        /// </summary>
        public void DiscardCardByIndex(int handIndex)
        {
            if (handIndex >= 0 && handIndex < hand.Count)
            {
                CardInstance cardToDiscard = hand[handIndex];
            
                hand.RemoveAt(handIndex);
                discardPile.Add(cardToDiscard);
            
                OnCardDiscarded?.Invoke(cardToDiscard);
                Debug.Log($"Discarded card at index {handIndex}: {cardToDiscard.BaseData.cardName}");
            }
            else
            {
                Debug.LogError("Attempted to discard a card at an invalid index!");
            }
        }
        
        /// <summary>
        /// Discards all cards currently in the player's hand and moves them to the discard pile.
        /// Typically called at the end of the player's turn.
        /// </summary>
        public void DiscardAllCardsInHand()
        {
            // We loop backwards because removing an item from a List shifts the indexes of remaining items.
            // Looping backwards ensures we don't skip any elements or get an index out of bounds error.
            for (int i = hand.Count - 1; i >= 0; i--)
            {
                CardInstance cardToDiscard = hand[i];
            
                // Utilize the existing DiscardCard method to handle moving the data and triggering UI events
                DiscardCard(cardToDiscard);
            }
        
            Debug.Log("All cards in hand have been discarded.");
        }

        /// <summary>
        /// Reshuffles the Discard Pile into a Draw Pile when the Draw Pile runs out.
        /// </summary>
        public void ReshuffleDiscardIntoDraw()
        {
            Debug.Log("The Draw Pile is empty. Shuffle the Discard Pile back into the Draw Pile...");
            drawPile.AddRange(discardPile);
            discardPile.Clear();
            
            ShuffleDeck(drawPile);
            OnDeckReshuffled?.Invoke();
        }

        /// <summary>
        /// Fisher-Yates algorithm for shuffling lists.
        /// </summary>
        private void ShuffleDeck(List<CardInstance> deckToShuffle)
        {
            for (int i = deckToShuffle.Count - 1; i > 0; i--)
            {
                int randomIndex = UnityEngine.Random.Range(0, i + 1);
                (deckToShuffle[i], deckToShuffle[randomIndex]) = (deckToShuffle[randomIndex], deckToShuffle[i]);
            }
        }
    }
}
