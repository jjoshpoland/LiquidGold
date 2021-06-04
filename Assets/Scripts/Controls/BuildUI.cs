using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class BuildUI : MonoBehaviour
{

    Tile currentTile;
    public Tile EmptyTilePrefab;
    public Building currentBuilding;
    GameObject buildingGhost;
    public GameObject destructionGhost;
    PlayerInput input;
    public bool Destroying;
    bool dragging;
    public UnityEvent OnNotEnoughResources;
    public UnityEvent OnConstraintsNotMet;
    public UnityEvent OnBuildingPlaced;
    
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
            buildingGhost.transform.position = currentTile.transform.position + new Vector3(0, .25f, 0);
        }
        if(dragging)
        {
            PlaceCurrentBuilding();
        }
    }

    /// <summary>
    /// Clears any existing building ghosts on the cursor and spawns a ghost to follow the cursor. Called by a UI button.
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

    public void StartDestroying()
    {
        if (buildingGhost != null)
        {
            Destroy(buildingGhost);
            buildingGhost = null;
        }
        Destroying = true;
        buildingGhost = Instantiate(destructionGhost);
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
                GlobalInventory.singleton.Drachmae = GlobalInventory.singleton.Drachmae - Market.singleton.PlaceOrder(currentBuilding.cost, Player.mainPlayer);
            }
            
            //Debug.Log("replacing " + currentTile + " @ " + currentTile.coords);
            if(TileMap.singleton.ReplaceTile(currentTile.coords, currentBuilding))
            {
                if(!currentBuilding.draggable)
                {
                    currentBuilding = null;
                    Destroy(buildingGhost);
                    buildingGhost = null;
                }
                else
                {
                    dragging = true;
                }
                OnBuildingPlaced.Invoke();
                //fire on placed building event
            }
        }
        else
        {
            if(!dragging)
            {
                OnConstraintsNotMet.Invoke();
            }
            
        }
    }

    void DestroyCurrentBuilding()
    {
        if(currentTile != null
            && currentTile.type == TileType.Structure)
        {
            if(TileMap.singleton.ReplaceTile(currentTile.coords, EmptyTilePrefab))
            {
                
            }
        }
    }
    #region InputEvents
    void OnSelect()
    {
        if(currentBuilding != null)
        {
            PlaceCurrentBuilding();
        }
        
        if(Destroying)
        {
            DestroyCurrentBuilding();
        }
    }

    void OnRelease()
    {
        if (!dragging) return;
        dragging = false;
        
    }

    public void OnCancel()
    {
        if (buildingGhost != null)
        {
            Destroy(buildingGhost);
        }
        if (Destroying)
        {
            Destroying = false;
        }

        if (dragging)
        {
            dragging = false;
        }

        currentBuilding = null;
        buildingGhost = null;

    }

    void OnAltSelect()
    {

        OnCancel();
        
    }
    #endregion
}
