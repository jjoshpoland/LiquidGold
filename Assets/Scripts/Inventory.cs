using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Inventory : MonoBehaviour
{
    public List<Good> goods;

    //these are filtering lists
    public List<Good> allowedGoods;
    public List<Good> requestedGoods;
    public List<Good> emptyingGoods;
    //end list

    public int maxCapacity;
    public int numTransports;
    public GameObject TransportPrefab;

    public UnityEvent OnEmpty;
    public UnityEvent OnFull;
    public UnityEvent OnDeposit;
    public UnityEvent OnWithdraw;

    public int remainingCapacity
    {
        get
        {
            return maxCapacity - goods.Count;
        }
    }
    private void Awake()
    {
        GlobalInventory.singleton.inventories.Add(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numTransports; i++)
        {
            GameObject t = Instantiate(TransportPrefab, transform);
            t.transform.parent = null;
        }
        
        
    }

    public void Init()
    {
        goods = new List<Good>();
        allowedGoods = new List<Good>();
        requestedGoods = new List<Good>();
        emptyingGoods = new List<Good>();
        OnEmpty = new UnityEvent();
        OnFull = new UnityEvent();
        OnDeposit = new UnityEvent();
        OnWithdraw = new UnityEvent();
    }

    private void OnDestroy()
    {
        GlobalInventory.singleton.inventories.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Deposit(Good good)
    {
        if(remainingCapacity > 0)
        {
            goods.Add(good);
            OnDeposit.Invoke();
            if(remainingCapacity == 0)
            {
                OnFull.Invoke();
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetGood(Good good)
    {
        if(goods.Contains(good))
        {
            goods.Remove(good);
            OnWithdraw.Invoke();
            if(goods.Count == 0)
            {
                OnEmpty.Invoke();
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Checks if this inventory can provide all needed ingredients for a given recipe
    /// </summary>
    /// <param name="recipe"></param>
    /// <returns></returns>
    public bool CheckRecipe(Recipe recipe)
    {
        //keep track of what has been pulled out of the inventory
        List<Good> pulledInputs = new List<Good>();
        foreach (Good input in recipe.inputs)
        {
            //check if the inventory cannot pull the good. if it can, just add it to the pulled inputs. if it cannot, add all pulled inputs back into the inventory and return false.
            if (!GetGood(input))
            {
                foreach (Good pulledInput in pulledInputs)
                {
                    goods.Add(pulledInput);
                }
                return false;
            }
            else
            {
                pulledInputs.Add(input);
            }
        }

        return true;
    }
}
