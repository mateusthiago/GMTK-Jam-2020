using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rigidBody;
    [SerializeField] Transform gunPoint;
    public int damage;
    float decayTime;
    Vector2 velocity;
    public bool isReady = true;
    Transform bulletContainer;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        gunPoint = transform.parent;
        bulletContainer = GameObject.Find("Bullets Container").transform;
        Reset();
    }

    private void Update()
    {
        if (isReady) return;

        Move();
        CheckDecay();
    }

    private void Move()
    {
        transform.Translate(velocity * Time.deltaTime, Space.World);
    }
    public void Shoot(Vector2 gunFacing, Vector2 bulletVelocity, float time)
    {
        transform.parent = bulletContainer;
        gameObject.SetActive(true);
        isReady = false;
        transform.up = gunFacing;
        velocity = bulletVelocity;
        decayTime = time;
    }

    private void Reset()
    {        
        isReady = true;
        if (gunPoint == null)
        {
            Destroy(this.gameObject);
            return;
        }
        transform.parent = gunPoint;
        transform.localPosition = Vector3.zero;
        decayTime = 0;
        rigidBody.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Reset();
    }

    private void CheckDecay()
    {
        if (decayTime > 0)
        {
            decayTime -= Time.deltaTime;
        }
        else
        {
            Reset();
        }
    }
}
