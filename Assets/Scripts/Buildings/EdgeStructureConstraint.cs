using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EdgeConstructionConstraint", menuName = "StructureConstraint/EdgeConstraint")]
public class EdgeStructureConstraint : StructurePlacementConstraint
{
    /// <summary>
    /// If selected, only tiles on the edge will evaluate to true. If unselected, tiles on the edge will never evaluate to true.
    /// </summary>
    public bool AllowEdge;

    public override bool Evaluate(Tile targetPlacement, Building targetStructure)
    {

        Tile currentTile = targetPlacement;
        if (currentTile != null)
        {
            Tile[] neighbors = TileMap.singleton.Neighbors(targetPlacement.coords);
            if(neighbors == null)
            {
                Debug.LogWarning("could not fetch neighbors for coords " + targetPlacement.coords);
            }
            for (int i = 0; i < neighbors.Length; i++)
            {

                if (neighbors[i] == null)
                {
                    if(AllowEdge)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        return false;



    }
}
