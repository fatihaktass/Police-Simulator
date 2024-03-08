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
    bool Arresting;
    bool isTextShown;
    public bool interactableButtons;
    public bool isFemale;
    bool isCriminal;

    Transform cameraTransform;
    public GameManager gameManager;
    public QuestionsAndAnswers questionsAndAnswers;

    int randomDay;
    int randomMonth;
    int randomYear;
    int identityNumber;
    int RandomName;
    int RandomSurName;
    string[] npcNames;
    string[] npcSurNames;

    public GameObject NpcCameraPosition;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        questionsAndAnswers = FindFirstObjectByType<QuestionsAndAnswers>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        cameraTransform = GameObject.FindWithTag("MainCamera").transform;
        NpcCameraPosition = GameObject.Find("NpcPosition");

        IDChanger();
        TagChanger();

        gameManager.ListAllNPCsAdded(this.gameObject);
        agentDestination = gameManager.ExitPoints().position;
        agent.SetDestination(agentDestination);
    }

    void IDChanger()
    {
        if (isFemale)
        {
            npcNames = new string[] { "Beyza", "Güneþ", "Esmeralda", "Ýrem", "Burcu", "Sude", "Layla", "Aisha", "Nur", "Elif" };
        }
        if (!isFemale)
        {
            npcNames = new string[] { "Fatih", "Ali", "Ýlkay", "Yusuf", "Yiðit", "Bora", "Ahmed", "Omar", "Yusuf", "Hülagü Han" };
        }

        npcSurNames = new string[] { "Aktaþ", "Güçlü", "Özgün", "Kudret", "Nasip", "Öztürk", "Iþýk", "Selvi", "Aslan", "Tahtacý", "Kulaksýz", "Çetinkaya", "Özbalçýk", "Çakmak", "Akpýnar", "Yýlmaz", "Doðan", "Üzer", "Rashid", "Hassan", "Samaan", "Saleh" };

        randomDay = Random.Range(0, 32);
        randomMonth = Random.Range(0, 13);
        randomYear = Random.Range(1980, 2006);
        identityNumber = Random.Range(120200152, 956241235);
        RandomName = Random.Range(0, npcNames.Length);
        RandomSurName = Random.Range(0, npcSurNames.Length);

    }

    void TagChanger()
    {
        int RandomPoint = Random.Range(0, 4);
        if (RandomPoint == 0 || gameManager.npcTagChanger == 4 || gameManager.npcCounter >= 20) { isCriminal = true; }
        if (RandomPoint >= 1 && gameManager.npcTagChanger <= 3 && gameManager.npcCounter <= 19) { isCriminal = false; }
        if (isCriminal) { gameObject.tag = "Criminal"; gameManager.npcTagChanger = 0; }
        if (!isCriminal) { gameObject.tag = "NPC"; gameManager.npcTagChanger++; gameManager.npcCounter++; Debug.Log(gameManager.npcCounter); }
        Debug.Log(gameManager.npcTagChanger);
    }

    private void Update()
    {
        if (gameManager.AllowFKeyAndInteraction)
        {
            InteractNPC();
        }
        AnimationGenderForNPC();
        ArrestingNPC();
        TextEditor();
        if (interactPlayer && !interactableButtons)
        {
            questionsAndAnswers.QuestionsButtonsActive();
        }
        if (Interacting)
        { // NPC'nin y pozisyonunda hareketini kýsýtlar.
            Vector3 targetPosition = new(cameraTransform.transform.position.x,
                                                transform.position.y,
                                                cameraTransform.transform.position.z);
            gameObject.transform.LookAt(targetPosition);
            gameManager.RandomIdentityInfos(identityNumber, npcNames[RandomName], npcSurNames[RandomSurName], randomDay, randomMonth, randomYear);
        }
    }

    

    void InteractNPC()
    {
        interactPlayer = Physics.CheckSphere(transform.position, 2.4f, checkingLayers);
        if (interactPlayer && Input.GetKeyDown(KeyCode.F) && gameManager.AllowFKeyAndInteraction && !Interacting)
        {
            agent.isStopped = true;
            gameObject.transform.position = NpcCameraPosition.transform.position;
            Interacting = true;
            anim.SetBool("isInteracting", true);
            gameManager.DisableFKeyandPlayerActions();
            gameManager.OpenInteractPanel(true);
            interactableButtons = true;
            isTextShown = true;
            gameManager.isCriminal = isCriminal;
        }
        if (!interactPlayer && Interacting && !Arresting)
        {
            agent.isStopped = false;
            Interacting = false;
            isTextShown = false;
            anim.SetBool("isInteracting", false);
            gameManager.EnableFKeyandPlayerActions();
            gameManager.OpenInteractPanel(false);
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

    void ArrestingNPC()
    {
        if (interactPlayer && gameManager.ArrestingNPC && !Arresting && Interacting)
        {
            gameManager.EnableFKeyandPlayerActions();
            gameManager.OpenInteractPanel(false);
            agent.SetDestination(gameObject.transform.position);
            gameManager.FinishAreaTeleportPoints(this.gameObject);
            agent.SetDestination(gameObject.transform.position);
            Destroy(agent);
            gameObject.SetActive(false);
            Interacting = false;
            Arresting = true;
        }

        if (gameManager.changingCamera)
        {
            anim.SetBool("isInteracting", true);
            gameObject.transform.LookAt(gameManager.finishCamera.transform.position);
        }
    }

    void TextEditor()
    {
        if (!isTextShown)
        {
            gameManager.QueryText(interactPlayer, "Sorgulamak için [F] tuþuna basýn.");
        }
        if (gameManager.ArrestingNPC)
        {
            gameManager.QueryText(gameManager.ArrestingNPC, "Tutuklama baþarýlý!");
            Invoke(nameof(ArrestingText), 0.1f);
        }
    }
    void ArrestingText()
    {
        gameManager.ArrestingNPC = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ExitTriggers")) 
        {
            if (gameObject.CompareTag("NPC"))
            {
                gameObject.SetActive(false);
                Debug.Log("npc öldü");
            }
            if (gameObject.CompareTag("Criminal"))
            {
                gameObject.SetActive(false);
                Debug.Log("suclu öldü");
                gameManager.exitCriminal++;
            }
        }
        
    }
}
