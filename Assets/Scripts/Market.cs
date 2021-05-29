using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Market : MonoBehaviour
{
    public List<GoodQuantity> Prices;
    public List<GoodQuantity> Quantities;
    Dictionary<Good, int> priceMap;
    Dictionary<Good, int> quantityMap;
    public static Market singleton;
    public UnityEvent OnOrderPlaced;
    public UnityEvent OnPricesUpdated;
    private void Awake()
    {
        singleton = this;
        priceMap = new Dictionary<Good, int>();
        quantityMap = new Dictionary<Good, int>();


        foreach (GoodQuantity gq in Quantities)
        {
            quantityMap.Add(gq.good, gq.quantity);
        }
        UpdatePrices();
        SeasonManager.singleton.OnMarketSeasonStart.AddListener(UpdatePrices);
        //foreach (GoodQuantity gq in Prices)
        //{
        //    priceMap.Add(gq.good, GetPrice(gq.good));

        //}


        Player[] players = FindObjectsOfType<Player>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePrices()
    {
        for (int i = 0; i < Prices.Count; i++)
        {
            GoodQuantity gq = Prices[i];
            int newPrice = CalculatePrice(quantityMap[gq.good]);
            gq.quantity = newPrice;
            Quantities[i].quantity = quantityMap[gq.good];
            priceMap[gq.good] = newPrice;
            //Debug.Log(gq.good + " quantity is now " + quantityMap[gq.good]);
        }

        OnPricesUpdated.Invoke();
    }

    int CalculatePrice(int quantity)
    {
        float realQuantity = quantity;
        if(realQuantity == 0)
        {
            realQuantity = 1;
        }
        else if(realQuantity < 0)
        {
            realQuantity = 1f / Mathf.Abs(realQuantity);
        }
        float price = 1f / realQuantity;
        price *= 1000f;
        return Mathf.RoundToInt(price);
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
            quantityMap[gq.good] -= gq.quantity;
            player.AddOrderToCurrentLedger(gq);
        }
        player.AddProfitsToCurrentLedger(cost);
        OnOrderPlaced.Invoke();
        UpdatePrices();
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
        quantityMap[order.good] -= order.quantity;
        player.AddOrderToCurrentLedger(order);

        player.AddProfitsToCurrentLedger(cost);
        OnOrderPlaced.Invoke();
        UpdatePrices();
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
