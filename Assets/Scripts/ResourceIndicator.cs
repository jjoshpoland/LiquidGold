using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class ResourceIndicator : MonoBehaviour
{
    public GameObject Indicator;
    Inventory inventory;
    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateIndicator()
    {
        if(inventory.goods.Count > 0)
        {
            Indicator.SetActive(true);
        }
        else
        {
            Indicator.SetActive(false);
        }
    }
}
