using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Inventory : MonoBehaviour
{
    public bool AIOnly;
    public bool allowTransportWithdrawal;
    public List<Good> goods;

    //these are filtering lists
    public List<Good> allowedGoods;
    public List<Good> requestedGoods;
    public List<Good> emptyingGoods;
    //end list

    public int maxCapacity;
    public int numTransports;
    public GameObject TransportPrefab;
    List<GameObject> transports;

    public UnityEvent OnEmpty;
    public UnityEvent OnFull;
    public UnityEvent<Good> OnDeposit;
    public UnityEvent<Good> OnWithdraw;

    public int remainingCapacity
    {
        get
        {
            return maxCapacity - goods.Count;
        }
    }
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        transports = new List<GameObject>();
        if(!AIOnly)
        {
            GlobalInventory.singleton.Add(this);
        }
        
        for (int i = 0; i < numTransports; i++)
        {
            GameObject t = Instantiate(TransportPrefab, transform);
            t.transform.parent = null;
            transports.Add(t);
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
        OnDeposit = new UnityEvent<Good>();
        OnWithdraw = new UnityEvent<Good>();
        enabled = true;
    }

    private void OnDestroy()
    {
        foreach(GameObject g in transports)
        {
            Destroy(g);
        }
        GlobalInventory.singleton.Remove(this);
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
            OnDeposit.Invoke(good);
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
            OnWithdraw.Invoke(good);
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

    public int CheckGoods(Good good)
    {
        int count = 0;

        foreach(Good storedGood in goods)
        {
            if(storedGood == good)
            {
                count++;
            }
        }

        return count;
    }
}
