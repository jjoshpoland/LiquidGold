using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    public float speed;
    public float minCamHeight;
    public float maxCamHeight;
    Vector2 move;
    float scroll;
    // Start is called before the first frame update
    void Start()
    {
        
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
