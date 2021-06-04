using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    public float speed;
    public float minCamHeight;
    public float maxCamHeight;
    Vector2 startingPos;
    public int mapsize;
    Vector2 move;
    float scroll;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = new Vector2((mapsize / 2f) * 10f, (mapsize / 2f) * 10f);
        transform.position = new Vector3(startingPos.x, transform.position.y, startingPos.y);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(move.x, -scroll / 4f, move.y) * speed * Time.deltaTime;
        if(transform.position.y < minCamHeight)
        {
            transform.position = new Vector3(transform.position.x, minCamHeight, transform.position.z);
        }
        else if(transform.position. y > maxCamHeight)
        {
            transform.position = new Vector3(transform.position.x, maxCamHeight, transform.position.z);
        }
        
        if(transform.position.z < 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        if(transform.position.x < 0)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
        if(transform.position.z > mapsize * 10)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, mapsize * 10f);
        }
        if(transform.position.x > mapsize * 10)
        {
            transform.position = new Vector3(mapsize * 10f, transform.position.y, transform.position.z);
        }
       
        
    }

    void OnMovement(InputValue value)
    {
        move = value.Get<Vector2>();

        
    }

    void OnZoom(InputValue value)
    {
        scroll = value.Get<float>();
    }
}
