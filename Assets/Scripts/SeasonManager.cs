using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SeasonManager : MonoBehaviour
{
    public float SeasonDuration = 240f;
    public UnityEvent OnSeasonEnd;
    public UnityEvent OnSeasonStart;
    public UnityEvent<float> OnSeasonProgress;
    public static SeasonManager singleton;
    float seasonStart;
    bool seasonActive;

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
        if(Time.time < seasonStart + SeasonDuration && seasonActive)
        {
            OnSeasonProgress.Invoke((Time.time - seasonStart) / SeasonDuration);   
        }
        else
        {
            seasonActive = false;
            OnSeasonEnd.Invoke();
        }
    }

    public void StartNewSeason()
    {
        seasonActive = true;
        seasonStart = Time.time;
        OnSeasonStart.Invoke();
    }
}
