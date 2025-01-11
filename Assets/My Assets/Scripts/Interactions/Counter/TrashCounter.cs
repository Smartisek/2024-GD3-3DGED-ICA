using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TrashCounter 
{
    public static int TotalItemsRecycled { get; private set; }
    public static event System.Action OnItemRecycled;

    public static void IncrementRecycledItems()
    {
        TotalItemsRecycled++;
        OnItemRecycled?.Invoke();
        Debug.Log("Total items recycled: " + TotalItemsRecycled);
    }

    public static void ResetRecycledItems()
    {
        TotalItemsRecycled = 0;
        OnItemRecycled?.Invoke();
        Debug.Log("Total items recycled: " + TotalItemsRecycled);
    }
}
