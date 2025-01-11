using GD.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private WinCondition winCondition;
    private ConditionContext conditionContext;

    private void Start()
    {
        conditionContext = new ConditionContext();
        SeedCounter.OnSeedPlanted += OnSeedPlanted;
    }

    private void OnDestroy()
    {
        SeedCounter.OnSeedPlanted -= OnSeedPlanted;
    }

    private void OnSeedPlanted()
    {
        winCondition.Evaluate(conditionContext);
    }
}
