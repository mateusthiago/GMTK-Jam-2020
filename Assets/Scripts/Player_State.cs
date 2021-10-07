using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_State : MonoBehaviour
{
    public bool usePhysics;
    [Space]
    public float maxHeat;
    public float currentHeat;
    [Space]
    public bool inControl;
    public float xAxis;
    public float yAxis;
    public bool shieldBoost;
    public float currentSpeed;
}
