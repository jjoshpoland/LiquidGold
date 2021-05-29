using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SeasonManager : MonoBehaviour
{
    public float SeasonDuration = 240f;
    public float MarketSeasonDuration = 60f;
    int currentSeason = -1;
    int currentMarketSeason = -1;
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
        StartNewSeason();
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

    }

    public void StartNewSeason()
    {
        currentSeason++;
        seasonActive = true;
        seasonStart = Time.time;
        OnSeasonStart.Invoke();
    }

    public void StartNewMarketSeason()
    {
        currentMarketSeason++;
        marketSeasonActive = true;
        seasonStart = Time.time;
        OnMarketSeasonStart.Invoke();
    }
}
