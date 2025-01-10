using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SeedCounter
{
    public static int TotalSeedsPlanted { get; private set; }

    public static void IncrementSeedCounter()
    {
        TotalSeedsPlanted++;
        Debug.Log("Total seeds planted: " + TotalSeedsPlanted);
    }

    public static void ResetSeedCounter()
    {
        TotalSeedsPlanted = 0;
        Debug.Log("Seed counter reset");
    }
}
