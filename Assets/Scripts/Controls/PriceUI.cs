using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PriceUI : MonoBehaviour
{
    public TMP_Text text;
    public Good good;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        UpdateLabel();
        Market.singleton.OnPricesUpdated.AddListener(UpdateLabel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLabel()
    {
        text.text = Market.singleton.GetPrice(good).ToString() + "\u03B4";
    }
}
