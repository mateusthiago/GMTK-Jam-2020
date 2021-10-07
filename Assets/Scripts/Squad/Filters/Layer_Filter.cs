using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Squad/Filter/Layer")]
public class Layer_Filter : Squad_Filter
{
    public LayerMask layerMask;
    public override List<Transform> Filter(Squad_Agent agent, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>();

        foreach (Transform contact in original)
        {            
            if ( (layerMask | (1 << contact.gameObject.layer)) == layerMask )
            {
                filtered.Add(contact);
            }
        }

        return filtered;
    }

    public override LayerMask GetLayerMask()
    {
        return layerMask;
    }
}
