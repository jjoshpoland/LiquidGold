using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class WarehouseDisplay : MonoBehaviour
{
    public float xSpacing;
    public float ySpacing;
    public float floorHeight;
    public int width;
    public int length;
    public Transform gridOrigin;
    public GameObject testPrefab;

    Vector3[] positions;
    GameObject[] goodModels;
    Good[] goods;
    // Start is called before the first frame update
    void Start()
    {
        SetInitialPositions();
        goodModels = new GameObject[width * length];
        goods = new Good[width * length];
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetInitialPositions()
    {
        int count = width * length;
        positions = new Vector3[count];
        for (int i = 0; i < count; i++)
        {
            int x = i % width;
            int y = i / length;
            positions[i] = new Vector3(x * xSpacing, floorHeight, y * ySpacing);
            
        }
    }

    public void AddGood(Good good)
    {
        //find empty space
        for (int i = 0; i < positions.Length; i++)
        {
            if(goodModels[i] == null)
            {
                GameObject newModel = Instantiate(good.model, gridOrigin);
                newModel.transform.localPosition += positions[i];
                goodModels[i] = newModel;
                goods[i] = good;
                break;
            }
        }
    }

    public void RemoveGood(Good good)
    {
        //find a spot with this good
        for (int i = 0; i < positions.Length; i++)
        {
            if(goods[i] == good)
            {
                Destroy(goodModels[i].gameObject);
                goodModels[i] = null;
                goods[i] = null;
                break;
            }
        }
    }
}
