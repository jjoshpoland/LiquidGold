using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isMainPlayer;
    public List<SeasonLedger> ledgers;
    public static Player mainPlayer;
    public GlobalInventory inventory;

    public int CurrentProfits
    {
        get
        {
            return ledgers[SeasonManager.singleton.CurrentSeason].totalProfits;
        }
    }
    private void Awake()
    {
        ledgers = new List<SeasonLedger>();
        AddLedger();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(isMainPlayer)
        {
            mainPlayer = this;
            inventory = GlobalInventory.singleton;
        }
        
        SeasonManager.singleton.OnSeasonStart.AddListener(AddLedger);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddLedger()
    {
        ledgers.Add(new SeasonLedger());
    }

    public void AddOrderToCurrentLedger(GoodQuantity order)
    {
        SeasonLedger currentLedger = ledgers[SeasonManager.singleton.CurrentSeason];

        if(currentLedger.totalTrades.TryGetValue(order.good, out int value))
        {
            currentLedger.totalTrades[order.good] = value + order.quantity;
        }
        else
        {
            currentLedger.totalTrades.Add(order.good, order.quantity);
        }

    }

    public void AddProfitsToCurrentLedger(int cost)
    {
        SeasonLedger currentLedger = ledgers[SeasonManager.singleton.CurrentSeason];
        currentLedger.totalProfits -= cost;
    }
}
