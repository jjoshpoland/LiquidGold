using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UtilityAction : ScriptableObject
{
    public List<ConsiderationWeight> considerations;
    public abstract void Execute(AI ai);

}
