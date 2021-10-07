using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Asteroid : MonoBehaviour
{
    public CircleCollider2D collider;
    public Vector2 rotationSpeedRange;
    public Vector2 travelSpeedRange;
    public float size;
    [HideInInspector] public float speedExchangeOnCollision;
    float rotationSpeed;
    float travelSpeed;
    Vector2 direction;

    void Start()
    {
        int randomSide = Random.Range(0, 2) * 2 - 1;
        rotationSpeed = Random.Range(rotationSpeedRange.x, rotationSpeedRange.y) * randomSide;
        travelSpeed = Random.Range(travelSpeedRange.x, travelSpeedRange.y);
        direction = Random.insideUnitCircle;
        speedExchangeOnCollision = travelSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        transform.Translate(direction * travelSpeed * Time.deltaTime, Space.World);
        travelSpeed *= 0.999f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        direction = (transform.position - collision.transform.position).normalized;
        rotationSpeed = rotationSpeed + Mathf.Sign(rotationSpeed) * 1 / size;
        
        if (collision.tag != "Obstacle")
        {
            if (collision.tag == "E_Bullet" || collision.tag == "P_Bullet")
            {
                transform.Translate(direction * (0.01f / size) * Time.deltaTime);
                travelSpeed += 0.05f;
                return;
            }

            float addSpeed = 0;
            if (collision.transform.root.tag == "Player") 
            {
                addSpeed = collision.transform.root.GetComponent<Player_State>().currentSpeed/2;
            }
            if (collision.transform.root.tag == "Enemy")
            {
                addSpeed = collision.transform.root.GetComponent<Enemy>().currentSpeed/2;
            }

            transform.Translate(direction * (2 / size) * Time.deltaTime);
            speedExchangeOnCollision = travelSpeed + addSpeed;
        }
        else if (collision.tag == "Obstacle")
        {
            Asteroid otherAsteroid = collision.GetComponent<Asteroid>();            
            this.speedExchangeOnCollision = otherAsteroid.travelSpeed;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        travelSpeed = speedExchangeOnCollision;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        direction = (transform.position - collision.transform.position).normalized;
        rotationSpeed = rotationSpeed + Mathf.Sign(rotationSpeed) * 1 / size;
        if (collision.tag == "E_Bullet" || collision.tag == "P_Bullet")
        {
            transform.Translate(direction * (0.01f / size) * Time.deltaTime);
        }
        else transform.Translate(direction * (2 / size) * Time.deltaTime);
    }

    public void SetSize(float size)
    {
        this.size = size;        
        transform.localScale = Vector3.one * size;
    }
}
