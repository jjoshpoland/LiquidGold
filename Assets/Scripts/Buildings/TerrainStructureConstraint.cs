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
    public override bool Evaluate(Tile targetPlacement, Building targetStructure)
    {
        return targetPlacement.type == requiredType;
    }
}
