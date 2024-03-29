using UnityEngine;

public class GameStatistics : MonoBehaviour
{
    static int _Maps;
    static int _DayCount;
    static int _SuccessDayCount;
    static float _RankPoints;

    void Start()
    {
        if (PlayerPrefs.HasKey("Maps"))
            _Maps = PlayerPrefs.GetInt("Maps");
        else
        {
            _Maps = 0;
            PlayerPrefs.SetInt("Maps", _Maps);
        }

        if (PlayerPrefs.HasKey("DayCount"))
            _DayCount = PlayerPrefs.GetInt("DayCount");
        else
        {
            _DayCount = 0;
            PlayerPrefs.SetInt("DayCount", _DayCount);
        }

        if (PlayerPrefs.HasKey("SuccessDayCount"))
            _SuccessDayCount = PlayerPrefs.GetInt("SuccessDayCount");
        else
        {
            _SuccessDayCount = 0;
            PlayerPrefs.SetInt("SuccessDayCount", _SuccessDayCount);
        }

        if (PlayerPrefs.HasKey("RankPoints"))
            _RankPoints = PlayerPrefs.GetFloat("RankPoints");
        else
        {
            _RankPoints = 0;
            PlayerPrefs.SetFloat("RankPoints", _RankPoints);
        }
    }

    public void StatisticsSystem(bool isSuccessful, bool isFull)
    {
        if (isSuccessful)
        {
            _SuccessDayCount++;
            _DayCount++;

            if (isFull)
                _RankPoints += Random.Range(100f, 150f);
            else
                _RankPoints += Random.Range(60f, 100f);

            PlayerPrefs.SetInt("SuccessDayCount", _SuccessDayCount);
        }
        else
        {
            _DayCount++;
            _RankPoints -= Random.Range(10f, 40f);
        }


        PlayerPrefs.SetFloat("RankPoints", _RankPoints);
        PlayerPrefs.SetInt("DayCount", _DayCount);
    }

    public int GetMapIndex()
    {
        return PlayerPrefs.GetInt("Maps");
    }

    public void SetMapIndex(int _mapIndex)
    {
        if (_DayCount >= 9)
        {
            _Maps = _mapIndex;
            PlayerPrefs.SetInt("Maps", _Maps);
        }
    }

    public int GetDayCount()
    {
       return PlayerPrefs.GetInt("DayCount");
    }
    public int GetSuccessDayCount()
    {
        return PlayerPrefs.GetInt("SuccessDayCount");
    }

    public float GetRankPoints()
    {
        return PlayerPrefs.GetFloat("RankPoints");
    }
}
