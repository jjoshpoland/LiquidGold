using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Resource : MonoBehaviour
{
    public Good type;
    public List<Good> goods;
    public int maxCapacity;

    public UnityEvent OnThreeQuarterFull;
    public UnityEvent OnHalfFull;
    public UnityEvent OnQuarterFull;
    public UnityEvent OnEmpty;

    public void Start()
    {
        for (int i = 0; i < maxCapacity; i++)
        {
            goods.Add(type);
        }
    }

    public void CheckRemainingResources()
    {
        float remainingPercent = goods.Count / maxCapacity;

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
