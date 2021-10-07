using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Squad_Agent : MonoBehaviour
{    
    public Collider2D AgentCollider { get { return agentCollider; } }
    [SerializeField] Collider2D agentCollider;
    
    public Squad AgentSquad { get { return agentSquad; } }
    public Squad agentSquad;

    [SerializeField] LayerMask collisionMask;

    void Start()
    {
        if (agentCollider == null) agentCollider = GetComponent<Collider2D>();        
    }

    public void Initialize(Squad squad) // Called by Squad.cs
    {
        agentSquad = squad;
    }

    public void Move(Vector2 velocity)
    {
        if (velocity.sqrMagnitude < (agentSquad.maxSpeed * agentSquad.maxSpeed)/4) velocity = transform.up * agentSquad.maxSpeed/2f;
        transform.up = velocity; // REMOVER
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.DrawWireSphere(transform.position, agentSquad.neighbourRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, agentSquad.avoidanceRadius);
    }
}
