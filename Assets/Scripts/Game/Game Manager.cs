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
    public GameObject[] area1Spawnps, area2Spawnps; // npclerin b�lgelere g�re do�aca�� noktalar
    public Transform[] exitPoints; // npclerin gidece�i noktalar
    public Transform[] finishAreaTeleportPoints; // tutuklanan npclerin ���nlanaca�� noktalar
    public Transform playerTransform;
    public GameObject mainCamera, finishCamera;
    public Transform directionalLightRot;

    public bool AllowFKeyAndInteraction = true; // F tu�una basmay� k�s�tl�yor.
    public bool ArrestingNPC; // NPC tutuklanma durumunu sorgular.
    public int npcTagChanger = 0;
    public int npcCounter = 0; // Spawn olan npc'leri sayar.
    public int exitCriminal = 0; // Ka�an su�lular� sayar.
    public bool changingCamera; // kameralar aras� ge�i� i�in kullan�l�yor.
    public bool isCriminal; // e�er karakter su�lu ise true de�er d�nd�r�r.
    float spawnSpeed = 10f;
    bool identityDelimiter; // S�n�rlay�c�n�n de�erine g�re randomIdentity ile random de�er olu�turur ve bu de�ere g�re kimli�i a��p kapat�r.
    bool escPanel;
    bool activeEscPanel = true;
    bool policeWhistle;
    bool loseGame;
    bool completedCriminals;
    bool playerCanArrest;
    int randomIdentity;

    [Header("UI Objects")]
    public TextMeshProUGUI QueryTMP;
    public GameObject InteractPanel, NpcsIdentityPanel, QuestionsPanel, FinishPanelBlack;
    public GameObject area1Collider, area2Collider;
    public Button identityButton, questionButton, arrestButton, releaseButton;
    public RawImage FinishImage;
    public TextMeshProUGUI[] IdentityCard;
    public GameObject IdentityPanel;
    public GameObject pausePanel; // Esc tu�una bas�nca a��lacak olan panel.
    public GameObject settingsPanel;
    public GameObject finishPanel; // Oyun bitti�inde istatistik g�sterilecek olan panel.
    public GameObject gameOverPanel;
    public TextMeshProUGUI criminalCountText, civilianCountText, succesRateText;
    public TextMeshProUGUI arrestedInfoText, escapedInfoText;
    public GameObject gameInfos;
    public GameObject warningPanel;
    public CanvasGroup fadePanel;

    [Header("Script Connections")]
    public PlayerController playerController;
    public MouseInput mouseInput;
    public QuestionsAndAnswers questAndAnswer;
    SettingsScript settingsScript;
    GameStatistics gameStatistics;

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

    public List<GameObject> ScoreObjectsList = new(); // Sadece tutuklanan npclerin eklendi�i list.
    public List<GameObject> AllNPCsList = new(); // B�t�n npclerin eklendi�i list.

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        mouseInput = FindFirstObjectByType<MouseInput>();
        questAndAnswer = FindFirstObjectByType<QuestionsAndAnswers>();
        settingsScript = GetComponent<SettingsScript>();
        gameStatistics = GetComponent<GameStatistics>();

        SpawnSpeedChanger();

        fadePanel.alpha = 0f;

        StartCoroutine(NPCSpawner());
        DirectionalRotChanger(false);
        tpPointIndex = 0;

        if (gameStatistics.GetMapIndex() == 0)
        {
            playerTransform.position = new Vector3(-7.31151f, 0.83f, -123.5954f); // birinci b�lgede oyuncunun spawn olaca�� nokta
            area1Collider.SetActive(false);
            area2Collider.SetActive(true);
            Debug.Log("1. B�lge");
        }
        if (gameStatistics.GetMapIndex() == 1)
        {
            playerTransform.position = new Vector3(6.6f, 0.83f, 0.85f); // ikinci b�lgede oyuncunun spawn olaca�� nokta
            area1Collider.SetActive(true);
            area2Collider.SetActive(false);
            Debug.Log("2. B�lge");
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CameraChanger();
        Final();
        VolumeAndSensValue();

        if (Input.GetKeyDown(KeyCode.Escape) && activeEscPanel && !loseGame)
        {
            EscPanel();
        }

        if (fadePanel.alpha > 0)
            fadePanel.alpha -= 1f * Time.deltaTime;
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
                gameInfos.SetActive(false);
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
        DirectionalRotChanger(true);
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

    void SpawnSpeedChanger()
    {
        if (gameStatistics.GetRankPoints() < 2000)
        {
            spawnSpeed = 10f;
        }
        else if (gameStatistics.GetRankPoints() >= 2000 && gameStatistics.GetRankPoints() < 4250)
        {
            spawnSpeed = 8f;
        }
        else
        {
            spawnSpeed = 6f;
        }
    }

    IEnumerator NPCSpawner()
    {
        while (gameScore < 5 && exitCriminal < 2 && !loseGame)
        {
            if (gameStatistics.GetMapIndex() == 0) // oyuncunun g�rev yeri 1. b�lge ise �al���r 
            {
                int RandomNPCIndex = Random.Range(0, npcs.Length);
                int randomSpawnpointsIndex = Random.Range(0, area1Spawnps.Length);
                // if (randomSpawnpointsIndex > 2)
                Instantiate(npcs[RandomNPCIndex], area1Spawnps[randomSpawnpointsIndex].transform.position, Quaternion.identity);
                yield return new WaitForSeconds(spawnSpeed);
            }
            if (gameStatistics.GetMapIndex() == 1) // oyuncunun g�rev yeri 2. b�lge ise �al���r
            {
                int RandomNPCIndex = Random.Range(0, npcs.Length);
                int randomSpawnpointsIndex = Random.Range(0, area2Spawnps.Length);
                // if (randomSpawnpointsIndex > 3)
                Instantiate(npcs[RandomNPCIndex], area2Spawnps[randomSpawnpointsIndex].transform.position, Quaternion.identity);
                yield return new WaitForSeconds(spawnSpeed);
            }
        }        
    }

    public Transform ExitPoints()
    {
        // ��k�� noktalar�n� rastgele �ekilde se�er ve geri d�nd�r�r.
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
        arrestedNPC.transform.position = finishAreaTeleportPoints[tpPointIndex].position + new Vector3(0f, 0.1f);
        tpPointIndex++;
    }

    public void QueryText(bool isOpening, string Texting)
    {
        // NPClerin yan�ndayken �st-ortada sorgula yaz�s�n� a��p kapat�r.
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
        AllowFKeyAndInteraction = true; // F tu�una basabilir.
        playerController.isAction = false; // Oyuncu hareket edebilir.
        mouseInput.mouseActivity = false; // kamera hareketleri aktif.
        OpenNPCsIdentity(false); // Kimlik g�z�k�yor ise kapat�r.
        OpenQuestionsPanel(false); // Soru panelini kapat�r.  
    }

    public void DisableFKeyandPlayerActions()
    {
        AllowFKeyAndInteraction = false; // F tu�una basamaz.
        playerController.isAction = true; // Oyuncu hareket edemez.
        mouseInput.mouseActivity = true; // kamera hareketleri kapal�.
        playerCanArrest = false;
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
        IdentityCard[0].text = identityNmbr.ToString(); // Kimlik numaras�
        IdentityCard[1].text = name; // �sim
        IdentityCard[2].text = surName; // Soyisim
        IdentityCard[3].text = rDay.ToString() + "." + rMonth.ToString() + "." + rYear.ToString();
    }

    public void OpenNPCsIdentity(bool isActive)
    {
        playerCanArrest = true;
        if (!identityDelimiter)
        {
            randomIdentity = Random.Range(0, 4);
            identityDelimiter = true;
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
        playerCanArrest = true;
    }

    public void ArrestNPC()
    {
        if (playerCanArrest)
        {
            fadePanel.alpha = 1;
            gameScore++;
            ArrestingNPC = true;
            identityDelimiter = false;
            OpenNPCsIdentity(false);
            QuestionsPanel.SetActive(false);
            arrestedInfoText.text = gameScore.ToString();
            playerCanArrest = false;
        }

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

    public void GoToMenu()
    {
        escPanel = false;

        if (criminalCount >= 4)
        {
            Statistics();
            SceneManager.LoadScene("Menu");
        }
        else
        {
            WarningPanel(true);
        }

    }

    public void GoDirectMenu()
    {
        Statistics();
        SceneManager.LoadScene("Menu");
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

    void DirectionalRotChanger(bool isFinished)
    {
        if (isFinished)
        {
            directionalLightRot.rotation = Quaternion.Euler(new Vector3(-2f, -218f));
        }
        else
        {
            directionalLightRot.rotation = Quaternion.Euler(new Vector3(50f, -30f));
        }
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

    public void VolumeAndSensValue()
    {
        mouseInput.mouseSensivity = settingsScript.GetMouseSensValue();

        foreach (AudioSource sfx in SFXs)
        {
            sfx.volume = settingsScript.GetSFXVolume();
        }

        foreach (AudioSource music in Musics)
        {
            music.volume = settingsScript.GetMusicVolume();
        }
    }

    public void Statistics()
    {
        if (criminalCount == 5)
        {
            gameStatistics.StatisticsSystem(true, true);
        }
        else if (criminalCount == 4)
        {
            gameStatistics.StatisticsSystem(true, false);
        }
        else
        {
            gameStatistics.StatisticsSystem(false, false);
        }
    }

    public void WarningPanel(bool isOpened)
    {
        if (isOpened)
        {
            warningPanel.SetActive(true);
            finishPanel.SetActive(false);
            gameOverPanel.SetActive(false);
        }
        else
        {
            warningPanel.SetActive(false);
        }
    }
}
