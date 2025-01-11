using GD.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private WinCondition winCondition;
    [SerializeField] private WinCondition oneStarsCondition;
    [SerializeField] private WinCondition twoStartsCondition;
    [SerializeField] private WinCondition threeStarsCondition;
    private ConditionContext conditionContext;

    private void Start()
    {
        conditionContext = new ConditionContext();
        SeedCounter.OnSeedPlanted += OnSeedPlanted;
        TrashCounter.OnItemRecycled += OnItemRecycled;

        winCondition.ResetCondition();
        oneStarsCondition.ResetCondition();
        twoStartsCondition.ResetCondition();
        threeStarsCondition.ResetCondition();
    }

    private void OnDestroy()
    {
        SeedCounter.OnSeedPlanted -= OnSeedPlanted;
        TrashCounter.OnItemRecycled -= OnItemRecycled;
    }

    private void OnSeedPlanted()
    {
        winCondition.Evaluate(conditionContext);
        oneStarsCondition.Evaluate(conditionContext);
        twoStartsCondition.Evaluate(conditionContext);
        threeStarsCondition.Evaluate(conditionContext);
    }

    private void OnItemRecycled()
    {
        winCondition.Evaluate(conditionContext);
        oneStarsCondition.Evaluate(conditionContext);
        twoStartsCondition.Evaluate(conditionContext);
        threeStarsCondition.Evaluate(conditionContext);
    }
}
