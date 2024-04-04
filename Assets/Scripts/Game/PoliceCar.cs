using System.Collections;
using UnityEngine;

public class PoliceCar : MonoBehaviour
{
    [SerializeField] GameObject[] pointLights;
    GameStatistics gameStatistics;
    int lightIndex = 0;

    void Start()
    {
        gameStatistics = FindAnyObjectByType<GameStatistics>(); 
        StartCoroutine(LightChanger());
    }
     
    IEnumerator LightChanger()
    {
        while (gameStatistics.GetMapIndex() == 0)
        {
            lightIndex++;
            if(lightIndex > 1)
                lightIndex = 0;

            pointLights[1 - lightIndex].SetActive(false);
            pointLights[lightIndex].SetActive(true);
            yield return new WaitForSeconds(.3f);
        }
    }
}
