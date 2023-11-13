using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPC : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameManager gameManager;
    bool interactPlayer;
    bool Interacted;
    public LayerMask checkingLayers;
    Vector3 agentDestination;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        agent = GetComponent<NavMeshAgent>();
        agentDestination = gameManager.ExitPoints().transform.position;
        agent.SetDestination(agentDestination);
    }

    private void Update()
    {
        interactPlayer = Physics.CheckSphere(transform.position, 2.4f, checkingLayers);
        if (interactPlayer && Input.GetKeyDown(KeyCode.F) && !Interacted)
        {
            Debug.Log("yakýndasýnýz");
            agent.isStopped = true;
            Interacted = true;
        }

        if (!interactPlayer && Interacted)
        {
            Debug.Log("uzakta veya etkileþim sona erdi");
            agent.isStopped = false;
            Interacted = false;
        }
    }
}
