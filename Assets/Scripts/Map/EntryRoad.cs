using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryRoad : MonoBehaviour
{
    public List<StructurePlacementConstraint> constraints;
    public Building roadPrefab;
    // Start is called before the first frame update
    void Start()
    {
        TileMap.singleton.OnGenerateComplete.AddListener(PlaceStartingRoad);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaceStartingRoad()
    {
        for (int x = 0; x < TileMap.singleton.size; x++)
        {
            for (int y = 0; y < TileMap.singleton.size; y++)
            {
                Tile currentTile = TileMap.singleton.tiles[x, y];
                bool goodSpot = true;

                foreach (StructurePlacementConstraint constraint in constraints)
                {
                    if(!constraint.Evaluate(currentTile, roadPrefab))
                    {
                        goodSpot = false;
                    }
                }

                if(goodSpot)
                {
                    TileMap.singleton.ReplaceTile(currentTile.coords, roadPrefab);
                    return;
                }
            }
        }

        
    }
}
