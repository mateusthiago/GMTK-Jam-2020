using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Squad/Behaviour/Squad Avoidance")]
public class SquadAvoidance_Behaviour : Filtered_Behaviour
{
    //public float movementSmoothTime = 0.5f;
    public override Vector2 CalculateMove(Squad_Agent agent, List<Transform> contacts, Squad squad)
    {
        // return 0 if no contacts
        if (contacts.Count == 0) return Vector2.zero;

        // get filter       
        List<Transform> filteredContacts = (filter == null) ? contacts : filter.Filter(agent, contacts);
        if (filteredContacts.Count == 0) return Vector2.zero;

        // determine avoidance position
        Vector2 currentVelocity = Vector2.zero;
        Vector2 avoidanceMove = Vector2.zero;
        int avoidContacts = 0;
        foreach (Transform contact in filteredContacts)
        {
            float sqrDistance = Vector2.SqrMagnitude(contact.position - agent.transform.position);
            if (sqrDistance <= squad.SqrAvoidanceRadius)
            {
                avoidContacts++;
                avoidanceMove += (Vector2)(agent.transform.position - contact.position);
            }
        }
        if (avoidContacts > 0)
        {
            avoidanceMove /= avoidContacts;
        }

        // create offset from agent position
        //avoidanceMove -= (Vector2)agent.transform.position;
        //avoidanceMove = Vector2.SmoothDamp(agent.transform.up, avoidanceMove, ref currentVelocity, movementSmoothTime, Mathf.Infinity, Time.deltaTime);
        return avoidanceMove;
    }
}
