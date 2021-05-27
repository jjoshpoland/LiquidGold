using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{
    public List<GoodQuantity> Prices;
    Dictionary<Good, int> priceMap;
    Dictionary<Good, int> demand;
    public static Market singleton;
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
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public int PlaceOrder(List<GoodQuantity> order)
    {
        int cost = 0;
        foreach(GoodQuantity gq in order)
        {
            cost += priceMap[gq.good];
            demand[gq.good] += gq.quantity;
        }

        return cost;
    }
}
