using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Squad/Filter/Same Squad")]
public class SameSquad_Filter : Squad_Filter
{
    public override List<Transform> Filter(Squad_Agent agent, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>();

        foreach (Transform contact in original)
        {
            Squad_Agent contactAgent = contact.GetComponent<Squad_Agent>();
            if (contactAgent != null && contactAgent.AgentSquad == agent.AgentSquad)
            {
                filtered.Add(contact);
            }
        }

        return filtered;
    }

    public override LayerMask GetLayerMask()
    {
        return new LayerMask();
    }
}
