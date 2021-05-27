using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Tile
{
    public GameObject ghostPrefab;
    public List<StructurePlacementConstraint> constraints;
    public List<GoodQuantity> cost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Checks all structure placement constraints and returns false if any of them evaluate to be false
    /// </summary>
    /// <param name="targetTile"></param>
    /// <returns></returns>
    public bool EvaluateConstraints(Tile targetTile)
    {
        foreach(StructurePlacementConstraint constraint in constraints)
        {
            if (!constraint.Evaluate(targetTile, this)) return false;
        }

        return true;
    }

    public bool HasRoadAccess()
    {
        Tile[] neighbors = TileMap.singleton.Neighbors(this.coords);
        for (int i = 0; i < neighbors.Length; i++)
        {

            if(neighbors[i] != null && neighbors[i].gameObject.layer == LayerMask.NameToLayer("Road"))
            {
                return true;
            }
        }
        

        return false;
    }
}

[System.Serializable]
public class GoodQuantity
{
    public Good good;
    public int quantity;
}
