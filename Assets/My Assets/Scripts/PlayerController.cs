using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    const string WALK = "Walk";
    const string IDLE = "Idle";

    PlayerInput playerInput;
    NavMeshAgent agent;
    Animator animator;

    [Header("Movement and Effect")]
    [SerializeField] ParticleSystem clickEffect;
    [SerializeField] LayerMask clickLayer;

    private void Awake()
    {
        playerInput = new PlayerInput();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        AssignInputs();
    }

    private void AssignInputs()
    {
        playerInput.Main.Movement.performed += ctx => ClickMove();
    }

    private void ClickMove()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickLayer))
        {
            agent.destination = hit.point;
            if(clickEffect != null)
            {
                Instantiate(clickEffect, hit.point += new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
            }
        }
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
}
