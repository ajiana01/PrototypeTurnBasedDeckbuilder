using System.Collections;
using _Project.Scripts.Gameplay.GameState;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.GameState
{
    public class GameStateUI : MonoBehaviour
    {
        [Header("Core Reference")]
        public GameStateManager stateManager;

        [Header("UI Elements")]
        public TextMeshProUGUI stateDisplayText;
        public Button endTurnButton;
        public Button playDummyCardButton;

        private void Start()
        {
            stateManager.OnStateChanged += UpdateUI;
            endTurnButton.onClick.AddListener(stateManager.EndPlayerTurn); 
            playDummyCardButton.onClick.AddListener(OnPlayDummyCardClicked);
            UpdateUI(stateManager.CurrentState);
        }

        private void OnDestroy()
        {
            if (stateManager != null)
            {
                stateManager.OnStateChanged -= UpdateUI;
            }
        }

        /// <summary>
        /// This function is called automatically every time GameStateManager executes ChangeState().
        /// </summary>
        private void UpdateUI(Gameplay.GameState.GameState newState)
        {
            stateDisplayText.text = $"Current State:\n<color=yellow>{newState}</color>";

            bool isPlayerTurn = (newState == Gameplay.GameState.GameState.PlayerTurn);
            
            endTurnButton.interactable = isPlayerTurn;
            playDummyCardButton.interactable = isPlayerTurn;
        }

        /// <summary>
        /// Simulation function to test entry and exit from the Resolving state
        /// </summary>
        private void OnPlayDummyCardClicked()
        {
            StartCoroutine(SimulateCardResolution());
        }

        private IEnumerator SimulateCardResolution()
        {
            Debug.Log("Cards played! Locking the UI and entering the Resolving phase...");
            
            stateManager.ChangeState(Gameplay.GameState.GameState.Resolve);

            yield return new WaitForSeconds(2.0f);

            Debug.Log("Card effect ends. Return to player's turn.");
            
            stateManager.ChangeState(Gameplay.GameState.GameState.PlayerTurn);
        }
    }
}
