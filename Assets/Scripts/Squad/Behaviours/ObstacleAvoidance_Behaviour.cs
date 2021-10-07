using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Squad/Behaviour/Obstacle Avoidance")]
public class ObstacleAvoidance_Behaviour : Filtered_Behaviour
{    
    public override Vector2 CalculateMove(Squad_Agent agent, List<Transform> contacts, Squad squad)
    {
        if (contacts.Count == 0) return Vector2.zero;

        List<Transform> filteredContacts = (filter == null) ? contacts : filter.Filter(agent, contacts);
        if (filteredContacts.Count == 0) return Vector2.zero;

        Vector2 avoidanceMove = Vector2.zero;
        int contactsToAvoid = 0;
        foreach (Transform contact in filteredContacts)
        {
            Vector2 directionToContact = contact.position - agent.transform.position;
            //float sqrDistanceToContact = Vector2.SqrMagnitude(directionToContact);
            //if (sqrDistanceToContact <= agent.AgentSquad.SqrSquadAvoidanceRadius)
            //{
            //    avoidanceMove -= directionToContact;
            //    contactsToAvoid++;
            //}

            var hit = Physics2D.Raycast(agent.transform.position, directionToContact, agent.AgentSquad.avoidanceRadius, filter.GetLayerMask());
            if (hit.collider != null)
            {
                contactsToAvoid++;
                avoidanceMove -= directionToContact;
            }
        }

        if (contactsToAvoid != 0)
        {
            avoidanceMove /= contactsToAvoid;            
            avoidanceMove = Vector2.Lerp(agent.transform.up, avoidanceMove.normalized, 0.5f);
            return avoidanceMove;
        }
        else
        {
            return Vector2.zero;
        }        
    }

}
