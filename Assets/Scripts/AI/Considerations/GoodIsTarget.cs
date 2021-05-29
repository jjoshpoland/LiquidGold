using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoodIsTarget", menuName = "AI/Consideration/GoodIsTarget")]
public class GoodIsTarget : Consideration
{
    public Good good;
    public float trueValue;
    public float falseValue;
    public override float Evaluate(AI ai)
    {
        if(ai.targetGood == good)
        {
            return trueValue;
        }
        else
        {
            return falseValue;
        }
    }

    
}
