using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPC : MonoBehaviour
{
    Vector3 agentDestination;
    public NavMeshAgent agent;
    public Animator anim;
    public LayerMask checkingLayers;
    
    bool interactPlayer;
    bool Interacted;
    public bool isFemale;
    
    public GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agentDestination = gameManager.ExitPoints().transform.position;
        agent.SetDestination(agentDestination);
    }

    private void Update()
    {
        InteractNPC();
        AnimationForNPC();
    }

    void InteractNPC()
    {
        interactPlayer = Physics.CheckSphere(transform.position, 2.4f, checkingLayers);
        if (interactPlayer && Input.GetKeyDown(KeyCode.F) && !Interacted)
        {
            agent.isStopped = true;
            Interacted = true;
            anim.SetBool("isInteracting", true);
        }
        if (!interactPlayer && Interacted)
        {
            agent.isStopped = false;
            Interacted = false;
            anim.SetBool("isInteracting", false);
        }
    }

    void AnimationForNPC()
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
