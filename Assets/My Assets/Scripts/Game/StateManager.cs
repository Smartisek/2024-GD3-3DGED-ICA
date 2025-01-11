using GD.Items;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GD.State
{
    /// <summary>
    /// Manages the game state by evaluating win and loss conditions.
    /// </summary>
    public class StateManager : MonoBehaviour
    {
        [FoldoutGroup("Timing & Reset")]
        [SerializeField]
        [Tooltip("Reset all conditions on start")]
        private bool resetAllConditionsOnStart = true;

        [FoldoutGroup("Context", expanded: true)]
        [SerializeField]
        [Tooltip("Player reference to evaluate conditions required by the context")]
        private PlayerInventory playerInventory;

        [FoldoutGroup("Context")]
        [SerializeField]
        [Tooltip("Player inventory collection to evaluate conditions required by the context")]
        private InventoryCollection inventoryCollection;

        /// <summary>
        /// The condition that determines if the player wins.
        /// </summary>
        [FoldoutGroup("Conditions")]
        [SerializeField]
        [Tooltip("The condition that determines if the player wins")]
        private ConditionBase winCondition;

        /// <summary>
        /// The condition that determines if the player loses.
        /// </summary>
        [FoldoutGroup("Conditions")]
        [SerializeField]
        [Tooltip("The condition that determines if the player loses")]
        private ConditionBase loseCondition;

        [FoldoutGroup("Achievements [optional]")]
        [SerializeField]
        [Tooltip("Set of optional conditions related to acheivements")]
        private List<ConditionBase> achievementConditions;

        /// <summary>
        /// Indicates whether the game has ended.
        /// </summary>
        private bool gameEnded = false;

        private ConditionContext conditionContext;

        private void Awake()
        {
            if (playerInventory == null)
                throw new System.Exception("Player reference is required!");

            if (inventoryCollection == null)
                throw new System.Exception("Inventory collection reference is required!");

            // Wrap the two objects inside the context envelope
            conditionContext = new ConditionContext(playerInventory, inventoryCollection);

          
        }

        private void Start()
        {
            if (resetAllConditionsOnStart)
                ResetConditions();
        }

        

        /// <summary>
        /// Handles the logic when the player wins.
        /// </summary>
        protected virtual void HandleWin()
        {
            Debug.Log($"Player Wins! Win condition met at {winCondition.TimeMet} seconds.");

            // Implement win logic here, such as:
            // - Displaying a victory screen
            // - Transitioning to the next level
            // - Awarding points or achievements
            // - Playing a victory sound or animation

            // Example:
            // UIManager.Instance.ShowVictoryScreen();
            // SceneManager.LoadScene("NextLevel");
        }

        /// <summary>
        /// Handles the logic when the player loses.
        /// </summary>
        protected virtual void HandleLoss()
        {
            Debug.Log($"Player Loses! Lose condition met at {loseCondition.TimeMet} seconds.");

            // Implement loss logic here, such as:
            // - Displaying a game over screen
            // - Offering a restart option
            // - Reducing player lives
            // - Playing a defeat sound or animation

            // Example:
            // UIManager.Instance.ShowGameOverScreen();
            // GameManager.Instance.RestartLevel();
        }

        /// <summary>
        /// Resets the win and loss conditions.
        /// Call this method when restarting the game or level.
        /// </summary>
        public void ResetConditions()
        {
            // Reset the gameEnded flag
            gameEnded = false;

            // Reset the win condition
            if (winCondition != null)
                winCondition.ResetCondition();

            // Reset the lose condition
            if (loseCondition != null)
                loseCondition.ResetCondition();

            // Reset the achievement conditions
            if (achievementConditions != null)
            {
                foreach (var achievmentCondition in achievementConditions)
                {
                    if (achievmentCondition != null)
                        achievmentCondition.ResetCondition();
                }
            }
        }

    }
}