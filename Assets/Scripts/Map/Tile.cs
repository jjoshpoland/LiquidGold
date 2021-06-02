using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileType type;
    public Vector2Int coords;
    public bool RandomRotationOnStart;
    // Start is called before the first frame update
    void Start()
    {
        if(RandomRotationOnStart)
        {
            int randomRoll = Random.Range(0, 4);
            transform.Rotate(new Vector3(0, 90f * randomRoll, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
