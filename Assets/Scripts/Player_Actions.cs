using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Actions : MonoBehaviour
{
    Player_State playerState;
    Player_Movement playerMovement;

    [SerializeField] Transform gun;
    [SerializeField] Bullet[] bullets = new Bullet[20];
    [SerializeField] float gunRotationSpeed;
    [SerializeField] float fireRate;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletDecay;
    [SerializeField] float boostForce;
    [SerializeField] Shield shield;
    [SerializeField] ParticleSystem DeathExplosion;
    public BoxCollider2D bodyCollider;
    float fireCooldown;
    public float boostCooldown = 5;
    float cooldownTimer = 0;
    Rigidbody2D myRigidbody;

    
    private void Awake()
    {
        playerState = GetComponent<Player_State>();
        playerMovement = GetComponent<Player_Movement>();
        myRigidbody = GetComponent<Rigidbody2D>();
        Player_Input.shieldBoost += ShieldBoost;        
    }

    private void Update()
    {        
        RotateGun();
        Fire();

        if (cooldownTimer < boostCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (playerState.shieldBoost == true && cooldownTimer >= 1) playerState.shieldBoost = false;
            if (cooldownTimer >= boostCooldown) cooldownTimer = boostCooldown;
            UI_Controller.instance?.UpdateBoostLabel(cooldownTimer / boostCooldown);
        }
    }

    private void RotateGun()
    {
        if (!playerState.inControl)
        {
            gun.Rotate(Vector3.forward * -playerState.xAxis * gunRotationSpeed * Time.deltaTime);
        }
    }

    private void Fire()
    {
        if (playerState.inControl || shield.isActive) return;

        if (fireCooldown > 0)
        {
            fireCooldown -= fireRate * Time.deltaTime;
            return;
        }

        for (int i = 0; i < bullets.Length; i++)
        {
            if (bullets[i].isReady)
            {                    
                Vector2 velocity = playerState.usePhysics ? myRigidbody.velocity : playerMovement.velocity;
                velocity += (Vector2)gun.transform.up * bulletSpeed;
                bullets[i].Shoot(gun.transform.up, velocity, bulletDecay);
                fireCooldown = 1;
                break;
            }
        }            
        
    }

    public void ShieldBoost()
    {
        if (cooldownTimer < boostCooldown) return;

        if (playerState.inControl)
        {
            if (playerState.usePhysics)
            {
                GetComponent<Rigidbody2D>().AddForce(transform.up * boostForce, ForceMode2D.Impulse);
            }
            else
            {
                playerState.shieldBoost = true;                
                shield.gameObject.SetActive(true);
                shield.transform.localScale = Vector3.one;
                playerMovement.velocity += (Vector2)transform.up * boostForce;
            }
        }
        else
        {
            if (playerState.usePhysics)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            else
            {
                playerState.shieldBoost = true;                
                shield.gameObject.SetActive(true);
                shield.transform.localScale = Vector3.one * 2f;
                playerMovement.velocity = Vector2.zero;
                playerState.currentSpeed = 0;
            }
        }

        cooldownTimer = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "P_Bullet") return; //se ativar o shield assim que a bullet tiver sido instanciada

        if (collision.tag == "E_Bullet")
        {
            GetComponent<Health>().TakeDamage(collision.GetComponent<Bullet>().damage);
        }
        else
        {
            print("PLAYER COLLISION WITH " + collision.tag);
            if (playerState.shieldBoost == false)
            {
                BounceOffCollision(collision.transform.position);
                var health = GetComponent<Health>();
                int damage = 1;

                if (collision.tag == "Obstacle")
                {
                    damage = (int)(playerState.currentSpeed / 2);                    
                }
                if (collision.tag == "Enemy" || collision.tag == "Shield")
                {
                    Enemy enemy = collision.transform.root.GetComponent<Enemy>();
                    if (enemy == null) return;
                    damage = (int)(enemy.currentSpeed / 2);                    
                }

                health.TakeDamage(damage);                
            }  
            if (playerState.shieldBoost == true && collision.tag == "Obstacle")
            {
                BounceOffCollision(collision.transform.position);
            }
        }
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.tag == "Obstacle" || collision.tag == "Shield")
    //    {
    //        BounceOffCollision(collision.transform.position);
    //    }
    //}

    private void BounceOffCollision(Vector3 collisionPosition)
    {
        Vector2 direction = (transform.position - collisionPosition).normalized;        
        transform.up += (Vector3)direction;
        var movement = GetComponent<Player_Movement>();
        movement.velocity = transform.up * playerState.currentSpeed / 2;
    }

    public void DeathEvent()
    {
        Player_Input.shieldBoost -= ShieldBoost;
        UI_Controller.instance?.EndGame(false);
        DeathExplosion.transform.parent = null;
        DeathExplosion.Play();
        Destroy(DeathExplosion.gameObject, 6f);
        Destroy(this.gameObject);
    }
}
