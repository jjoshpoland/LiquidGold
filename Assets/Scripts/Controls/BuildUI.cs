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
    public Material buildingGhostGood;
    public Material buildingGhostBad;
    bool dragging;
    public UnityEvent OnNotEnoughResources;
    public UnityEvent OnConstraintsNotMet;
    public UnityEvent OnBuildingPlaced;
    public UnityEvent OnDestroyBuilding;
    public UnityEvent OnEsc;
    
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
            if(!EvaluateBuildingPlacement())
            {
                AssignGhostMaterial(Color.red);
            }
            else
            {
                AssignGhostMaterial(Color.green);
            }
        }
        if(dragging)
        {
            PlaceCurrentBuilding();
        }
    }

    void AssignGhostMaterial(Color color)
    {
        MeshRenderer mr = buildingGhost.GetComponent<MeshRenderer>();
        for (int i = 0; i < mr.materials.Length; i++)
        {
            //if(mr.materials[i] != mat)
            mr.materials[i].color = color;
        }
        MeshRenderer[] mr2 = buildingGhost.GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer subMr in mr2)
        {
            for (int i = 0; i < subMr.materials.Length; i++)
            {
                //if (subMr.materials[i] != mat)
                    subMr.materials[i].color = color;
            }
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
        if(EvaluateBuildingPlacement())
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
            //Debug.Log("tried to place " + currentBuilding + " on " + currentTile);
        }
    }

    bool EvaluateBuildingPlacement()
    {
        return buildingGhost != null
            && currentBuilding != null
            && currentTile != null
            && currentTile.type != TileType.Structure
            && currentBuilding.EvaluateConstraints(currentTile);
    }

    void DestroyCurrentBuilding()
    {
        if(currentTile != null
            && currentTile.type == TileType.Structure)
        {
            if(TileMap.singleton.ReplaceTile(currentTile.coords, EmptyTilePrefab))
            {
                OnDestroyBuilding.Invoke();
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
        bool nothingToCancel = true;
        if (buildingGhost != null)
        {
            Destroy(buildingGhost);
            nothingToCancel = false;
        }
        if (Destroying)
        {
            Destroying = false;
            nothingToCancel = false;
        }
        if (dragging)
        {
            dragging = false;
            nothingToCancel = false;
        }

        currentBuilding = null;
        buildingGhost = null;
        if(nothingToCancel)
        {
            OnEsc.Invoke();
        }
    }

    void OnAltSelect()
    {

        OnCancel();
        
    }
    #endregion
}
