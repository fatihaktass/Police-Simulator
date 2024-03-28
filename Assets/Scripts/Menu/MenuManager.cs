using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject statisticsPanel;
    [SerializeField] GameObject actionButtons;
    [SerializeField] GameObject statisticsMenuNpc;
    [SerializeField] GameObject mainMenuNPCs;

    bool settingsPanelIsOpened;


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
        mainMenuNPCs.SetActive(false);
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
        mainMenuNPCs.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }


}