using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int gameScore = 0;

    public GameObject[] npcs;
    public GameObject[] area1Spawnps, area2Spawnps; // npclerin bölgelere göre doðacaðý noktalar
    public GameObject[] exitpoints; // npclerin gideceði noktalar

    public bool area1; // Hangi bölgede görev yapacaðýný sorguluyor.
    public bool AllowFKey = true; // F tuþuna basmayý kýsýtlýyor.

    public TextMeshProUGUI QueryTMP;
    public GameObject InteractPanel, NpcsIdentityPanel;
    public Button identityButton, questionButton, arrestButton, releaseButton;

    public PlayerController playerController;
    public MouseInput mouseInput;

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        mouseInput = FindFirstObjectByType<MouseInput>();
        StartCoroutine(NPCSpawner());
    }

    public void AddGameScore()
    {
        gameScore++;
    }

    IEnumerator NPCSpawner()
    {
        while (gameScore < 5)
        {
            if (area1) // oyuncunun görev yeri 1. bölge ise çalýþýr 
            {
                yield return new WaitForSeconds(1f);
                int RandomNPCIndex = Random.Range(0, npcs.Length);
                int randomSpawnpointsIndex = Random.Range(0, area1Spawnps.Length);
                Instantiate(npcs[RandomNPCIndex], area1Spawnps[randomSpawnpointsIndex].transform.position, Quaternion.identity);
                AddGameScore();
            }
            if (!area1) // oyuncunun görev yeri 2. bölge ise çalýþýr
            {
                 yield return new WaitForSeconds(1f);
                 int RandomNPCIndex = Random.Range(0, npcs.Length);
                 int randomSpawnpointsIndex = Random.Range(0, area2Spawnps.Length);
                 Instantiate(npcs[RandomNPCIndex], area2Spawnps[randomSpawnpointsIndex].transform.position, Quaternion.identity);
                 AddGameScore();
            }
        }
    }

    public GameObject ExitPoints()
    {
        int RandomExitPoints = Random.Range(0,exitpoints.Length); 
        return exitpoints[RandomExitPoints];
    }

    public void QueryText(bool isOpening)
    {
        // NPClerin yanýndayken üst-ortada sorgula yazýsýný açýp kapatýr.
        if (isOpening)
        {
            QueryTMP.alpha = 1f;
        }
        if (!isOpening)
        {
            float currentValue = QueryTMP.alpha -= 3f * Time.deltaTime;
            currentValue = Mathf.Clamp01(currentValue);
            QueryTMP.alpha = currentValue;
        }
    }

    public bool FKeyandPlayerActions(bool active)
    {
        if (active)
        {
            AllowFKey = true;
            playerController.isAction = false; // Oyuncu hareket edebilir.
            mouseInput.mouseActivity = false; // kamera hareketleri aktif.
            NPCsIdentity(false); // Kimlik gözüküyor ise kapatýr.
        }
        if (!active)
        {
            AllowFKey = false;
            playerController.isAction = true; // Oyuncu hareket edemez.
            mouseInput.mouseActivity = true; // kamera hareketleri kapalý.
        }
        return AllowFKey;
    }

    public void OpenInteractPanel(bool isActive)
    {
        if (isActive)
        {
            InteractPanel.SetActive(true);
        }
        if (!isActive)
        {
            InteractPanel.SetActive(false);
        }
    }

    public void ReleaseNpc()
    {
        FKeyandPlayerActions(true);
        Cursor.lockState = CursorLockMode.Locked;
        InteractPanel.SetActive(false);
    }

    public void NPCsIdentity(bool isActive)
    {
        NpcsIdentityPanel.SetActive(isActive);
    }
}
