using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternatingRotation : MonoBehaviour
{
    public float speed;
    public Transform topRotation;
    public Transform bottomRotation;
    public bool rotating;
    bool direction = false;

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(direction)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, topRotation.rotation, speed * Time.deltaTime);
            if(Quaternion.Angle(transform.rotation, topRotation.rotation) < 1f)
            {
                direction = false;
            }
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, bottomRotation.rotation, speed * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, bottomRotation.rotation) < 1f)
            {
                direction = true;
            }
        }
    }
}
