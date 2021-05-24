﻿using System.Collections;
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
    NavMeshAgent agent;

    public UnityEvent OnDropOff;
    public UnityEvent OnPickUp;
    

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
        home = transform.parent.gameObject;
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

        
    }

    public void SetDestination()
    {
        if(IsEmpty)
        {
            if(targetGood == null)
            {
                Inventory[] homeInventories = home.GetComponents<Inventory>();
                //try to find goods that the home is emptying
                for (int j = 0; j < homeInventories.Length; j++)
                {
                    for (int i = 0; i < homeInventories[j].emptyingGoods.Count; i++)
                    {
                        //does this inventory contain any goods on its own emptying list?
                        if(homeInventories[j].goods.Contains(homeInventories[j].emptyingGoods[i]))
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

                if(destination != null)
                {
                    return;
                }

                //try to find goods that the home is requesting

                for (int j = 0; j < homeInventories.Length; j++)
                {
                    for (int i = 0; i < homeInventories[j].requestedGoods.Count; i++)
                    {
                        targetGood = homeInventories[j].requestedGoods[i];
                        destination = FindAvailableProvider();
                        if (destination != null)
                        {
                            break;
                        }
                    }
                }
                
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
            destination = FindAvailableReciever();
            if(destination == null)
            {
                //could not find a valid reciever for goods
            }
        }
    }

    void Interact()
    {
        if(DistanceCheck())
        {
            if(IsEmpty && targetGood != null)
            {
                //pick up
                if(destination.GetGood(targetGood))
                {
                    goods.Add(targetGood);
                    OnPickUp.Invoke();
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
        //else
        //{
        //    Debug.Log(name + "is out of range of its target inventory");
        //}
    }

    bool DistanceCheck()
    {
        if (destination == null) return false;
        
        //check if its close enough
        if(Vector3.Distance(transform.position, destination.transform.position) < 6f)
        {
            return true;
        }

        //if its not close enough, it might be on the outer edge of the collider of the destination and needs to search for it
        Ray ray = new Ray(transform.position + new Vector3(0, .5f, 0), destination.transform.position + new Vector3(0, .5f, 0));

        if(Physics.Raycast(ray, out RaycastHit hitInfo, 2f))
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
            //if this inventory doesnt have the goods targeted, skip
            if (!inv.goods.Contains(targetGood)) continue;
            

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
}
