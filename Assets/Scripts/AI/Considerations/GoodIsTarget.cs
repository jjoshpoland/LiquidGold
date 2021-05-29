using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoodIsTarget", menuName = "AI/Consideration/GoodIsTarget")]
public class GoodIsTarget : Consideration
{
    public Good good;
    public override float Evaluate(AI ai)
    {
        if(ai.targetGood == good)
        {
            return 1;
        }
        else
        {
            return .5f;
        }
    }

    
}
