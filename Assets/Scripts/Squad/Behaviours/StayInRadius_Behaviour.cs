using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Squad/Behaviour/Stay In Radius")]
public class StayInRadius_Behaviour : Squad_Behaviour
{
    public Vector2 center;
    public float radius = 20f;
    [Range(0.5f, 1f)]
    public float tolerance = 0.9f;
    public override Vector2 CalculateMove(Squad_Agent agent, List<Transform> contacts, Squad squad)
    {
        Vector2 centerOffset = center - (Vector2)agent.transform.position;
        float t = centerOffset.magnitude / radius;
        if ( t < tolerance)
        {
            return Vector2.zero;
        }

        return centerOffset * t * t;
    }
}
