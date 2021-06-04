using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalInventory : MonoBehaviour
{
    public bool isSingleton;
    public int startingDrachmae;
    public static GlobalInventory singleton;
    public List<Inventory> inventories;
    public UnityEvent OnGoodsUpdated;
    public UnityEvent<int> OnDrachmaeUpdated;
    public UnityEvent<string> OnDrachmaeUpdated_Label;
    

    Dictionary<Good, int> quantities;
    int drachmae = 0;
    public int Drachmae
    {
        get
        {
            return drachmae;
        }
        set
        {
            drachmae = value;
            OnDrachmaeUpdated.Invoke(drachmae);
            OnDrachmaeUpdated_Label.Invoke(drachmae.ToString() + " \u03B4");
        }
    }
    
    private void Awake()
    {
        quantities = new Dictionary<Good, int>();
        if(isSingleton)
        {
            singleton = this;
        }
        drachmae = startingDrachmae;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        OnDrachmaeUpdated_Label.Invoke(drachmae.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetQuantity(Good good)
    {
        if(quantities.TryGetValue(good, out int value))
        {
            return value;
        }
        else
        {
            return 0;
        }
    }

    public void Add(Inventory inventory)
    {
        if (inventories.Contains(inventory)) return;

        inventories.Add(inventory);
        inventory.OnDeposit.AddListener(AddGood);
        inventory.OnWithdraw.AddListener(RemoveGood);

        
    }

    public void Remove(Inventory inventory)
    {
        if( inventories.Remove(inventory))
        {
            
        }
    }

    public void AddGood(Good good)
    {
        if(quantities.TryGetValue(good, out int value))
        {
            quantities[good] = value + 1;
        }
        else
        {
            //Debug.Log(good + " added to global inventory for first time");
            quantities.Add(good, 1);
        }
        //Debug.Log(good + " has been added, quantity now " + quantities[good]);
        OnGoodsUpdated.Invoke();
    }

    public void RemoveGood(Good good)
    {
        if(quantities.TryGetValue(good, out int value))
        {
            quantities[good] = Mathf.Max(0, value - 1);
            //Debug.Log(good + " has been removed, quantity now " + quantities[good]);
        }
        
        OnGoodsUpdated.Invoke();
    }

    public bool PullGoods(List<GoodQuantity> goods)
    {
        if (goods == null) return true;
        if (goods.Count == 0) return true;


        //check if the needs can be met
        foreach(GoodQuantity good in goods)
        {
            if (GetQuantity(good.good) < good.quantity) return false;
        }

        //pull the goods from their warehouses if the quantities are available
        Dictionary<Good, int> pulled = new Dictionary<Good, int>();

        foreach(GoodQuantity gq in goods)
        {
            Good good = gq.good;
            pulled.Add(good, 0);

            foreach(Inventory inventory in inventories)
            {
                //try to pull the amount needed until no more is left or the amount needed has been pulled, then break and move on to next inventory
                for (int i = 0; i < gq.quantity; i++)
                {
                    if (inventory.GetGood(good))
                    {
                        pulled[good] = pulled[good] + 1;
                        if (pulled[good] == gq.quantity)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                
                //if enough 
                if(pulled[good] == gq.quantity)
                {
                    break;
                }
            }
        }


        return true;
    }

    public bool PullGoods(GoodQuantity goods)
    {
        if (goods == null) return true;
        if (GetQuantity(goods.good) < goods.quantity) return false;


        Dictionary<Good, int> pulled = new Dictionary<Good, int>();
        Good good = goods.good;
        pulled.Add(good, 0);

        foreach (Inventory inventory in inventories)
        {
            //try to pull the amount needed until no more is left or the amount needed has been pulled, then break and move on to next inventory
            for (int i = 0; i < goods.quantity; i++)
            {
                if (inventory.GetGood(good))
                {
                    pulled[good] = pulled[good] + 1;
                    if (pulled[good] == goods.quantity)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            //if enough 
            if (pulled[good] == goods.quantity)
            {
                break;
            }
        }

        return true;
    }

    public bool AddGoods(GoodQuantity goods)
    {
        int capacity = 0;
        int deposited = 0;

        foreach(Inventory inv in inventories)
        {
            if(inv.allowedGoods.Contains(goods.good) || inv.requestedGoods.Contains(goods.good))
            {
                if(inv.remainingCapacity > 0)
                {
                    capacity += inv.remainingCapacity;
                }
                if(capacity >= goods.quantity)
                {
                    break;
                }
            }
        }

        if(capacity >= goods.quantity)
        {
            foreach(Inventory inv in inventories)
            {
                if(inv.allowedGoods.Contains(goods.good) || inv.requestedGoods.Contains(goods.good))
                {
                    for (int i = 0; i < goods.quantity; i++)
                    {
                        if (inv.Deposit(goods.good))
                        {
                            deposited++;
                            if (deposited == goods.quantity)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                
                
            }
        }

        return false;
    }
}
