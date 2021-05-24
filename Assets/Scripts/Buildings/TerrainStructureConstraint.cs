using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checks if the targeted tile is of the required terrain type
/// </summary>
[CreateAssetMenu(fileName = "TerrainStructureConstraint", menuName = "StructureConstraint/TerrainConstraint")]
public class TerrainStructureConstraint : StructurePlacementConstraint
{
    public TileType requiredType;
    public bool excludeThisType;
    public override bool Evaluate(Tile targetPlacement, Building targetStructure)
    {
        if(!excludeThisType)
        {
            return targetPlacement.type == requiredType;
        }
        else
        {
            return targetPlacement.type != requiredType;
        }
        
    }
}
