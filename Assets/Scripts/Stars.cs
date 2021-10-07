using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform starUL;
    [SerializeField] Transform starUR;
    [SerializeField] Transform starLL;
    [SerializeField] Transform starLR;
    [SerializeField] Vector2 particleSystemSize; // igual ao size do shape do particlesystem star
    [SerializeField] Vector2 rectSize; 
    [SerializeField] Rect viewRect;
    [SerializeField] Vector2 relativePosition;

    Rect starRect;
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        starRect.size = rectSize;
        UpdateRect();
    }

    void Update()
    {
        CheckPlayerPosition();
    }

    private void CheckPlayerPosition()
    {
        if (!player) return;

        if (player.position.x <= starRect.xMin)
        {
            //print("LEFT TO RIGHT");
            Swap(ref starUL, ref starLL, ref starUR, ref starLR);
        }
        else if (player.position.x > starRect.xMax)
        {
            //print("RIGHT TO LEF");
            Swap(ref starUR, ref starLR, ref starUL, ref starLL);
        }
        else if (player.position.y <= starRect.yMin)
        {
            //print("LOWER TO UPPER");
            Swap(ref starLL, ref starLR, ref starUL, ref starUR);
        }
        else if (player.position.y > starRect.yMax)
        {
            //print("UPPER TO LOWER");
            Swap(ref starUL, ref starUR, ref starLL, ref starLR);
        }
    }

    private void Swap(ref Transform from1, ref Transform from2, ref Transform to1, ref Transform to2)
    {        
        Transform temp1 = from1;
        Transform temp2 = from2;
        from1 = to1;
        from2 = to2;
        to1 = temp1;
        to2 = temp2;

        from1.Translate((to1.position - from1.position) * 2);
        from2.Translate((to2.position - from2.position) * 2);

        UpdateRect();
    }

    private void UpdateRect()
    {
        starRect.center = (Vector2)starUL.position + new Vector2(particleSystemSize.x, -particleSystemSize.y) / 2;        
        viewRect = starRect;
    }

    void UpdatePositions(Vector2 displacement)
    {

    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(starRect.center, rectSize);
    }
}
