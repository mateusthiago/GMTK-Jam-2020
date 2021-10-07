using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Squad_Filter : ScriptableObject
{
    public abstract List<Transform> Filter(Squad_Agent agent, List<Transform> original);

    public abstract LayerMask GetLayerMask();
}
