using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public Transform policeTransform; 

    StatisticsMenuNPC statisticsNPC;
    Vector3 StartPos;

    void Start()
    {
        statisticsNPC = policeTransform.gameObject.GetComponent<StatisticsMenuNPC>();
        StartPos = transform.position;
    }
    
    void Update()
    {
        if (statisticsNPC.allowForMove)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, policeTransform.position.z + -2.34f);
        }
        else
            transform.position = StartPos;
    }

    
}
