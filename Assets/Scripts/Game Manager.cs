using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int gameScore = 0;
    public GameObject[] npcs;
    public GameObject[] area1Spawnps, area2Spawnps; // npclerin b�lgelere g�re do�aca�� noktalar
    public GameObject[] exitpoints; // npclerin gidece�i noktalar
    public bool area1;

    void Start()
    {
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
            if (area1) // oyuncunun g�rev yeri 1. b�lge ise �al���r 
            {
                yield return new WaitForSeconds(1f);
                int RandomNPCIndex = Random.Range(0, npcs.Length);
                int randomSpawnpointsIndex = Random.Range(0, area1Spawnps.Length);
                Instantiate(npcs[RandomNPCIndex], area1Spawnps[randomSpawnpointsIndex].transform.position, Quaternion.identity);
                AddGameScore();
            }
            if (!area1) // oyuncunun g�rev yeri 2. b�lge ise �al���r
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
}
