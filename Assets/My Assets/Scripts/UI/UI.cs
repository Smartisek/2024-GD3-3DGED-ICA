using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    #region FIELDS
    [SerializeField] private Slider trashSlider;
    [SerializeField] private Slider treeSlider;

    [SerializeField] private Image star1;
    [SerializeField] private Image star2;
    [SerializeField] private Image star3;

    //Notification slider
    [SerializeField] private TextMeshProUGUI policeText;
    [SerializeField] private float slideDuration = 1f;
    [SerializeField] private Image border;
    [SerializeField] private Image police;
    [SerializeField] private GameObject policeMessageGroup;

    [SerializeField] private int totalTrash;
    [SerializeField] private int totalTrees;

    #endregion

    #region LISTENERS
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
    #endregion

    private void Start()
    {
        if (trashSlider != null)
        {
            trashSlider.maxValue = totalTrash;
        }

        if (treeSlider != null)
        {
            treeSlider.maxValue = totalTrees;
        }

        if (star1 && star2 && star3)
        {
            star1.enabled = false;
            star2.enabled = false;
            star3.enabled = false;
        }

        UpdateTreeSlider();
        UpdateTrashSlider();

        if (policeMessageGroup != null)
        {
            policeMessageGroup.SetActive(false);
        }
    }

    #region UI METHODS
    public void UpdateTreeSlider()
    {
        if (treeSlider != null)
        {
            treeSlider.value = SeedCounter.TotalSeedsPlanted;
        }

    }

    public void UpdateTrashSlider()
    {
        if (trashSlider != null)
        {
            trashSlider.value = TrashCounter.TotalItemsRecycled;
        }

    }
    #endregion

    #region CONDITIONS
    public void WinCondition()
    {
        Debug.Log("You win! FROM UI");
    }

    public void OneStarCondition()
    {
        star1.enabled = true;
        StartCoroutine(ShowPoliceMessage("You have 1 star!", Color.white));
    }

    public void TwoStarsCondition()
    {
        star2.enabled = true;
    }

    public void ThreeStarsCondition()
    {
        star3.enabled = true;
    }

    public void WrongBin()
    {
        StartCoroutine(ShowPoliceMessage("This DOES NOT belong in here!", Color.red));
    }


    #endregion

    #region NOTIFICATION UI
    //UI function for sliding in telling a message and sliding out 
    private IEnumerator ShowPoliceMessage(string message, Color color)
    {
        if (policeMessageGroup && policeText != null)
        {
            policeText.text = message;
            policeText.color = color;

           policeMessageGroup.SetActive(true);

            //sliding in 
            float elapsedTime = 0f;
            Vector3 startPosition = new Vector3(-Screen.width, policeMessageGroup.transform.localPosition.y, policeMessageGroup.transform.localPosition.z);
            Vector3 endPosition = new Vector3(0, policeMessageGroup.transform.localPosition.y, policeMessageGroup.transform.localPosition.z);

            while (elapsedTime < slideDuration)
            {
                policeMessageGroup.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / slideDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            policeMessageGroup.transform.localPosition = endPosition;

            //wait for 2 seconds
            yield return new WaitForSeconds(2f);

            //slide out
            elapsedTime = 0f;
            while(elapsedTime < slideDuration)
            {
                policeMessageGroup.transform.localPosition = Vector3.Lerp(endPosition, startPosition, elapsedTime / slideDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            policeMessageGroup.transform.localPosition = startPosition;

            policeMessageGroup.SetActive(false);
        }
    }
    #endregion


}
