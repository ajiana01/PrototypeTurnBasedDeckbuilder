using System;
using System.Collections;
using _Project.Scripts.Data.Card;
using _Project.Scripts.Gameplay.Card;
using _Project.Scripts.Gameplay.CardDeck;
using _Project.Scripts.Gameplay.Health;
using UnityEngine;

namespace _Project.Scripts.Gameplay.GameState
{
    public enum GameState
    {
        Prepare,
        PlayerTurnStart,
        PlayerTurn,
        Resolve,
        EnemyTurn,
        Win,
        Lose
    }
    
    public class GameStateManager : MonoBehaviour
    {
        [Header("Core References")]
        public DeckManager deckManager;
        
        [Header("Battle Entities")]
        public HealthSystem playerHealth;
        public HealthSystem enemyHealth;

        public GameState CurrentState { get; private set; }
        
        // Event triggered whenever the state changes
        public event Action<GameState> OnStateChanged;

        private void Start()
        {
            // Start the game by initializing the board/deck
            ChangeState(GameState.Prepare);
        }

        /// <summary>
        /// Handles the transition between different game states.
        /// </summary>
        public void ChangeState(GameState newState)
        {
            CurrentState = newState;
            OnStateChanged?.Invoke(newState);

            Debug.Log($"=== Game State Changed: {newState} ===");

            // Execute specific logic based on the new state
            switch (newState)
            {
                case GameState.Prepare:
                    HandlePrepare();
                    break;
                case GameState.PlayerTurnStart:
                    HandlePlayerTurnStart();
                    break;
                case GameState.PlayerTurn:
                    HandlePlayerTurn();
                    break;
                case GameState.Resolve:
                    break;
                case GameState.EnemyTurn:
                    HandleEnemyTurn();
                    break;
                case GameState.Win:
                    Debug.Log("<color=green>YOU WIN!</color>");
                    break;
                case GameState.Lose:
                    Debug.Log("<color=red>YOU LOSE!</color>");
                    break;
            }
        }

        #region Preapare

        private void HandlePrepare()
        {
            deckManager.InitializeDeck();
            ChangeState(GameState.PlayerTurnStart);
        }

        #endregion

        #region Player Turn Start

        private void HandlePlayerTurnStart()
        {
            // 1. Refresh Action Points / Mana here (Action Economy reset)
            
            // 2. Draw initial hand for the turn
            if (deckManager.drawPile.Count == 0)
            {
                deckManager.ReshuffleDiscardIntoDraw();
            }
            deckManager.DrawCards(5);
            
            ChangeState(GameState.PlayerTurn);
        }

        #endregion
        
        #region Player Turn

        private void HandlePlayerTurn()
        {
            Debug.Log("Waiting for player actions...");
        }
        
        /// <summary>
        /// Called by the UI when the player clicks on a card.
        /// </summary>
        public void TryPlayCard(CardInstance card)
        {
            if (CurrentState != GameState.PlayerTurn) return;

            StartCoroutine(ResolveCardEffect(card));
        }
        
        /// <summary>
        /// Resolution Process: Lock UI, run effect, reduce HP, then discard card.
        /// </summary>
        private IEnumerator ResolveCardEffect(CardInstance card)
        {
            ChangeState(GameState.Resolve);

            yield return new WaitForSeconds(0.5f);

            switch (card.BaseData.type)
            {
                case CardType.Attack:
                    Debug.Log($"Play {card.BaseData.cardName}, give {card.BaseData.effectValue} damage!");
                    enemyHealth.TakeDamage(card.BaseData.effectValue);
                    break;
                case CardType.Heal:
                    Debug.Log($"Play {card.BaseData.cardName}, heal {card.BaseData.effectValue} HP!");
                    playerHealth.Heal(card.BaseData.effectValue);
                    break;
                case CardType.Skill:
                    Debug.Log($"Play {card.BaseData.cardName}, give {card.BaseData.effectValue} damage!");
                    enemyHealth.TakeDamage(card.BaseData.effectValue);
                    break;
            }

            deckManager.DiscardCard(card);

            yield return new WaitForSeconds(0.5f);

            if (enemyHealth.currentHealth <= 0)
            {
                ChangeState(GameState.Win);
            }
            else
            {
                ChangeState(GameState.PlayerTurn);
            }
        }
        
        
        /// <summary>
        /// Called by the UI (e.g., an "End Turn" button) when the player is done.
        /// </summary>
        public void EndPlayerTurn()
        {
            if (CurrentState != GameState.PlayerTurn) return;

            // Discard all remaining cards in hand at the end of the turn
            deckManager.DiscardAllCardsInHand();
            
            ChangeState(GameState.EnemyTurn);
        }

        #endregion
        
        #region Enemy Turn

        private void HandleEnemyTurn()
        {
            Debug.Log("Enemy is thinking...");
            
            StartCoroutine(EnemyActionRoutine());
        }
        
        /// <summary>
        /// Simple Enemy AI: Wait 1 second, attack the player, then return the turn.
        /// </summary>
        private IEnumerator EnemyActionRoutine()
        {
            Debug.Log("The enemy is preparing to attack...");
            yield return new WaitForSeconds(1.5f); 

            int enemyDamage = 5; // Hardcode damage
            Debug.Log($"The enemy attacks the player for {enemyDamage} damage!");
            playerHealth.TakeDamage(enemyDamage);

            yield return new WaitForSeconds(0.5f);

            if (playerHealth.currentHealth <= 0)
            {
                ChangeState(GameState.Lose);
            }
            else
            {
                EndEnemyTurn();
            }
        }
        
        /// <summary>
        /// Called when the enemy finishes their actions.
        /// </summary>
        public void EndEnemyTurn()
        {
            if (CurrentState != GameState.EnemyTurn) return;
            
            ChangeState(GameState.PlayerTurnStart);
        }

        #endregion
    }
}
