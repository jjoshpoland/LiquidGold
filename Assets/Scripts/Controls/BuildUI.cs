using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class BuildUI : MonoBehaviour
{

    Tile currentTile;
    public Building currentBuilding;
    GameObject buildingGhost;
    PlayerInput input;

    public UnityEvent OnNotEnoughResources;
    
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();   
    }

    // Update is called once per frame
    void Update()
    {
        currentTile = MapInfo.singleton.currentTile;
        if (currentTile != null && buildingGhost != null)
        {
            buildingGhost.transform.position = currentTile.transform.position;
        }
    }

    /// <summary>
    /// Clears any existing building ghosts on the cursor and spawns a ghost to follow the cursor
    /// </summary>
    /// <param name="building"></param>
    public void AttachBuildingGhostToMouse(Building building)
    {
        if(buildingGhost != null)
        {
            Destroy(buildingGhost);
            buildingGhost = null;
        }
        GameObject newGhost = Instantiate(building.ghostPrefab);
        buildingGhost = newGhost;
        currentBuilding = building;
    }

    
    /// <summary>
    /// Evaluates whether or not a building can be placed based on the tile under the mouse and the building selected by the player
    /// </summary>
    void PlaceCurrentBuilding()
    {
        if(buildingGhost != null 
            && currentBuilding != null 
            && currentTile != null 
            && currentTile.type != TileType.Structure 
            && currentBuilding.EvaluateConstraints(currentTile)
            )
        {
            if(!GlobalInventory.singleton.PullGoods(currentBuilding.cost))
            {
                OnNotEnoughResources.Invoke();
                GlobalInventory.singleton.Drachmae = GlobalInventory.singleton.Drachmae - Market.singleton.PlaceOrder(currentBuilding.cost);
            }
            
            //Debug.Log("replacing " + currentTile + " @ " + currentTile.coords);
            if(TileMap.singleton.ReplaceTile(currentTile.coords, currentBuilding))
            {
                currentBuilding = null;
                Destroy(buildingGhost);
                buildingGhost = null;
                //fire on placed building event
            }
        }
        else
        {
            Debug.Log("cannot place " + currentBuilding + " on " + currentTile);
        }
    }
    #region InputEvents
    void OnSelect()
    {
        PlaceCurrentBuilding();
    }
    #endregion
}
