using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI : MonoBehaviour
{
    #region FIELDS
    [SerializeField] private Slider trashSlider;
    [SerializeField] private Slider treeSlider;
    [SerializeField] private Image star1;
    [SerializeField] private Image star2;
    [SerializeField] private Image star3;
    [SerializeField] private int totalTrash;
    [SerializeField] private int totalTrees;

    #endregion

    private void OnEnable()
    {
        SeedCounter.OnSeedPlanted += UpdateTreeSlider;
        TrashCounter.OnItemRecycled += UpdateTrashSlider;
    }

    private void OnDisable()
    {
        SeedCounter.OnSeedPlanted -= UpdateTreeSlider;
        TrashCounter.OnItemRecycled -= UpdateTrashSlider;
    }

    private void Start()
    {
        if(trashSlider != null)
        {
            trashSlider.maxValue = totalTrash;
        }

        if (treeSlider != null)
        {
            treeSlider.maxValue = totalTrees;
        }

        if(star1 && star2 && star3)
        {
            star1.enabled = false;
            star2.enabled = false;
            star3.enabled = false;
        }

        UpdateTreeSlider();
        UpdateTrashSlider();
    }

    public void UpdateTreeSlider()
    {
        if (treeSlider != null)
        {
            treeSlider.value = SeedCounter.TotalSeedsPlanted;
        }
        UpdateStars();
    }

    public void UpdateTrashSlider()
    {
        if (trashSlider != null)
        {
            trashSlider.value = TrashCounter.TotalItemsRecycled;
        }
        UpdateStars();
    }

    private void UpdateStars()
    {
        int trashRecycled = TrashCounter.TotalItemsRecycled;
        int treesPlanted = SeedCounter.TotalSeedsPlanted;

        if(treesPlanted >= 1)
        {
            star1.enabled = true;
        }
    }

    public void WinCondition()
    {
        Debug.Log("You win! FROM UI");
    }
}
