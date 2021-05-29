using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingCostBenefit", menuName = "AI/Consideration/BuildingCostBeneft")]
public class BuildingCostBenefit : Consideration
{
    public GoodProductionCostBenefit productionConsideration;
    public Building building;
    public float breakEvenTimeGoal = 30f;
    public override float Evaluate(AI ai)
    {
        float buildCost = 0;
        foreach(GoodQuantity gq in building.cost)
        {
            buildCost += Market.singleton.GetPrice(gq.good) * gq.quantity;
        }

        float buildBenefit = productionConsideration.Evaluate(ai) * breakEvenTimeGoal;

        if(ai.debugAI && debug)
        {
            Debug.Log(building + " cost=" + buildCost + " and benefit over" + breakEvenTimeGoal + " seconds is " + buildBenefit);
        }

        return buildBenefit / buildCost;
    }

}
