using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Squad/Behaviour/Alignment")]
public class Alignment_Behaviour : Filtered_Behaviour
{
    //public float smoothTime = 0.5f;
    public override Vector2 CalculateMove(Squad_Agent agent, List<Transform> contacts, Squad squad)
    {
        // mantain direction if no contacts
        if (contacts.Count == 0) return agent.transform.up;

        // get filter       
        List<Transform> filteredContacts = (filter == null) ? contacts : filter.Filter(agent, contacts);
        if (filteredContacts.Count == 0) return Vector2.zero;

        // determine avg alignment
        Vector2 alignmentMove = Vector2.zero;
        foreach (Transform contact in filteredContacts)
        {
            alignmentMove += (Vector2)contact.up;
        }        
        alignmentMove /= filteredContacts.Count;

        //Vector2 currentVelocity = Vector2.zero;
        //alignmentMove = Vector2.SmoothDamp(agent.transform.up, alignmentMove, ref currentVelocity, smoothTime);
        return alignmentMove;
    }
}
