using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    Vector3 agentDestination;
    public NavMeshAgent agent;
    public Animator anim;
    public LayerMask checkingLayers;
    
    bool interactPlayer;
    bool Interacting;
    public bool interactableButtons;
    public bool isFemale;
    
    public GameManager gameManager;
    Transform cameraTransform;

    QuestionsAndAnswers questionsAndAnswers;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        questionsAndAnswers = FindFirstObjectByType<QuestionsAndAnswers>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        cameraTransform = GameObject.FindWithTag("MainCamera").transform;

        agentDestination = gameManager.ExitPoints().position;
        agent.SetDestination(agentDestination);
    }

    private void Update()
    {
        InteractNPC();
        AnimationGenderForNPC();
        if (interactPlayer && !interactableButtons)
        {
            questionsAndAnswers.QuestionsButtonsActive();
        }
    }

    void InteractNPC()
    {
        interactPlayer = Physics.CheckSphere(transform.position, 2.4f, checkingLayers);
        if (interactPlayer && Input.GetKeyDown(KeyCode.F) && gameManager.AllowFKey && !Interacting)
        {
            agent.isStopped = true;
            Interacting = true;
            anim.SetBool("isInteracting", true);
            gameManager.DisableFKeyandPlayerActions();
            gameManager.OpenInteractPanel(true);
            Cursor.lockState = CursorLockMode.None;
            interactableButtons = true;
        }
        if (!interactPlayer && Interacting)
        {
            agent.isStopped = false;
            Interacting = false;
            anim.SetBool("isInteracting", false);
            gameManager.EnableFKeyandPlayerActions();
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
