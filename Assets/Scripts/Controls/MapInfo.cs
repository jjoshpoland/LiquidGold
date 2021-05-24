using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class MapInfo : MonoBehaviour
{
    public static MapInfo singleton;
    Vector2 mousePos;
    public LayerMask mouseInteractions;
    public Tile currentTile;
    Building currentBuilding;
    public TMP_Text mapinfotext;
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
        mousePos = Mouse.current.position.ReadValue();
        currentTile = GetTileUnderMouse();
        UpdateUI();
    }

    public Tile GetTileUnderMouse()
    {

        Ray cursorRay = Camera.main.ScreenPointToRay(mousePos);
        
        if (Physics.Raycast(cursorRay, out RaycastHit hitinfo, float.MaxValue, mouseInteractions))
        {
            if (hitinfo.collider.TryGetComponent<Tile>(out Tile tile))
            {
                return tile;
            }
            else
            {
                //Debug.Log("raycast hitting " + hitinfo.collider.name);
            }
        }
        return null;
    }

    void UpdateUI()
    {
        if(currentTile != null)
        {
            mapinfotext.text = "X: " + currentTile.coords.x + ", Y: " + currentTile.coords.y + " " + currentTile.name;
        }
    }

}
