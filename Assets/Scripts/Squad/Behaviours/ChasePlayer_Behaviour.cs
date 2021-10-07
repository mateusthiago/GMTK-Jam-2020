using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Squad/Behaviour/Chase Player")]
public class ChasePlayer_Behaviour : Squad_Behaviour
{
    Vector2 player;
    public float radius = 20f;
    [Range(0.5f, 1f)]
    public float tolerance = 0.9f;
    public override Vector2 CalculateMove(Squad_Agent agent, List<Transform> contacts, Squad squad)
    {
        player = (Vector2)GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector2 playerOffset = player - (Vector2)agent.transform.position;
        float t = playerOffset.magnitude / radius;
        if (t < tolerance)
        {
            return Vector2.zero;
        }

        return playerOffset * t * t;
    }
}
