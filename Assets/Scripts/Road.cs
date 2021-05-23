using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Road : Building
{
    public int lineCapacity = 4;
    public List<Transport> cardinalLine;
    public List<Transport> antiCardinalLine;

    public Transform[] cardinalNodes;
    public Transform[] antiCardinalNodes;


    private void Update()
    {
        
    }

    //public bool Enter(Transport transport, bool cardinal)
    //{
    //    if (Contains(transport)) return false;

    //    if(cardinal)
    //    {
    //        if(cardinalLine.Count >= lineCapacity)
    //        {
    //            return false;
    //        }
    //        else
    //        {
    //            return true;
    //        }
    //    }
    //}

    public bool Contains(Transport transport)
    {
        if(cardinalLine.Contains(transport))
        {
            return true;
        }
        else if(antiCardinalLine.Contains(transport))
        {
            return true;
        }

        return false;
    }
}
