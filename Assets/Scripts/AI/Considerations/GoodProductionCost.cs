using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoodProductionCost", menuName = "AI/Consideration/GoodProductionCost")]
public class GoodProductionCost : Consideration
{
    public Recipe recipe;

    public override float Evaluate(AI ai)
    {
        float price = 0;
        foreach(Good input in recipe.inputs)
        {
            price -= Market.singleton.GetPrice(input);
        }

        return price;
    }
}
