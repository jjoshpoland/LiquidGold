using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoodAvailability", menuName = "AI/Consideration/GoodAvailabilty")]
public class GoodAvailability : Consideration
{
    public Good good;
    public override float Evaluate(AI ai)
    {
        float baseAvailability = 0f - Market.singleton.GetPrice(good);
        Inventory mainInventory = ai.mainInventory;

        if(mainInventory != null)
        {
            foreach(Building b in ai.buildings)
            {
                if(b.TryGetComponent<Producer>(out Producer producer))
                {
                    if(producer.productionRecipe.outputs.Contains(good))
                    {
                        baseAvailability += (Market.singleton.GetPrice(good) / producer.productionRecipe.time);
                    }
                }
            }
            

            foreach(Good g in mainInventory.goods)
            {
                if(g == good)
                {
                    baseAvailability++;
                }
            }
        }

        return baseAvailability;
    }

    
}
