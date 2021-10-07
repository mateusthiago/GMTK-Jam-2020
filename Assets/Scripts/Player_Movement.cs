using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Timeline;

public class Player_Movement : MonoBehaviour
{
    Player_State playerState;
    Rigidbody2D rigidBody;

    [SerializeField] float rotationSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;    
    [SerializeField] public Vector2 velocity;
    [SerializeField] ParticleSystem thrusterParticles;

    private void Awake()
    {
        Physics2D.autoSyncTransforms = true;
        playerState = GetComponent<Player_State>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        RotateAndMove();
    }

    private void RotateAndMove()
    {
        if (playerState.usePhysics)
        {
            if (playerState.inControl)
            {
                rigidBody.AddTorque(-playerState.xAxis * rotationSpeed);
                rigidBody.AddForce(transform.up * acceleration);
                if (rigidBody.velocity.magnitude > maxSpeed)
                {
                    rigidBody.velocity = rigidBody.velocity.normalized * maxSpeed;
                }
            }
        }

        else
        {
            velocity += (Vector2)transform.up * acceleration * Time.deltaTime;
            Vector2 velocityDirection = velocity.normalized;

            if (playerState.inControl)
            {
                transform.Rotate(Vector3.forward * rotationSpeed * -playerState.xAxis * Time.deltaTime);

                if (playerState.shieldBoost == true && velocity.sqrMagnitude > maxSpeed * maxSpeed)
                {
                    velocity *= 0.99f;
                }
                else if (velocity.sqrMagnitude > maxSpeed * maxSpeed)
                {
                    velocity = velocityDirection * maxSpeed;
                }
            }
            else
            {
                float adjustedSpeed = Mathf.MoveTowards(playerState.currentSpeed, maxSpeed / 2, Time.deltaTime * maxSpeed/10);                
                velocity = velocityDirection * adjustedSpeed;
            }
            transform.Translate(velocity * Time.deltaTime, Space.World);
            playerState.currentSpeed = velocity.magnitude;
        }
        
    }
}
