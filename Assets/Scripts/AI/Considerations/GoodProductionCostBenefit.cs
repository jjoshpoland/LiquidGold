using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoodProductionCost", menuName = "AI/Consideration/GoodProductionCost")]
public class GoodProductionCostBenefit : Consideration
{
    public Recipe recipe;

    public override float Evaluate(AI ai)
    {
        if(recipe == null)
        {
            
        }
        float cost = 0;
        foreach(Good input in recipe.inputs)
        {
            cost += Market.singleton.GetPrice(input) / recipe.time;
        }

        float benefit = 0;
        foreach(Good output in recipe.outputs)
        {
            benefit += Market.singleton.GetPrice(output) / recipe.time;
        }

        if(cost == 0)
        {
            cost = 1;
        }

        if(ai.debugAI && debug)
        {
            Debug.Log(recipe + " cost benefit is " + (benefit / cost).ToString());
        }

        return benefit / cost;
    }
}
