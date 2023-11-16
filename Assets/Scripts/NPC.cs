using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    Vector3 agentDestination;
    public NavMeshAgent agent;
    public Animator anim;
    public LayerMask checkingLayers;
    
    bool interactPlayer;
    bool Interacting;
    public bool isFemale;
    
    public GameManager gameManager;
    Transform cameraTransform;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        cameraTransform = GameObject.FindWithTag("MainCamera").transform;
        agentDestination = gameManager.ExitPoints().transform.position;
        agent.SetDestination(agentDestination);
    }

    private void Update()
    {
        InteractNPC();
        AnimationGenderForNPC();
    }

    void InteractNPC()
    {
        interactPlayer = Physics.CheckSphere(transform.position, 2.4f, checkingLayers);
        if (interactPlayer && Input.GetKeyDown(KeyCode.F) && gameManager.AllowFKey && !Interacting)
        {
            agent.isStopped = true;
            Interacting = true;
            anim.SetBool("isInteracting", true);
            gameManager.FKeyandPlayerActions(false);
            gameManager.OpenInteractPanel(true);
            Cursor.lockState = CursorLockMode.None;
        }
        if (!interactPlayer && Interacting)
        {
            agent.isStopped = false;
            Interacting = false;
            anim.SetBool("isInteracting", false);
            gameManager.FKeyandPlayerActions(true);
            gameManager.OpenInteractPanel(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Interacting)
        {
            Vector3 targetPosition = new(cameraTransform.transform.position.x,
                                                transform.position.y,
                                                cameraTransform.transform.position.z);
            gameObject.transform.LookAt(targetPosition);
        }
    }

    void AnimationGenderForNPC()
    {
        if (isFemale)
        {
            anim.SetBool("isFemale", true);
        }
        if (!isFemale)
        {
            anim.SetBool("isFemale", false);
        }
    }
}
