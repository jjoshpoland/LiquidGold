using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VictoryCondition : MonoBehaviour
{
    public bool moneyTarget;
    public bool timeTarget;
    public bool competitionTarget;

    public int MinSeasons;
    public int TargetMoney;

    public Player mainPlayer;
    public List<Player> AIs;
    public UnityEvent OnVictory;
    public UnityEvent OnDefeat;

    public static VictoryCondition singleton;
    private void Awake()
    {
        singleton = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CheckVictory()
    {
        bool winning = true;
        if(moneyTarget)
        {
            if(mainPlayer.inventory.Drachmae < TargetMoney)
            {
                winning = false;
            }
        }
        if (competitionTarget)
        {
            int bestMoney = int.MinValue;
            foreach (Player ai in AIs)
            {
                if (ai.inventory.Drachmae > mainPlayer.inventory.Drachmae)
                {
                    winning = false;
                }
            }
        }
        if (timeTarget)
        {
            if(SeasonManager.singleton.CurrentSeason < MinSeasons)
            {
                winning = false;
            }
            else
            {
                if(SeasonManager.singleton.CurrentSeason > MinSeasons && winning == false)
                {
                    OnDefeat.Invoke();
                }
            }
        }




        return winning;
    }

    public void EndLevel()
    {
        LevelManager.singleton.LoadLevel(0);
    }
}
