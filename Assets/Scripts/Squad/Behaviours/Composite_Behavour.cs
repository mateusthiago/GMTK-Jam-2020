using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

[CreateAssetMenu(menuName = "Squad/Behaviour/Composite")]
public class Composite_Behavour : Squad_Behaviour
{
    public Squad_Behaviour[] behaviours;
    public float[] weights;
    public override Vector2 CalculateMove(Squad_Agent agent, List<Transform> contacts, Squad squad)
    {
        // handle data mismatch
        if (weights.Length != behaviours.Length)
        {
            Debug.Break();
            Debug.Log("composite behaviour array mismatch error");
            return Vector2.zero;
        }

        // set move
        Vector2 move = Vector2.zero;
        for (int i = 0; i < behaviours.Length; i++)
        {
            Vector2 partialMove = behaviours[i].CalculateMove(agent, contacts, squad) * weights[i];

            if (partialMove != Vector2.zero)
            {
                if (partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    partialMove = partialMove.normalized * weights[i];
                }

                move += partialMove;
            }
        }

        return move;
    }
}
