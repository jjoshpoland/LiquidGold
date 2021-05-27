using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoodUI : MonoBehaviour
{
    public Good good;
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        GetQuantity();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Hooked up to global inventory unity event
    /// </summary>
    public void GetQuantity()
    {
        text.text = GlobalInventory.singleton.GetQuantity(good).ToString();
    }
}
