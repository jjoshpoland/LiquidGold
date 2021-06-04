using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Inventory))]
public class Harvester : Building
{
    public Good Target;
    public float harvestTime;
    public int range;
    bool active;
    public UnityEvent<Good> OnHarvest;
    public UnityEvent OnResourcesDepleted;
    float lastHarvest;
    Inventory inventory;
    List<Resource> KnownResourcesInRange;
    public int AIMaxHarvests = 25;
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        active = true;
        inventory = GetComponent<Inventory>();
        FindResourcesInRange();
        lastHarvest = Time.time;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SeasonManager.singleton.IsMarketSeason)
        {
            return;
        }
        if(active && Time.time > lastHarvest + harvestTime)
        {
            if(HarvestNearestResource())
            {
                lastHarvest = Time.time;
                OnHarvest.Invoke(Target);
            }
            else
            {
                active = false;
                OnResourcesDepleted.Invoke();
            }
        }
    }

    public void FindResourcesInRange()
    {
        List<Resource> resources = new List<Resource>();
        int xmin = coords.x - (range / 2);
        int xmax = coords.x + (range / 2);
        int ymin = coords.y - (range / 2);
        int ymax = coords.y + (range / 2);

        for (int x = xmin; x < xmax; x++)
        {
            for (int y = ymin; y < ymax; y++)
            {
                Tile currentTile = TileMap.singleton.GetTileAtCoords(new Vector2Int(x, y));
                if(currentTile != null && currentTile.TryGetComponent<Resource>(out Resource resource))
                {
                    if(resource.type == Target)
                    {
                        resources.Add(resource);
                    }
                    
                }
            }
        }

        KnownResourcesInRange = resources;
    }

    float DistanceToResource(Resource resource)
    {
        if(resource != null)
        {
            return Vector3.Distance(transform.position, resource.transform.position);
        }
        else
        {
            return float.MaxValue;
        }
    }

    bool HarvestNearestResource()
    {
        if(inventory.remainingCapacity <= 0)
        {
            return false;
        }
        if(KnownResourcesInRange == null)
        {
            return false;
        }
        float closestDist = float.MaxValue;
        Resource closestResource = null;
        foreach(Resource resource in KnownResourcesInRange)
        {
            float distance = DistanceToResource(resource);
            if (distance < closestDist)
            {
                closestDist = distance;
                closestResource = resource;
            }
        }

        if(closestResource != null)
        {
            if(closestResource.goods.Remove(Target) && inventory.Deposit(Target))
            {
                closestResource.CheckRemainingResources();
                return true;
            }
        }

        return false;
    }
}
