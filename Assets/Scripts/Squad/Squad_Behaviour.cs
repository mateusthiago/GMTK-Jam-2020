using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Squad_Behaviour : ScriptableObject
{
    public abstract Vector2 CalculateMove(Squad_Agent agent, List<Transform> contacts, Squad squad);
}
