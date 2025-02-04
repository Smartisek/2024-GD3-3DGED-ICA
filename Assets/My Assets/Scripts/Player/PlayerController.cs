using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    /** Click point logic based on Pogle, accessable at: https://www.youtube.com/watch?v=LVu3_IVCzys **/

    #region Variables
    const string IS_WALKING = "isWalking";

    PlayerInput playerInput;
    NavMeshAgent agent;
    Animator animator;

    [Header("Movement and Effect")]
    [SerializeField] ParticleSystem clickEffect;
    [SerializeField] LayerMask clickLayer;
    [SerializeField] float rotationThreshold = 0.1f; // Threshold distance to stop rotation
    [SerializeField] float movementThreshold = 0.1f; // Threshold velocity to consider as moving

    [Header("Inventory UI")]
    [SerializeField] private InventoryUI inventoryUI;

    [Header("Player Inventory")]
    [SerializeField] private PlayerInventory playerInventory;

    [SerializeField] private NPC npc;

    public bool IsMoving { get; private set; } // Public property to expose movement status
    private bool isColliding; //flag when player bumps into something 

    #endregion

    private void Awake()
    {
        playerInput = new PlayerInput(); //access the PlayerInput class
        agent = GetComponent<NavMeshAgent>(); // Get NavMeshAgent component
        animator = GetComponent<Animator>(); // Get Animator component
        inventoryUI = FindObjectOfType<InventoryUI>(); // Find the InventoryUI component in the scene
        playerInventory = GetComponent<PlayerInventory>(); // Find the PlayerInventory component in the scene
        npc = FindObjectOfType<NPC>(); // Find the NPC component in the scene
        AssignInputs(); // Assign inputs
    }

    private void Update()
    {

        //check if the player is moving and if the player is moving, rotate the player to the direction of movement
        if (agent.velocity.sqrMagnitude > movementThreshold && agent.remainingDistance > rotationThreshold && !isColliding)
        {
            FaceTarget();
        }
        SetAnimations(agent.velocity.sqrMagnitude > movementThreshold); //correct animations to the actions 
        IsMoving = agent.velocity.sqrMagnitude > movementThreshold; // Update movement status for camera rotate check 
    }

    #region Movement Methods
    private void AssignInputs()
    {
     playerInput.Main.Movement.performed += ctx => ClickMove(); //subscribe to the movement performed event created inside the input system in unity
     playerInput.Main.PickUp.performed += ctx => PickUpItem(); //subscribe to the pick up performed event created inside the input system in unity
     playerInput.Main.Dialogue.performed += ctx => Dialogue(); //subscribe to the dialogue performed event created inside the input system in unity
    }
       

    private void ClickMove()
    {
        if (inventoryUI.IsInventoryOpen || npc.inDialogue)
        {
            return;
        }
       
        RaycastHit hit; // Create a raycast hit variable
        //check if the player clicks on the ground and if the player clicks on the ground, move the player to the clicked position
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickLayer))
        {
            agent.isStopped = false;
            agent.destination = hit.point; // Set the destination of the NavMeshAgent to the clicked position 
            //create an effect where we clicked 
            if (clickEffect != null)
            {
                Instantiate(clickEffect, hit.point + new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
            }
           
        }
    }

    private void FaceTarget()
    {
        if (agent.velocity != Vector3.zero) //if player is not just standing still
        {
            Vector3 direction = (agent.destination - transform.position).normalized; // Get the direction to the destination
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Get the rotation to look at the destination
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Rotate the player to the destination
        }
        else
        {
            return; //if player is standing still, return
        }
    }

    #endregion

    private void SetAnimations(bool isWalking)
    {
        
        animator.SetBool(IS_WALKING, isWalking); //set the walking animation to the player
    }

    private void PickUpItem()
    {
        if (playerInventory != null)
        {
            playerInventory.PickUpItem();
        }
    }

    private void Dialogue()
    {
        if (npc != null)
        {
            npc.StartDialogue();
            Debug.Log("Interacting with NPC.");
        } else
        {
            Debug.Log("No NPC found.");
        }
    }

    #region Subscribers
    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
    #endregion
}