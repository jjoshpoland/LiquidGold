using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressiveTranslation : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public Transform target;
    public bool moving;
    public float speed;

    public bool Moving
    {
        get
        {
            return moving;
        }
        set
        {
            moving = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(moving)
        {
            Move();
        }
    }

    public void Move()
    {
        target.position = target.position + (end.position - target.position).normalized * speed * Time.deltaTime;
        if (Vector3.Distance(end.position, target.position) < .25f)
        {
            target.transform.position = start.position;
            
        }
        
    }
}
