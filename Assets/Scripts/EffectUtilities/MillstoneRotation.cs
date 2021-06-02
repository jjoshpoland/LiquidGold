using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillstoneRotation : MonoBehaviour
{
    public bool rotating;
    public Vector3 rotationSpeed;
    

    public bool Rotating
    {
        get
        {
            return rotating;
        }
        set
        {
            rotating = value;
        }
    }

    private void Awake()
    {


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(rotating)
        {
            Rotate();
        }
    }

    public void Rotate()
    {
        transform.Rotate(new Vector3(rotationSpeed.x, rotationSpeed.y * Time.deltaTime, rotationSpeed.z));
    }
}
