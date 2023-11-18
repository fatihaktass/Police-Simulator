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
    public Transform[] area1ExitPoints, area2ExitPoints; // npclerin gideceði noktalar

    public bool area1; // Hangi bölgede görev yapacaðýný sorguluyor.
    public bool AllowFKey = true; // F tuþuna basmayý kýsýtlýyor.

    public TextMeshProUGUI QueryTMP;
    public Transform playerTransform;
    public GameObject InteractPanel, NpcsIdentityPanel, QuestionsPanel;
    public GameObject area1Collider, area2Collider;
    public Button identityButton, questionButton, arrestButton, releaseButton;

    public PlayerController playerController;
    public MouseInput mouseInput;

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        mouseInput = FindFirstObjectByType<MouseInput>();
        StartCoroutine(NPCSpawner());

        if (area1)
        {
            playerTransform.position = new Vector3(-7.31151f, 0.9f, -123.5954f);
            area1Collider.SetActive(false);
            area2Collider.SetActive(true);
        }
        if (!area1)
        {
            playerTransform.position = new Vector3(6.6f, 0.9f, 0.85f);
            area1Collider.SetActive(true);
            area2Collider.SetActive(false);
        }
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

    public Transform ExitPoints()
    {
        if (area1)
        {   // Birinci bölgede olan çýkýþ noktalarýný rastgele þekilde seçer ve geri döndürür.
            int RandomExitPoints = Random.Range(0, area1ExitPoints.Length);
            return area1ExitPoints[RandomExitPoints];
        }
        if (!area1)
        {
            // Ýkinci bölgede olan çýkýþ noktalarýný rastgele þekilde seçer ve geri döndürür.
            int RandomExitPoints = Random.Range(0, area2ExitPoints.Length);
            return area2ExitPoints[RandomExitPoints];
        }
        return null;
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

    public void EnableFKeyandPlayerActions()
    {
        AllowFKey = true; // F tuþuna basabilir.
        playerController.isAction = false; // Oyuncu hareket edebilir.
        mouseInput.mouseActivity = false; // kamera hareketleri aktif.
        OpenNPCsIdentity(false); // Kimlik gözüküyor ise kapatýr.
        OpenQuestionsPanel(false); // Soru panelini kapatýr.  
    }

    public void DisableFKeyandPlayerActions()
    {
        AllowFKey = false; // F tuþuna basamaz.
        playerController.isAction = true; // Oyuncu hareket edemez.
        mouseInput.mouseActivity = true; // kamera hareketleri kapalý.
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

    public void OpenNPCsIdentity(bool isActive)
    {
        NpcsIdentityPanel.SetActive(isActive);
    }

    public void OpenQuestionsPanel(bool isActive)
    {
        QuestionsPanel.SetActive(isActive);
    }

    public void ReleaseNpc()
    {
        EnableFKeyandPlayerActions();
        Cursor.lockState = CursorLockMode.Locked;
        InteractPanel.SetActive(false);
    }
}
