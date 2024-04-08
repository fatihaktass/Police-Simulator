using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject statisticsPanel;
    [SerializeField] GameObject actionButtons;
    [SerializeField] GameObject statisticsMenuNpc;
    [SerializeField] GameObject mainMenuNpcs;
    [SerializeField] GameObject Area1Map, Area2Map;
    [SerializeField] GameObject rankboard;
    [SerializeField] GameObject infoPanel;
    [SerializeField] Slider rankboardSlider;

    bool settingsPanelIsOpened;
    bool infoButtonPressed;
    bool rankboardIsOpened;
    static bool mapChangePerm;

    GameStatistics gameStatistics;

    [Header("Statistics Objects")]
    [SerializeField] TextMeshProUGUI dayCountText;
    [SerializeField] TextMeshProUGUI successDayCountText;
    [SerializeField] TextMeshProUGUI rankPointsText;
    [SerializeField] TextMeshProUGUI playerRankText;
    [SerializeField] TextMeshProUGUI reminderText;

    private void Start()
    {
        gameStatistics = GetComponent<GameStatistics>();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void StatisticsButton()
    {
        if (settingsPanelIsOpened)
        {
            settingsPanelIsOpened = false;
            settingsPanel.SetActive(false);
        }
        actionButtons.SetActive(false);
        mainMenuNpcs.SetActive(false);
        statisticsMenuNpc.SetActive(true);
        statisticsMenuNpc.GetComponent<StatisticsMenuNPC>().StartRot();
        statisticsPanel.SetActive(true);
    }

    public void SettingsButton()
    {
        settingsPanelIsOpened = !settingsPanelIsOpened;

        if (settingsPanelIsOpened)
        {
            settingsPanel.SetActive(true);
        }
        else
        {
            settingsPanel.SetActive(false);
        }
    }

    public void BackButton()
    {
        statisticsPanel.SetActive(false);
        statisticsMenuNpc.SetActive(false);
        actionButtons.SetActive(true);
        mainMenuNpcs.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void GetAllStatistic()
    {
        dayCountText.text = gameStatistics.GetDayCount().ToString();
        successDayCountText.text = gameStatistics.GetSuccessDayCount().ToString();
        rankPointsText.text = "Rütbe Puaný: " + gameStatistics.GetRankPoints().ToString("N1");
        rankboardSlider.value = gameStatistics.GetRankPoints();

        if (gameStatistics.GetMapIndex() == 0)
            MapChanger(false);
        else
            MapChanger(true);

        Ranks();
    }

    public void MapChanger(bool isPositive)
    {
        if (isPositive)
        {
            if (mapChangePerm)
            {
                Area1Map.SetActive(false);
                Area2Map.SetActive(true);
                gameStatistics.SetMapIndex(1);
            }
            else
            {
                reminderText.alpha = 255f;
                reminderText.text = "Yeni görev yeri için en az Kýdemli Memur rütbesinde olmalýsýnýz.";
                reminderText.CrossFadeAlpha(1f, 0f, false);
                reminderText.CrossFadeAlpha(0f, 1f, false);
            }
            
        }
        else
        {
            Area2Map.SetActive(false);
            Area1Map.SetActive(true);
            gameStatistics.SetMapIndex(0);
        }
    }

    void Ranks()
    {
        if (gameStatistics.GetRankPoints() < 2000)
        {
            playerRankText.text = "Memur";
        }
        else if (gameStatistics.GetRankPoints() >= 2000 && gameStatistics.GetRankPoints() < 4250)
        {
            mapChangePerm = true;
            playerRankText.text = "K. Memur";
        }
        else if (gameStatistics.GetRankPoints() >= 4250 && gameStatistics.GetRankPoints() < 6200)
        {
            playerRankText.text = "Komiser";
        }
        else if (gameStatistics.GetRankPoints() >= 6200 && gameStatistics.GetRankPoints() < 10000)
        {
            playerRankText.text = "B.Komiser";
        }
        else if (gameStatistics.GetRankPoints() >= 10000)
        {
            playerRankText.text = "Emniyet Müdürü";
        }
    }

    public void Rankboard()
    {
        rankboardIsOpened = !rankboardIsOpened;

        if (rankboardIsOpened)
        {
            infoButtonPressed = false;
            infoPanel.SetActive(false);
            rankboard.SetActive(true);
        }
        else
            rankboard.SetActive(false);
    }
    
    public void InfoButton()
    {
        infoButtonPressed = !infoButtonPressed;

        if (infoButtonPressed)
        {
            rankboardIsOpened = false;
            rankboard.SetActive(false);
            infoPanel.SetActive(true);
        }
            
        else 
            infoPanel.SetActive(false);
    }
}
