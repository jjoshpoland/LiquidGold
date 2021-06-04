using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIBuyer : MonoBehaviour
{
    public List<Good> goods;
    [Range(0f, 1f)]
    public float share = .15f;
    public float rate = 15f;
    [Range(0f, 1f)]
    public float eventChance = .05f;
    public int quantityThreshold = 70;
    public int surplusAmount = -250;
    float lastBuy;
    public UnityEvent<Good> OnCrash;
    public UnityEvent<Good> OnSurplus;
    // Start is called before the first frame update
    void Start()
    {
        lastBuy = Time.time;   
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > lastBuy + rate)
        {
            if(Random.Range(0f, 1f) < eventChance)
            {
                Good targetGood = goods[Random.Range(0, goods.Count)];

                int quantity = Market.singleton.GetQuantity(targetGood);
                if(quantity > quantityThreshold)
                {
                    //crash
                    PlaceTargetedBuy(targetGood, .75f);
                    OnCrash.Invoke(targetGood);
                }
                else
                {
                    //surplus
                    PlaceSurplusSell(targetGood);
                    OnSurplus.Invoke(targetGood);
                }
            }

            foreach(Good good in goods)
            {
                PlaceTargetedBuy(good, share);
            }
            lastBuy = Time.time;
        }
    }

    void PlaceTargetedBuy(Good good, float marketShare)
    {
        int quantity = Market.singleton.GetQuantity(good);

        float targetQuantity = quantity * marketShare;

        Market.singleton.PlaceOrder(new GoodQuantity(good, Mathf.RoundToInt(targetQuantity)), null);
    }

    void PlaceSurplusSell(Good good)
    {
        Market.singleton.PlaceOrder(new GoodQuantity(good, surplusAmount), null);
    }
}
