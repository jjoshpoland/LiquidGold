using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class SeasonManager : MonoBehaviour
{
    public float SeasonDuration = 240f;
    public float MarketSeasonDuration = 60f;
    int currentSeason = -1;
    int currentMarketSeason = -1;
    public Transform Sun;
    public float dayLightSpeed;
    public TMP_Text seasonText;
    public bool StartOnStart;
    public UnityEvent OnSeasonEnd;
    public UnityEvent OnSeasonStart;
    public UnityEvent OnMarketSeasonStart;
    public UnityEvent OnMarketSeasonEnd;
    public UnityEvent<float> OnSeasonProgress;
    public static SeasonManager singleton;
    float seasonStart;
    bool seasonActive;
    bool marketSeasonActive;
    
    public bool IsMarketSeason
    {
        get
        {
            return !seasonActive && marketSeasonActive;
        }
    }

    public int CurrentSeason
    {
        get
        {
            return currentSeason;
        }
    }

    private void Awake()
    {
        singleton = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if(StartOnStart)
        {
            StartNewSeason();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(seasonActive)
        {
            if(Time.time < seasonStart + SeasonDuration)
            {
                OnSeasonProgress.Invoke((Time.time - seasonStart) / SeasonDuration);
            }
            else
            {
                seasonActive = false;
                OnSeasonEnd.Invoke();
            }
        }
        else if(marketSeasonActive)
        {
            if (Time.time < seasonStart + MarketSeasonDuration)
            {
                OnSeasonProgress.Invoke((Time.time - seasonStart) / MarketSeasonDuration);
            }
            else
            {
                marketSeasonActive = false;
                OnMarketSeasonEnd.Invoke();
            }
        }
        Sun.transform.Rotate(new Vector3(dayLightSpeed * Time.deltaTime, 0, 0));

    }

    public void StartNewSeason()
    {
        
        currentSeason++;
        if (VictoryCondition.singleton.CheckVictory())
        {
            VictoryCondition.singleton.OnVictory.Invoke();
        }
        seasonActive = true;
        seasonStart = Time.time;
        seasonText.text = "Season " + (currentSeason + 1).ToString();
        OnSeasonStart.Invoke();
    }

    public void StartNewMarketSeason()
    {
        currentMarketSeason++;
        marketSeasonActive = true;
        seasonStart = Time.time;
        seasonText.text = "Market Season " + (currentMarketSeason + 1).ToString();
        OnMarketSeasonStart.Invoke();
    }
}
