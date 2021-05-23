using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Inventory))]
public class Resource : MonoBehaviour
{
    public Inventory inventory;

    public UnityEvent OnThreeQuarterFull;
    public UnityEvent OnHalfFull;
    public UnityEvent OnQuarterFull;
    public UnityEvent OnEmpty;

    public void CheckRemainingResources()
    {
        float remainingPercent = inventory.goods.Count / inventory.maxCapacity;

        if(remainingPercent >= .75f)
        {

        }
        else if(remainingPercent >= .5f)
        {
            OnThreeQuarterFull.Invoke();
        }
        else if(remainingPercent >= .25f)
        {
            OnHalfFull.Invoke();
        }
        else if(remainingPercent > 0)
        {
            OnQuarterFull.Invoke();
        }
        else
        {
            OnEmpty.Invoke();
        }
    }
}
