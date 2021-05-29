using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoodAvailability", menuName = "AI/Consideration/GoodAvailabilty")]
public class GoodAvailability : Consideration
{
    public Good good;
    
    public override float Evaluate(AI ai)
    {
        float availableQuantity = 0f;
        float currentDemand= 0f;
        Inventory mainInventory = ai.mainInventory;

        if(mainInventory != null)
        {
            foreach(Building b in ai.buildings)
            {
                if(b.TryGetComponent<Producer>(out Producer producer))
                {
                    for (int i = 0; i < producer.productionRecipe.outputs.Count; i++)
                    {
                        if (producer.productionRecipe.outputs[i] == good)
                        {
                            availableQuantity += (1f / producer.productionRecipe.time);
                        }
                    }

                    for (int i = 0; i < producer.productionRecipe.inputs.Count; i++)
                    {
                        if(producer.productionRecipe.inputs[i] == good)
                        {
                            currentDemand += (1f / producer.productionRecipe.time);
                        }
                    }
                }
            }
            

            foreach(Good g in mainInventory.goods)
            {
                if(g == good)
                {
                    availableQuantity++;
                }
            }

            
        }

        if(ai.debugAI)
        {
            Debug.Log(good + " available quantity: " + availableQuantity + ", current demand: " + currentDemand);
        }

        if(currentDemand == 0)
        {
            if(availableQuantity > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        return availableQuantity / currentDemand;
    }

    
}
