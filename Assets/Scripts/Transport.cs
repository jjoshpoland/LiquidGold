using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Transport : MonoBehaviour
{
    public GameObject home;
    public Inventory destination;
    public List<Good> goods;
    public Good targetGood;
    public float interactionRange;
    NavMeshAgent agent;

    public UnityEvent OnDropOff;
    public UnityEvent<Good> OnPickUp;
    Animator animator;
    public GameObject displayModel;
    public Transform carrySpot;
    public bool IsEmpty
    {
        get
        {
            return goods.Count == 0;
        }
    }

    public Good CurrentGoods
    {
        get
        {
            if(!IsEmpty)
            {
                return goods[0];
            }
            else
            {
                return null;
            }
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        home = transform.parent.gameObject;
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        OnPickUp.AddListener(ShowCarriedGood);
        OnDropOff.AddListener(EmptyCarriedGood);
    }

    void ShowCarriedGood(Good good)
    {
        displayModel = Instantiate(good.model, carrySpot);
    }

    void EmptyCarriedGood()
    {
        if(displayModel != null)
        {
            Destroy(displayModel);
            displayModel = null;

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SeasonManager.singleton.IsMarketSeason)
        {
            agent.isStopped = true;
            return;
        }
        agent.isStopped = false;
        if(destination == null)
        {
            agent.isStopped = true;
            SetDestination();
            if (destination != null)
            {
                agent.SetDestination(destination.transform.position);
                agent.isStopped = false;
            }
            else
            {
                agent.SetDestination(home.transform.position);
                agent.isStopped = false;
            }
        }
        else
        {
            Interact();
        }

        if(animator != null)
        {
            animator.SetBool("Moving", agent.desiredVelocity.magnitude > .1f);
        }
        
    }

    public void SetDestination()
    {
        if(IsEmpty)
        {
            if(targetGood == null)
            {

                //try to find goods that the home is emptying
                CheckHomeForPickups();

                if(destination != null)
                {
                    return;
                }

                //try to find goods that the home is requesting
                CheckEverywhereForHomeGoods();
                
                
            }
            else
            {
                destination = FindAvailableProvider();
                
            }
            
            if(destination == null)
            {
                //could not find any available goods of the needed types
            }
        }
        else
        {
            //if home doesn't need my shit, then look for anyone who does
            if(!CheckIfHomeNeedsMyGoods())
            {
                destination = FindAvailableReciever();
            }
            
            if(destination == null)
            {
                //could not find a valid reciever for goods
            }
        }
    }

    void Interact()
    {
        agent.isStopped = false;
        if(DistanceCheck())
        {
            if(IsEmpty && targetGood != null)
            {
                //pick up
                if(destination.GetGood(targetGood))
                {
                    goods.Add(targetGood);
                    OnPickUp.Invoke(targetGood);
                    destination = null;
                    targetGood = null;
                }
                else
                {
                    destination = FindAvailableProvider();
                }
            }
            else if(!IsEmpty)
            {
                //drop off
                if (!destination.Deposit(CurrentGoods))
                {
                    destination = FindAvailableReciever();
                }
                else
                {
                    goods.Remove(CurrentGoods);
                    OnDropOff.Invoke();
                    destination = null;
                }
                

                
            }
        }
        else
        {
            agent.SetDestination(destination.transform.position);
            agent.isStopped = false;
        }
    }

    bool DistanceCheck()
    {
        if (destination == null) return false;
        
        //check if its close enough
        if(Vector3.Distance(transform.position, destination.transform.position) < interactionRange)
        {
            return true;
        }

        //if its not close enough, it might be on the outer edge of the collider of the destination and needs to search for it
        Ray ray = new Ray(transform.position + new Vector3(0, .5f, 0), destination.transform.position + new Vector3(0, .5f, 0));

        if(Physics.Raycast(ray, out RaycastHit hitInfo, interactionRange / 3f))
        {
            if(hitInfo.collider.TryGetComponent<Inventory>(out Inventory foundInventory))
            {
                if(foundInventory == destination)
                {
                    return true;
                }
            }
        }

        return false;
    }

    Inventory FindAvailableReciever()
    {
        if (IsEmpty)
        {
            Debug.LogWarning(name + " trying to find a reciever, but it doesn't have any goods that need to be dropped off");
            return null;
        }

        float closestDist = float.MaxValue;
        Inventory closestInv = null;

        foreach(Inventory inv in GlobalInventory.singleton.inventories)
        {
            //if this inventory doesnt allow the goods contained, skip
            if (!inv.allowedGoods.Contains(CurrentGoods)) continue;
            //if this inventory is full, skip
            if (inv.remainingCapacity <= 0) continue;
            //if this inventory is not accessible
            if (inv.TryGetComponent<Building>(out Building building))
            {
                if (!building.HasRoadAccess())
                {
                    Debug.Log(inv.name + " doesnt have road access");
                    continue;
                }
            }
            else
            {
                //if this isn't a building, don't do anything with it
                continue;
            }

            //find the distance and if its closer than the nearest stored inv, make this the nearest inv
            float distance = Vector3.Distance(transform.position, inv.transform.position);
            if(distance < closestDist)
            {
                closestDist = distance;
                closestInv = inv;
            }
        }

        
        return closestInv;
    }

    Inventory FindAvailableProvider()
    {
        if (!IsEmpty)
        {
            Debug.LogWarning(name + " trying to find a provider, but it already has goods that need to be dropped off");
            return null;
        }

        float closestDist = float.MaxValue;
        Inventory closestInv = null;

        foreach (Inventory inv in GlobalInventory.singleton.inventories)
        {
            if (!inv.allowTransportWithdrawal) continue;
            //if this inventory doesnt have the goods targeted, skip
            if (!inv.goods.Contains(targetGood)) continue;
            //if (inv.gameObject == home) continue;

            //find the distance and if its closer than the nearest stored inv, make this the nearest inv
            float distance = Vector3.Distance(transform.position, inv.transform.position);
            if (distance < closestDist)
            {
                closestDist = distance;
                closestInv = inv;
            }
        }

        return closestInv;
    }

    void CheckHomeForPickups()
    {
        Inventory[] homeInventories = home.GetComponents<Inventory>();
        for (int j = 0; j < homeInventories.Length; j++)
        {
            if (!homeInventories[j].allowTransportWithdrawal) continue;

            for (int i = 0; i < homeInventories[j].emptyingGoods.Count; i++)
            {
                //does this inventory contain any goods on its own emptying list?
                if (homeInventories[j].goods.Contains(homeInventories[j].emptyingGoods[i]))
                {
                    targetGood = homeInventories[j].emptyingGoods[i];
                    destination = homeInventories[j];
                }

                //destination = FindAvailableProvider();
                if (destination != null)
                {
                    break;
                }
            }
        }
    }

    void CheckEverywhereForHomeGoods()
    {
        Inventory[] homeInventories = home.GetComponents<Inventory>();
        for (int j = 0; j < homeInventories.Length; j++)
        {
            for (int i = 0; i < homeInventories[j].requestedGoods.Count; i++)
            {
                if(homeInventories[j].remainingCapacity == 0)
                {
                    continue;
                }
                targetGood = homeInventories[j].requestedGoods[i];
                destination = FindAvailableProvider();
                if (destination != null)
                {
                    break;
                }
            }
            if(destination != null)
            {
                break;
            }
        }
    }

    bool CheckIfHomeNeedsMyGoods()
    {
        Inventory[] homeInventories = home.GetComponents<Inventory>();
        for (int j = 0; j < homeInventories.Length; j++)
        {
            //is this inventory requesting my current goods?
            if (homeInventories[j].requestedGoods.Contains(CurrentGoods))
            {
                destination = homeInventories[j];
            }

            if (destination != null)
            {
                Debug.Log(name + " needs to bring " + CurrentGoods + " home");
                return true;
            }
            
        }

        return false;
    }
}
