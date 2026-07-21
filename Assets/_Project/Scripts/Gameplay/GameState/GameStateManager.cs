using System;
using _Project.Scripts.Gameplay.CardDeck;
using UnityEngine;

namespace _Project.Scripts.Gameplay.GameState
{
    public enum GameState
    {
        Prepare,
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
        
        // We can add references for player and enemy here later

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
                case GameState.PlayerTurn:
                    HandlePlayerTurn();
                    break;
                case GameState.Resolve:
                case GameState.EnemyTurn:
                    HandleEnemyTurn();
                    break;
                case GameState.Win:
                case GameState.Lose:
                    break;
            }
        }

        private void HandlePrepare()
        {
            deckManager.InitializeDeck();
            ChangeState(GameState.PlayerTurn);
        }

        private void HandlePlayerTurn()
        {
            // 1. Refresh Action Points / Mana here (Action Economy reset)
            
            // 2. Draw initial hand for the turn
            if (deckManager.drawPile.Count == 0)
            {
                deckManager.ReshuffleDiscardIntoDraw();
            }
            deckManager.DrawCards(5);
            
            Debug.Log("Waiting for player actions...");
        }

        private void HandleEnemyTurn()
        {
            Debug.Log("Enemy is thinking...");
            
            // TODO: Implement Enemy AI logic here
            
            EndEnemyTurn();
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

        /// <summary>
        /// Called when the enemy finishes their actions.
        /// </summary>
        public void EndEnemyTurn()
        {
            if (CurrentState != GameState.EnemyTurn) return;
            
            ChangeState(GameState.PlayerTurn);
        }
    }
}
