using System.Collections;
using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    #region FIELDS
    [Header("Item Data from scriptable object")]
    [SerializeField] private ScriptableObject itemData;

    [Header("UI to show when player in range")]
    [SerializeField] private GameObject pickupUI;
    private Coroutine rotateUICoroutine;

    #endregion

    public ScriptableObject ItemData => itemData;

    private void Awake()
    {
        if (itemData != null)
        {
            pickupUI.SetActive(false); //dont show
        }
    }

    private IEnumerator RotateUI()
    {
        while (true)
        {
            pickupUI.transform.Rotate(Vector3.up*100*Time.deltaTime);
            yield return null;
        }
    }

    #region COLLISION
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerInventory = other.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.SetNearbyItem(this);

                if (pickupUI != null)
                {
                    pickupUI.SetActive(true); //show when player collide
                    if (rotateUICoroutine == null)
                    {
                        rotateUICoroutine = StartCoroutine(RotateUI());
                    }
                }
            }
            else
            {
                Debug.LogError("PlayerInventory component not found on the player.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerInventory = other.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.ClearNearbyItem(this);

                if (pickupUI != null)
                {
                    pickupUI.SetActive(false); //dont show when player exit
                    if (rotateUICoroutine != null)
                    {
                        StopCoroutine(rotateUICoroutine);
                        rotateUICoroutine = null;
                    }
                }
            }
            else
            {
                Debug.LogError("PlayerInventory component not found on the player.");
            }
        }
    }
}
#endregion