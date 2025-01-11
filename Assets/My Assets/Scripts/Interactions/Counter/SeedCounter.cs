using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SeedCounter
{
    public static int TotalSeedsPlanted { get; private set; }
    public static event Action OnSeedPlanted;

    public static void IncrementSeedCounter()
    {
        TotalSeedsPlanted++;
        OnSeedPlanted?.Invoke();
        Debug.Log("Total seeds planted: " + TotalSeedsPlanted);
    }

    public static void ResetSeedCounter()
    {
        TotalSeedsPlanted = 0;
        OnSeedPlanted?.Invoke();
        Debug.Log("Seed counter reset");
    }
}
