using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Squad : MonoBehaviour
{
    public enum Formation {Circular, Squad };

    public Squad_Agent agentPrefab;
    List<Squad_Agent> agents = new List<Squad_Agent>();
    public Squad_Behaviour behaviour;
    public Formation formation;
    public int startingCount = 5;
    public float agentDensity = 1f;    
    public float driveFactor = 10f;
    public float maxSpeed = 5f;
    public float neighbourRadius = 4f;
    public float avoidanceRadius = 2f;
    public LayerMask collisionMask;

    float sqrMaxSpeed;
    float sqrNeighbourRadius;
    float sqrAvoidanceRadius;
    public float SqrAvoidanceRadius {  get { return sqrAvoidanceRadius; } }
    public float SqrMaxSpeed {  get { return sqrMaxSpeed; } }

    
    void Start()
    {
        sqrMaxSpeed = maxSpeed * maxSpeed;
        sqrNeighbourRadius = neighbourRadius * neighbourRadius;
        sqrAvoidanceRadius = avoidanceRadius * avoidanceRadius;
        SpawnSquad();
    }

    public void SpawnSquad()
    {
        for (int i = 0; i < startingCount; i++)
        {
            Vector2 position = (Vector2)transform.position + Random.insideUnitCircle * startingCount * agentDensity; // formation == Formation.Circular ? CalculateCircularPosition(i) : CalculateSquadPosition(i);
            Quaternion rotation = Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f));

            Squad_Agent newAgent = Instantiate(agentPrefab, position, rotation, transform);

            newAgent.name = "Agent " + i;
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }
    }

    private Vector2 CalculateCircularPosition(int order)
    {
        float minRadius = (float)startingCount / 2 * avoidanceRadius;
        float maxRadius = startingCount / agentDensity;
        float r = minRadius;
        float angle = Mathf.Deg2Rad * (360/startingCount * (order) + 90); 
        float x = Mathf.Cos(angle) * r;
        float y = Mathf.Sin(angle) * r;
        Vector2 position = new Vector2(x, y);
        //print(string.Format("AGENT {0}\nr = {1}  angle = {2}  position = {3}", order, r, angle, position));
        return position;
    }

    private Vector2 CalculateSquadPosition(int order)
    {        
        int side = (order % 2 == 0) ? +1 : -1;
        int line = (order + 1) / 2;
        float minDist = avoidanceRadius * 2 + 0.1f;
        float x = line * minDist * side;
        float y = line * minDist * -1;
        Vector2 position = new Vector2(x, y);
        //print(string.Format("AGENT {0}\nr = {1}  angle = {2}  position = {3}", order, r, angle, position));
        return position;
    }

    public void Respawn()
    {
        foreach(Squad_Agent agent in agents)
        {
            Destroy(agent.gameObject);
        }

        agents.Clear();
        SpawnSquad();
    }
        
    void Update()
    {
        foreach(Squad_Agent agent in agents)
        {
            List<Transform> contacts = GetNearbyObjects(agent);

            Vector2 velocity = behaviour.CalculateMove(agent, contacts, this);
            velocity *= driveFactor;
            if (velocity.sqrMagnitude > sqrMaxSpeed) velocity = velocity.normalized * maxSpeed;
            agent.Move(velocity);
        }
    }

    List<Transform> GetNearbyObjects(Squad_Agent agent)
    {
        List<Transform> contacts = new List<Transform>();
        Collider2D[] hits = Physics2D.OverlapCircleAll(agent.transform.position, neighbourRadius, collisionMask);        
        foreach (Collider2D collider in hits)
        {
            if (collider != agent.AgentCollider) contacts.Add(collider.transform);
        }
        return contacts;        
    }    
}
