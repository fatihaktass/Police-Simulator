using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject statisticsPanel;
    [SerializeField] GameObject actionButtons;
    [SerializeField] GameObject statisticsMenuNpc;
    [SerializeField] GameObject mainMenuNpcs;
    [SerializeField] GameObject Area1Map, Area2Map;
    [SerializeField] GameObject rankboard;

    bool settingsPanelIsOpened;
    bool rankboardIsOpened;

    GameStatistics gameStatistics;

    [Header("Statistics Objects")]
    [SerializeField] TextMeshProUGUI dayCountText;
    [SerializeField] TextMeshProUGUI successDayCountText;
    [SerializeField] TextMeshProUGUI rankPointsText;

    private void Start()
    {
        gameStatistics = GetComponent<GameStatistics>();
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
        rankPointsText.text = "Rütbe Puaný: " + gameStatistics.GetRankPoints().ToString();
    }

    public void MapChanger(bool isPositive)
    {
        if (isPositive)
        {
            Area1Map.SetActive(false); 
            Area2Map.SetActive(true);
            gameStatistics.SetMapIndex(1);
        }
        else
        {
            Area2Map.SetActive(false);
            Area1Map.SetActive(true);
            gameStatistics.SetMapIndex(0);
        }
    }

    public void Rankboard()
    {
        rankboardIsOpened = !rankboardIsOpened;

        if (rankboardIsOpened)
            rankboard.SetActive(true);
        else
            rankboard.SetActive(false);
    }
}
