using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checks all neghbors for the provided tile and determines if they match the required tile type
/// </summary>
[CreateAssetMenu(fileName = "NeighborTerrainConstructionConstraint", menuName = "StructureConstraint/NeighborTerrainConstraint")]
public class NeighborTerrainStructureConstraint : StructurePlacementConstraint
{
    public TileType requiredType;

    public override bool Evaluate(Tile targetPlacement, Building targetStructure)
    {

        Tile currentTile = targetPlacement;
        if (currentTile != null)
        {
            Tile[] neighbors = TileMap.singleton.Neighbors(targetPlacement.coords);
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (neighbors[i] != null && neighbors[i].type == requiredType)
                {
                    return true;
                }
            }
        }

        return false;


        
    }
}
