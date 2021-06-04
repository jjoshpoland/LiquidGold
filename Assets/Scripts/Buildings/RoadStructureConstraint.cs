using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoadConstructionConstraint", menuName = "StructureConstraint/RoadConstraint")]
public class RoadStructureConstraint : StructurePlacementConstraint
{
    public override bool Evaluate(Tile targetPlacement, Building targetStructure)
    {

        Tile currentTile = targetPlacement;
        if (currentTile != null)
        {
            Tile[] neighbors = TileMap.singleton.Neighbors(targetPlacement.coords);
            if (neighbors == null)
            {
                Debug.LogWarning("could not fetch neighbors for coords " + targetPlacement.coords);
            }
            //search cardinal directions only
            for (int i = 0; i < neighbors.Length; i = i + 2)
            {

                if (neighbors[i] != null)
                {
                    if (neighbors[i].gameObject.layer == LayerMask.NameToLayer("Road"))
                    {
                        return true;
                    }
                }
            }
        }

        return false;



    }
}
