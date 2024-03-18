using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;

    bool settingsPanelIsOpened;


    public void PlayButton()
    {
        SceneManager.LoadScene("MainScene");
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

    public void QuitButton()
    {
        Application.Quit();
    }
}
