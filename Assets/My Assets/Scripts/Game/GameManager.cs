using GD.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region FIELDS
    //Consditions that need evaluation
    [SerializeField] private WinCondition winCondition;
    [SerializeField] private WinCondition oneStarsCondition;
    [SerializeField] private WinCondition twoStartsCondition;
    [SerializeField] private WinCondition threeStarsCondition;
    private ConditionContext conditionContext;
    #endregion

    private void Start()
    {
        conditionContext = new ConditionContext();
        SeedCounter.OnSeedPlanted += OnSeedPlanted; //Subscribe to the event
        TrashCounter.OnItemRecycled += OnItemRecycled; //Subscribe to the event
        SoundManager.PlayBackgroundMusic("BACKGROUND", 0.40f);
    }

    private void OnDestroy()
    {
        //Unsubscribe from the events
        SeedCounter.OnSeedPlanted -= OnSeedPlanted;
        TrashCounter.OnItemRecycled -= OnItemRecycled;
    }

    private void OnSeedPlanted()
    {
        //when a seed planted, check if conditions met 
        winCondition.Evaluate(conditionContext);
        oneStarsCondition.Evaluate(conditionContext);
        twoStartsCondition.Evaluate(conditionContext);
        threeStarsCondition.Evaluate(conditionContext);
    }

    private void OnItemRecycled()
    {
        //when an item recycled, check if conditions met
        winCondition.Evaluate(conditionContext);
        oneStarsCondition.Evaluate(conditionContext);
        twoStartsCondition.Evaluate(conditionContext);
        threeStarsCondition.Evaluate(conditionContext);
    }
}
