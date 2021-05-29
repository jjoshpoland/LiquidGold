using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HasGood", menuName = "AI/Consideration/HasGood")]
public class HasGood : Consideration
{
    public Good good;
    public override float Evaluate(AI ai)
    {
        if(ai.mainInventory.goods.Contains(good))
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

}
