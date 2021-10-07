using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] List<GameObject> arrows;
    [SerializeField] float radius;
    [SerializeField] LayerMask layerMask;    
    int arrowsNeeded = 0;
    int arrowsOnLastFrame = 0;

    private void Update()
    {
        UpdateRadar();
    }

    private void UpdateRadar()
    {
        var contacts = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
        arrowsNeeded = contacts.Length;

        while (arrows.Count < arrowsNeeded)
        {
            GameObject newArrow = Instantiate(arrowPrefab, transform);
            arrows.Add(newArrow);
        }

        for (int i = 0; i < arrowsNeeded; i++)
        {
            arrows[i].SetActive(true);
            arrows[i].transform.up = (contacts[i].transform.position - transform.position).normalized;
        }

        if (arrows.Count > arrowsNeeded && arrowsOnLastFrame > 0)
        {
            for (int i = arrows.Count - 1; i >= contacts.Length; i--)
            {
                arrows[i].SetActive(false);
            }
        }

        arrowsOnLastFrame = arrowsNeeded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
