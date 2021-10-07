using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

[ExecuteInEditMode]
public class BoxCastTest : MonoBehaviour
{
    public Collider2D collider;
    public Vector2 size;
    public float distance;
    public LayerMask layerMask;    
    public Vector3 direction;
    public bool useTransformUp;    

    RaycastHit2D[] hitArray = new RaycastHit2D[10];
    int hit;

    void Update()
    {
        if (useTransformUp) direction = transform.up;
        //hits = Physics2D.BoxCastAll(transform.position, size, transform.rotation.eulerAngles.z, direction, distance, layerMask);
        Array.Clear(hitArray, 0, hitArray.Length);
        hit = collider.Cast(direction, hitArray, distance);
    }

    private void OnDrawGizmos()
    {
        Vector3 dirNormalized = direction.normalized;

        Vector2 UL = new Vector2(-size.x / 2, size.y / 2);
        Vector2 UR = new Vector2(size.x / 2, size.y / 2);
        Vector2 LL = new Vector2(-size.x / 2, -size.y / 2);
        Vector2 LR = new Vector2(size.x / 2, -size.y / 2);
        Vector2[] cornerPoints = { UL, UR, LL, LR };

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, size);
        
        Gizmos.color = Color.green;
        foreach (var corner in cornerPoints)
        {                        
            Gizmos.DrawRay(corner, transform.InverseTransformDirection(dirNormalized * distance));
        }
        Gizmos.DrawWireCube(transform.InverseTransformPoint(transform.position + dirNormalized * distance), size);

        if (hitArray != null)
        {
            foreach (var hit in hitArray)
            {
                if (hit.collider != null)
                {
                    Gizmos.color = Color.red;                    
                    Vector2 boxCenter = transform.InverseTransformPoint(transform.position + dirNormalized * hit.distance);
                    Gizmos.DrawWireCube(boxCenter, size);

                    Gizmos.matrix = Matrix4x4.identity;
                    Gizmos.DrawWireSphere(hit.point, 0.2f);
                }
            }
        }
    }

    /*
     
    * PREVE BOXCAST E CIRCLECAST - TIREI DO SCRIPT ENEMY, DEVE PRECISAR DE ALGUMAS CORREÇÕES
     
    public bool isCircle;
    private void OnDrawGizmos()
    {
        //Vector2[] cornerPoints = new Vector2[4];
        //if (isCircle)
        //{
        //    cornerPoints[0] = new Vector2(-collisionCheckSize.x, 0);
        //    cornerPoints[1] = new Vector2(collisionCheckSize.x, 0);
        //    cornerPoints[2] = new Vector2(0, -collisionCheckSize.y);
        //    cornerPoints[3] = new Vector2(0, collisionCheckSize.y);
        //}
        //else
        //{
        //    cornerPoints[0] = new Vector2(-collisionCheckSize.x / 2, collisionCheckSize.y / 2);
        //    cornerPoints[1] = new Vector2(collisionCheckSize.x / 2, collisionCheckSize.y / 2);
        //    cornerPoints[2] = new Vector2(-collisionCheckSize.x / 2, -collisionCheckSize.y / 2);
        //    cornerPoints[3] = new Vector2(collisionCheckSize.x / 2, -collisionCheckSize.y / 2);
        //}


        Gizmos.matrix = transform.localToWorldMatrix;
        if (isCircle) Gizmos.DrawWireSphere(Vector3.zero, collisionCheckSize.x);
        else Gizmos.DrawWireCube(Vector3.zero, collisionCheckSize);

        //Gizmos.color = Color.green;
        //foreach (var corner in cornerPoints)
        //{
        //    if (corner != null) Gizmos.DrawRay(corner, transform.InverseTransformDirection(colliderCastDirection * colliderCastDistance));
        //}
        //if (isCircle) Gizmos.DrawWireSphere(transform.InverseTransformPoint((Vector2)transform.position + colliderCastDirection * colliderCastDistance), collisionCheckSize.x);
        //else Gizmos.DrawWireCube(transform.InverseTransformPoint((Vector2)transform.position + colliderCastDirection * colliderCastDistance), collisionCheckSize);

        for (int i = 0; i < contactCount; i++)
        {
            if (collisionContacts[i] != null)
            {
                //Gizmos.color = Color.red;
                //Vector2 boxCenter = transform.InverseTransformPoint(transform.position + dirNormalized * collisionContacts[i].distance);
                //Gizmos.DrawWireCube(boxCenter, collisionCheckSize);

                Gizmos.matrix = Matrix4x4.identity;
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(nearestContactPoint, 0.2f);
            }
        }
    }
    */
}
