using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic data object used to evaluate constraints based on provided function, building, and target tile.
/// </summary>
public class StructurePlacementConstraint : ScriptableObject
{
    public virtual bool Evaluate(Tile targetPlacement, Building targetStructure)
    {
        return true;
    }
}
