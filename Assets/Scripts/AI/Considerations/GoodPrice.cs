using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoodPrice", menuName = "AI/Consideration/GoodPrice")]
public class GoodPrice : Consideration
{
    public Good good;
    public override float Evaluate(AI ai)
    {
        return Market.singleton.GetPrice(good);
    }

}
