using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int gameScore = 0;
    int tpPointIndex;
    int criminalCount = 0;

    [Header("Objects")]
    public GameObject[] npcs;
    public GameObject[] area1Spawnps, area2Spawnps; // npclerin bölgelere göre doðacaðý noktalar
    public Transform[] exitPoints; // npclerin gideceði noktalar
    public Transform[] finishAreaTeleportPoints; // tutuklanan npclerin ýþýnlanacaðý noktalar
    public Transform playerTransform;
    public GameObject mainCamera, finishCamera;

    public bool area1; // Hangi bölgede görev yapacaðýný sorguluyor.
    public bool AllowFKeyAndInteraction = true; // F tuþuna basmayý kýsýtlýyor.
    public bool ArrestingNPC; // NPC tutuklanma durumunu sorgular.
    public int npcTagChanger = 0; 
    public int npcCounter = 0; // Spawn olan npc'leri sayar.
    public int exitCriminal = 0; // Kaçan suçlularý sayar.
    public bool changingCamera; // kameralar arasý geçiþ için kullanýlýyor.
    public bool isCriminal; // eðer karakter suçlu ise true deðer döndürür.
    bool identityDelimiter; // Sýnýrlayýcýnýn deðerine göre randomIdentity ile random deðer oluþturur ve bu deðere göre kimliði açýp kapatýr.
    bool escPanel;
    bool activeEscPanel = true;
    bool policeWhistle;
    bool loseGame;
    bool completedCriminals;
    int randomIdentity;


    static int gameDay;
    

    [Header("UI Objects")]
    public TextMeshProUGUI QueryTMP;
    public GameObject InteractPanel, NpcsIdentityPanel, QuestionsPanel, FinishPanelBlack;
    public GameObject area1Collider, area2Collider;
    public Button identityButton, questionButton, arrestButton, releaseButton;
    public RawImage FinishImage;
    public TextMeshProUGUI[] IdentityCard;
    public GameObject IdentityPanel;
    public GameObject pausePanel; // Esc tuþuna basýnca açýlacak olan panel.
    public GameObject settingsPanel;
    public GameObject finishPanel; // Oyun bittiðinde istatistik gösterilecek olan panel.
    public GameObject gameOverPanel; 
    public TextMeshProUGUI criminalCountText, civilianCountText, succesRateText;

    [Header("Script Connections")]
    public PlayerController playerController;
    public MouseInput mouseInput;
    public QuestionsAndAnswers questAndAnswer;
    SettingsScript settingsScript;

    [Header("Light Settings")]
    public GameObject[] spotLights;
    bool LightIsOpen;
    int lightSequence = 0;

    [Header("Audio Settings")]
    public AudioSource OpeningLight;
    public AudioSource victorySFX;
    public AudioSource loseSFX;
    public AudioSource whistleSFX;
    public AudioSource[] SFXs;
    public AudioSource[] Musics;

    public List<GameObject> ScoreObjectsList = new(); // Sadece tutuklanan npclerin eklendiði list.
    public List<GameObject> AllNPCsList = new(); // Bütün npclerin eklendiði list.

    private void Awake()
    {
        if (gameDay % 3 == 0)
        {
            area1 = !area1;
        }
    }

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        mouseInput = FindFirstObjectByType<MouseInput>();
        questAndAnswer = FindFirstObjectByType<QuestionsAndAnswers>();
        settingsScript = GetComponent<SettingsScript>();

        StartCoroutine(NPCSpawner());
        tpPointIndex = 0;

        Cursor.lockState = CursorLockMode.Locked;

        if (area1)
        {
            playerTransform.position = new Vector3(-7.31151f, 0.83f, -123.5954f); // birinci bölgede oyuncunun spawn olacaðý nokta
            area1Collider.SetActive(false);
            area2Collider.SetActive(true);
        }
        if (!area1)
        {
            playerTransform.position = new Vector3(6.6f, 0.83f, 0.85f); // ikinci bölgede oyuncunun spawn olacaðý nokta
            area1Collider.SetActive(true);
            area2Collider.SetActive(false);
        }
    }

    private void Update()
    {
        CameraChanger();
        Final();

        if (Input.GetKeyDown(KeyCode.Escape) && activeEscPanel && !loseGame)
        {
            EscPanel();
        }
        
        foreach (AudioSource sfx in SFXs)
        {
            sfx.volume = settingsScript.GetSFXVolume();
        }

    }

    void CameraChanger()
    {
        if (gameScore < 5 && !loseGame)
        {
            changingCamera = false;
            mainCamera.SetActive(true);
            finishCamera.SetActive(false);
            FinishPanelBlack.SetActive(false);
        }
        if (gameScore >= 5 && !changingCamera)
        {
            completedCriminals = true;
            DisableFKeyandPlayerActions();
            FinishImage.gameObject.SetActive(true);
            FinishImage.CrossFadeAlpha(0f, 1f, true);
            Invoke("FinishArea", 1.6f);

            if (!policeWhistle)
            {
                policeWhistle = true;
                whistleSFX.Play();
            }
        }
        
    }

    void FinishArea()
    {
        whistleSFX.Stop();
        FinishImage.gameObject.SetActive(false);
        changingCamera = true;
        mainCamera.SetActive(false);
        finishCamera.SetActive(true);
        foreach (GameObject obj in AllNPCsList) { obj.SetActive(false); }
        foreach (GameObject obj in ScoreObjectsList) { obj.SetActive(true); }
    }

    void Final()
    {
        if (!LightIsOpen && changingCamera && lightSequence <= 4)
        {
            if (Physics.Raycast(spotLights[lightSequence].transform.position, Vector3.down, out RaycastHit hit, 10f, LayerMask.GetMask("NPC")))
            {
                Light lightComp = spotLights[lightSequence].GetComponent<Light>();
                OpeningLight.Play();

                if (lightSequence < spotLights.Length)
                {
                    lightSequence++;
                }

                lightComp.intensity = 8;
                if (hit.collider.CompareTag("Criminal"))
                {
                    criminalCount++;
                    lightComp.color = Color.red;
                }
                if (hit.collider.CompareTag("NPC"))
                {
                    lightComp.color = Color.green;
                }
                LightIsOpen = true;
                Invoke("LightOpener", 2f);
            }
        }

        if (lightSequence == spotLights.Length)
        {
            FinishedGame();
            activeEscPanel = false;
            if (criminalCount >= 4)
            {
                victorySFX.Play();
            }
            else
            {
                loseSFX.Play();
            }
            lightSequence++;
        }

        if (exitCriminal >= 2 && !loseGame && !completedCriminals)
        {
            loseSFX.Play();
            OpenInteractPanel(false);
            gameOverPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            FinishArea();
            foreach (GameObject obj in ScoreObjectsList) { obj.SetActive(false); }
            loseGame = true;
        }
    }

    void LightOpener()
    {
        LightIsOpen = false;
    }

    IEnumerator NPCSpawner()
    {
        while (gameScore < 5 && exitCriminal < 2 && !loseGame)
        {
            if (area1) // oyuncunun görev yeri 1. bölge ise çalýþýr 
            {
                int RandomNPCIndex = Random.Range(0, npcs.Length);
                int randomSpawnpointsIndex = Random.Range(0, area1Spawnps.Length);
               // if (randomSpawnpointsIndex > 2)
                Instantiate(npcs[RandomNPCIndex], area1Spawnps[randomSpawnpointsIndex].transform.position, Quaternion.identity);
                yield return new WaitForSeconds(3f);
            }
            if (!area1) // oyuncunun görev yeri 2. bölge ise çalýþýr
            {    
                int RandomNPCIndex = Random.Range(0, npcs.Length);
                int randomSpawnpointsIndex = Random.Range(0, area2Spawnps.Length);
               // if (randomSpawnpointsIndex > 3)
                Instantiate(npcs[RandomNPCIndex], area2Spawnps[randomSpawnpointsIndex].transform.position, Quaternion.identity);
                yield return new WaitForSeconds(10f);
            }
        }
    }

    public Transform ExitPoints()
    {
          // Çýkýþ noktalarýný rastgele þekilde seçer ve geri döndürür.
          int RandomExitPoints = Random.Range(0, exitPoints.Length);
          return exitPoints[RandomExitPoints];
    }

    public void ListAllNPCsAdded(GameObject allNPC)
    {
        AllNPCsList.Add(allNPC);
    }

    public void FinishAreaTeleportPoints(GameObject arrestedNPC)
    {
        ScoreObjectsList.Add(arrestedNPC);
        arrestedNPC.transform.position = finishAreaTeleportPoints[tpPointIndex].position;
        tpPointIndex++;
    }


    public void QueryText(bool isOpening, string Texting)
    {
        // NPClerin yanýndayken üst-ortada sorgula yazýsýný açýp kapatýr.
        if (isOpening)
        {
            QueryTMP.alpha = 1f;
            QueryTMP.text = Texting;
        }
        if (!isOpening)
        {
            float currentValue = QueryTMP.alpha -= 3f * Time.deltaTime;
            currentValue = Mathf.Clamp01(currentValue);
            QueryTMP.alpha = currentValue;
        }
    }

    public void EnableFKeyandPlayerActions()
    {
        AllowFKeyAndInteraction = true; // F tuþuna basabilir.
        playerController.isAction = false; // Oyuncu hareket edebilir.
        mouseInput.mouseActivity = false; // kamera hareketleri aktif.
        OpenNPCsIdentity(false); // Kimlik gözüküyor ise kapatýr.
        OpenQuestionsPanel(false); // Soru panelini kapatýr.  
    }

    public void DisableFKeyandPlayerActions()
    {
        AllowFKeyAndInteraction = false; // F tuþuna basamaz.
        playerController.isAction = true; // Oyuncu hareket edemez.
        mouseInput.mouseActivity = true; // kamera hareketleri kapalý.
    }

    public void OpenInteractPanel(bool isActive)
    {
        if (isActive)
        {
            Cursor.lockState = CursorLockMode.None;
            InteractPanel.SetActive(true);
            activeEscPanel = false;
        }
        if (!isActive)
        {
            Cursor.lockState = CursorLockMode.Locked;
            InteractPanel.SetActive(false);
            activeEscPanel = true;
        }
    }

    public void RandomIdentityInfos(int identityNmbr, string name, string surName, int rDay, int rMonth, int rYear)
    {
        IdentityCard[0].text = identityNmbr.ToString(); // Kimlik numarasý
        IdentityCard[1].text = name; // Ýsim
        IdentityCard[2].text = surName; // Soyisim
        IdentityCard[3].text = rDay.ToString() + "." + rMonth.ToString() + "." + rYear.ToString();
    }

    public void OpenNPCsIdentity(bool isActive)
    {
        if (!identityDelimiter)
        {
            randomIdentity = Random.Range(0, 4);
            identityDelimiter = true;
            Debug.LogError("false: " + randomIdentity);
        }

        if (randomIdentity >= 2)
        {
            NpcsIdentityPanel.SetActive(isActive);
        }
        else
        {
            IdentityPanel.SetActive(true);
        }

        if (!isActive)
        {
            NpcsIdentityPanel.SetActive(false);
            IdentityPanel.SetActive(false);
        }
    }

    public void OpenQuestionsPanel(bool isActive)
    {
        QuestionsPanel.SetActive(isActive);
        questAndAnswer.IsOpening(true);
    }

    public void ArrestNPC()
    {
        gameScore++;
        ArrestingNPC = true;
        identityDelimiter = false;
        OpenNPCsIdentity(false);
        QuestionsPanel.SetActive(false);
    }

    public void ReleaseNpc()
    {
        EnableFKeyandPlayerActions();
        Cursor.lockState = CursorLockMode.Locked;
        InteractPanel.SetActive(false);
        identityDelimiter = false;
        OpenNPCsIdentity(false);
        QuestionsPanel.SetActive(false);
    }

    public void GameDayUpdater()
    {
        gameDay++;
        Debug.Log(gameDay);
    }

    void FinishedGame()
    {
        finishPanel.GetComponent<Animator>().SetTrigger("Finish");
        Cursor.lockState = CursorLockMode.None;

        criminalCountText.text = criminalCount.ToString();
        civilianCountText.text = (ScoreObjectsList.Count - criminalCount).ToString();
        succesRateText.text = (criminalCount * 100 / ScoreObjectsList.Count).ToString();
    }

    public void EscPanel()
    {
        escPanel = !escPanel;
        if (!escPanel)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
            settingsPanel.SetActive(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SettingsPanel(bool isOpened)
    {
        pausePanel.SetActive(!isOpened);
        if (isOpened)
        {
            settingsPanel.SetActive(true);
        }
        else
        {
            settingsPanel.SetActive(false);
        }
    }
}
