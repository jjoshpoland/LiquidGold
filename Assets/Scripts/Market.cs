using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Market : MonoBehaviour
{
    public List<GoodQuantity> Prices;
    Dictionary<Good, int> priceMap;
    Dictionary<Good, int> demand;
    public static Market singleton;
    public UnityEvent OnOrderPlaced;
    private void Awake()
    {
        singleton = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        priceMap = new Dictionary<Good, int>();
        demand = new Dictionary<Good, int>();
        foreach(GoodQuantity gq in Prices)
        {
            priceMap.Add(gq.good, gq.quantity);
            demand.Add(gq.good, 0);
        }

        Player[] players = FindObjectsOfType<Player>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetPrice(Good good)
    {
        if(priceMap.TryGetValue(good, out int value))
        {
            return value;
        }
        else
        {
            return 0;
        }
    }

    public void UpdateMarket(List<GoodQuantity> newPrices)
    {
        foreach(GoodQuantity price in newPrices)
        {
            priceMap[price.good] = price.quantity;
        }

        foreach(GoodQuantity price in Prices)
        {
            price.quantity = priceMap[price.good];
        }
    }

    /// <summary>
    /// Stores the order information and returns the cost
    /// </summary>
    /// <param name="order"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    public int PlaceOrder(List<GoodQuantity> order, Player player)
    {
        int cost = 0;
        foreach(GoodQuantity gq in order)
        {
            cost += priceMap[gq.good] * gq.quantity;
            demand[gq.good] += gq.quantity;
            player.AddOrderToCurrentLedger(gq);
        }
        player.AddProfitsToCurrentLedger(cost);
        OnOrderPlaced.Invoke();
        return cost;
    }

    /// <summary>
    /// Stores the order information and returns the cost
    /// </summary>
    /// <param name="order"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    public int PlaceOrder(GoodQuantity order, Player player)
    {
        int cost = priceMap[order.good] * order.quantity;
        demand[order.good] += order.quantity;
        player.AddOrderToCurrentLedger(order);

        player.AddProfitsToCurrentLedger(cost);
        OnOrderPlaced.Invoke();
        return cost;
        
    }
}

public class SeasonLedger
{
    public Dictionary<Good, int> totalTrades;
    public int totalProfits;

    public SeasonLedger()
    {
        totalTrades = new Dictionary<Good, int>();
    }
}
