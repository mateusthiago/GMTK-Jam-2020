using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar_Arrow : MonoBehaviour
{
    public bool hasTarget { get; private set; }
    public Transform target;

    private void Update()
    {
        if (target)
        {
            Vector2 direction = target.position - transform.position;
            transform.up = direction.normalized;
        }
        else
        {
            RemoveArrow();
        }
    }

    public void AddTarget(Transform newTarget)
    {
        hasTarget = true;
        target = newTarget;
        gameObject.SetActive(true);
    }

    public void RemoveArrow()
    {
        hasTarget = false;
        target = null;
        gameObject.SetActive(false);
    }
}
