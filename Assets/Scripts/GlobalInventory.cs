using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInventory : MonoBehaviour
{
    public static GlobalInventory singleton;
    public List<Inventory> inventories;
    private void Awake()
    {
        singleton = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
