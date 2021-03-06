using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RemainingCapacity", menuName = "AI/Consideration/RemainingCapacity")]
public class RemainingCapacity : Consideration
{
    public override float Evaluate(AI ai)
    {
        float remainingCapacity = ai.mainInventory.remainingCapacity;

        return 5f - remainingCapacity;
    }
}
