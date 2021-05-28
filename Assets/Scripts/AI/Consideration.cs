using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consideration : ScriptableObject
{
    public abstract float Evaluate(AI ai);
}

[System.Serializable]
public class ConsiderationWeight
{
    public Consideration consideration;
    public bool invert;
    public float weight;
}
