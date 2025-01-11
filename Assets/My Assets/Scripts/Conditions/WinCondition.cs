using GD.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WinCondition", menuName = "GD/Conditions")]
public class WinCondition : ConditionBase
{
    [SerializeField] private int requiredTreesPlanted;
    [SerializeField] private int requiredTrashRecycled;
    [SerializeField] private GameEvent onWinConditionEvent;

    protected override bool EvaluateCondition(ConditionContext conditionContext)
    {
        Debug.Log($"Evaluating WinCondition: TotalSeedsPlanted = {SeedCounter.TotalSeedsPlanted}, requiredTreesPlanted = {requiredTreesPlanted}");
        bool isConditionMet = SeedCounter.TotalSeedsPlanted >= requiredTreesPlanted /*|| TrashCounter.TotalItemsRecycled >= requiredTrashRecycled*/;

        if(isConditionMet && !IsMet)
        {
            IsMet = true;
            TimeMet = Time.timeSinceLevelLoad;
            Debug.Log("****Win condition met!");
            onWinConditionEvent?.Raise();   
        }

        return isConditionMet;
    }
}
