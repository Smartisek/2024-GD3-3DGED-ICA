using GD.Items;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GD.State
{
    /** NIAL'S CODE EDITTED TO MY GAME https://github.com/nmcguinness/2024-25-GD3A-IntroToUnity/tree/main **/

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

        [FoldoutGroup("Conditions")]
        [SerializeField]
        [Tooltip("The condition that determines if to show one star")]
        private ConditionBase oneStarsCondition;

        [FoldoutGroup("Conditions")]
        [SerializeField]
        [Tooltip("The condition that determines if to show two stars")]
        private ConditionBase twoStarsCondition;

        [FoldoutGroup("Conditions")]
        [SerializeField]
        [Tooltip("The condition that determines if to show three star")]
        private ConditionBase threeStarsCondition;

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
            if (oneStarsCondition != null && twoStarsCondition != null && threeStarsCondition != null)
            {
                oneStarsCondition.ResetCondition();
                twoStarsCondition.ResetCondition();
                threeStarsCondition.ResetCondition();
            }
               


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