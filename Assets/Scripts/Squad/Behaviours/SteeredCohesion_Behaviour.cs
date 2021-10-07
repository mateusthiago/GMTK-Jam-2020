using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Squad/Behaviour/Steered Cohesion")]
public class SteeredCohesion_Behaviour : Filtered_Behaviour
{
    public float movementSmoothTime = 0.5f;
    public override Vector2 CalculateMove(Squad_Agent agent, List<Transform> contacts, Squad squad)
    {        
        // return 0 if no contacts
        if (contacts.Count == 0) return Vector2.zero;

        // get filter
        List<Transform> filteredContacts = (filter == null) ? contacts : filter.Filter(agent, contacts);
        if (filteredContacts.Count == 0) return Vector2.zero;

        // determine average position        
        Vector2 cohesionMove = Vector2.zero;
        foreach (Transform contact in filteredContacts)
        {
            cohesionMove += (Vector2)contact.position;
        }
        cohesionMove /= filteredContacts.Count;

        // create offset from agent position
        Vector2 currentVelocity = Vector2.zero;
        cohesionMove -= (Vector2)agent.transform.position;
        cohesionMove = Vector2.SmoothDamp(agent.transform.up, cohesionMove, ref currentVelocity, movementSmoothTime, Mathf.Infinity, Time.deltaTime);
        return cohesionMove;
    }
}
