using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LedgerUI : MonoBehaviour
{
    public Good good;
    public Player player;
    public TMP_Text quantity;
    private void Awake()
    {

        quantity = GetComponentInChildren<TMP_Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Market.singleton.OnOrderPlaced.AddListener(UpdateUI);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        
    }

    public void UpdateUI()
    {
        int total = 0;

        if (player.ledgers[SeasonManager.singleton.CurrentSeason].totalTrades.TryGetValue(good, out int value))
        {
            total = value;
        }
        quantity.text = total.ToString();
    }

    public void Buy()
    {
        if(player.inventory.AddGoods(new GoodQuantity(good, 1)))
        {
            player.inventory.Drachmae = player.inventory.Drachmae - Market.singleton.PlaceOrder(new GoodQuantity(good, 1), player);
        }
        
    }

    public void Sell()
    {
        if(player.inventory.PullGoods(new GoodQuantity(good, 1)))
        {
            player.inventory.Drachmae = player.inventory.Drachmae - Market.singleton.PlaceOrder(new GoodQuantity(good, -1), player);
        }
        
    }
}
