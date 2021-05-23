using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public Building building;
    public UnityEvent<Building> OnSelect;
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if(TryGetComponent<Button>(out Button button))
        {
            button.onClick.AddListener(Select);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        OnSelect.Invoke(building);
    }
}
