using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LedgerTotalUI : MonoBehaviour
{
    public Player player;
    public Good good;
    public TMP_Text text;
    public TotalType type;
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
        if(type == TotalType.Profit)
        {
            text.text = player.inventory.Drachmae.ToString();
        }
        else if(type == TotalType.Goods)
        {
            text.text = player.ledgers[SeasonManager.singleton.CurrentSeason].totalTrades[good].ToString();
        }
        else if(type == TotalType.TotalOfTotalProfits)
        {
            int total = 0;
            Player[] players = FindObjectsOfType<Player>();
            for (int i = 0; i < players.Length; i++)
            {
                total += players[i].CurrentProfits;
            }

            text.text = total.ToString();
        }
        else if(type == TotalType.TotalOfTotalGoods)
        {
            int total = 0;
            Player[] players = FindObjectsOfType<Player>();
            for (int i = 0; i < players.Length; i++)
            {
                if(players[i].ledgers[SeasonManager.singleton.CurrentSeason].totalTrades.TryGetValue(good, out int value))
                {
                    total += value;
                }
                
            }

            text.text = total.ToString();
        }
    }
}

public enum TotalType
{
    Goods, Profit, TotalOfTotalProfits, TotalOfTotalGoods
}
