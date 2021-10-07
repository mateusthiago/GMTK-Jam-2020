using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    Player_State player;
    [SerializeField] bool isBigEnemy;
    [SerializeField] float rotationSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;
    [SerializeField] public Vector2 velocity;
    [SerializeField] public float currentSpeed;
    [Space]
    [SerializeField] float bulletSpeed;
    [SerializeField] float fireRate;
    [SerializeField] float bulletDecay;
    [SerializeField] float aimAngleTolerance;
    [SerializeField] float minPlayerDistance;
    [SerializeField] float playerDistance;    
    Vector2 playerFuturePosition;
    Vector2 playerFutureDirection;
    float playerFutureAngle;
    [SerializeField] float waitToGetPlayer;    
    float fireCooldown;
    [SerializeField] Bullet[] bullets = new Bullet[10];
    [SerializeField] LayerMask collisionMask;
    [SerializeField] ParticleSystem DeathExplosion;
    [SerializeField]public Shield shield;
    bool shieldOn;
    public Collider2D bodyCollider;
    public float collisionCheckRadius;
    Collider2D[] collisionContacts = new Collider2D[6];
    Vector2 nearestContactPoint;
    int contactCount;
    Vector3 playerDirection;
    float playerAngle;
    public bool playerCollisionNear;
    public float timerToGetPlayer;

    public static float obstacleAvoidanceWeight = 6f;
    public static float alignmentWeight = 2f;
    public static float avoidanceWeight = 6f;
    public static float chasePlayerWeight = 4f;
    

    private void Start()
    {
        var squadContainer = transform.root;
        if (squadContainer.tag == "Squad")
        {
            transform.parent = null;
            if (squadContainer.childCount == 0) Destroy(squadContainer.gameObject);
        }
        var spawner = FindObjectOfType<Enemy_Spawner>()?.GetComponent<Enemy_Spawner>();
        if (spawner)
        {
            name = "Enemy " + spawner.enemiesLeftToSpawn;
            spawner.AddEnemy(isBigEnemy);
        }
        player = FindObjectOfType<Player_State>();
        //playerDirection = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
        //transform.up = playerDirection;
    }


    private void Update()
    {

        if (player != null)
        {
            GetPlayerData();
            Fire();
        }

        CalculateSteering();
        Move();        
        
    }

    private void GetPlayerData()
    {
        // pega a distancia, a direção do player, e o angulo entre o up e o player
        playerDistance = ((Vector2)player.transform.position - (Vector2)transform.position).magnitude;
        playerDirection = (player.transform.position - transform.position).normalized;
        playerAngle = Vector2.Angle(transform.up, playerDirection);

        playerFuturePosition = player.transform.position + player.transform.up * player.currentSpeed * Time.deltaTime * playerDistance;
        playerFutureDirection = (playerFuturePosition - (Vector2)transform.position).normalized;
        playerFutureAngle = Vector2.Angle(transform.up, playerFutureDirection);
        Debug.DrawLine(playerFuturePosition - Vector2.one * 0.5f, playerFuturePosition + Vector2.one * 0.5f, Color.red);
        GUIDebugger.Add("\n" + this.name + "\n");
        GUIDebugger.Add(String.Format("playerAngle = {0:0.00}", playerAngle));
        GUIDebugger.Add(String.Format("playerFutureAngle = {0:0.00}", playerFutureAngle));
        GUIDebugger.Add("playerDistance = " + playerDistance);
    }

    private void CalculateSteering()
    {        
        Vector3 turnDirection = transform.up;
        playerCollisionNear = false;
        contactCount = Physics2D.OverlapCircleNonAlloc(transform.position, collisionCheckRadius, collisionContacts, collisionMask);

        for (int i = 0; i < contactCount; i++)
        {
            nearestContactPoint = collisionContacts[i].ClosestPoint(transform.position);
            float contactDistance = ((Vector2)transform.position - nearestContactPoint).magnitude;
            Vector3 contactDirection = (nearestContactPoint - (Vector2)transform.position).normalized;
            float contactAngle = Vector2.Angle(transform.up, contactDirection);

            if (Mathf.Abs(contactAngle) > 90 || collisionContacts[i].gameObject == this.gameObject)
            {
                continue;
            }            

            if (collisionContacts[i].gameObject.tag == "Enemy")
            {
                if (contactDistance < 3)
                {
                    Vector3 avoidanceSteering = -contactDirection * avoidanceWeight;
                    turnDirection += avoidanceSteering;
                    Debug.DrawRay(transform.position, avoidanceSteering, Color.yellow);
                }
                else
                {
                    Vector3 alignmentSteering = collisionContacts[i].transform.up * alignmentWeight;
                    turnDirection += alignmentSteering;
                    Debug.DrawRay(transform.position, alignmentSteering, Color.cyan);
                }
            }
            else
            {
                if (contactDistance <= 3.5f) UseShield();

                if (collisionContacts[i].gameObject.tag == "Player")
                {
                    playerCollisionNear = true;
                }
                
                Vector3 avoidObstacleSteering = Vector2.Lerp(transform.up, -contactDirection, 0.75f).normalized * obstacleAvoidanceWeight;
                turnDirection += avoidObstacleSteering;
                Debug.DrawRay(transform.position, avoidObstacleSteering, Color.red);
            }
        }

        if (playerCollisionNear == false)
        {
            float tolerance = playerDistance / minPlayerDistance;
            if (playerAngle < 90 || tolerance > 1.5f)
            {                
                GUIDebugger.Add("tolerance = " + tolerance);
                Vector3 chaseSteering = playerFutureDirection * chasePlayerWeight;
                turnDirection += chaseSteering;
                Debug.DrawRay(transform.position, chaseSteering, Color.green);
                Debug.DrawRay(transform.position, playerFutureDirection * 100, Color.blue);
            }
        }

        Debug.DrawRay(transform.position, turnDirection, Color.magenta);
        turnDirection = turnDirection.normalized;
        float directionAngle = Vector2.SignedAngle(transform.up, turnDirection);
        float currentAngle = transform.eulerAngles.z;
        float targetAngle = currentAngle + directionAngle;
        float newRotation = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        GUIDebugger.Add("targetAngle = " + targetAngle);
        GUIDebugger.Add("newRotation = " + newRotation);
        transform.eulerAngles = Vector3.forward * newRotation;
        //transform.up = Vector3.Lerp(transform.up, turnDirection, rotationSpeed * Time.deltaTime);
    }

    private void Move()
    {
        velocity += (Vector2)transform.up * acceleration * Time.deltaTime;
        Vector2 velocityDirection = velocity.normalized;  
        
        if (playerDistance <= minPlayerDistance * 0.66f)
        {
            float minSpeed = Mathf.Max(player.currentSpeed, maxSpeed * 0.75f);
            float adjustedSpeed = Mathf.MoveTowards(currentSpeed, minSpeed, Time.deltaTime * maxSpeed);
            velocity = velocityDirection * adjustedSpeed;
        }
        else 
        {            
            float adjustedSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, Time.deltaTime * maxSpeed * 0.8f);
            velocity = velocityDirection * adjustedSpeed;
        }        
        
        currentSpeed = velocity.magnitude;
        transform.Translate(velocity * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "P_Bullet")
        {
            if (bodyCollider.IsTouching(collision)) GetComponent<Health>().TakeDamage(collision.GetComponent<Bullet>().damage);
        }
        else if (collision.tag == "Enemy")
        {
            Enemy otherEnemy = collision.transform.root.GetComponent<Enemy>();
            if (otherEnemy && bodyCollider.IsTouching(otherEnemy.bodyCollider))
            {                
                DeathEvent();
            }
        }
        else if (collision.tag == "Shield" || collision.tag == "Obstacle" || collision.tag == "Player")
        {            
            if (bodyCollider.IsTouching(collision)) DeathEvent();
        }
    }

    public void UseShield()
    {
        if (!shieldOn)
        {
            shieldOn = true;
            shield.gameObject.SetActive(true);
            StartCoroutine(ShieldTimer());
        }
    }

    private IEnumerator ShieldTimer()
    {
        yield return new WaitForSeconds(1f);
        shield.gameObject.SetActive(false);
        yield return new WaitForSeconds(4f);
        shieldOn = false;
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, collisionCheckRadius);

        for (int i = 0; i < contactCount; i++)
        {
            if (collisionContacts[i] != null && collisionContacts[i].gameObject != this.gameObject)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(collisionContacts[i].ClosestPoint(transform.position), 0.2f);
            }
        }
    }

    private void Fire()
    {
        if (fireCooldown > 0)
        {
            fireCooldown -= fireRate * Time.deltaTime;
            return;
        }

        if (!PlayerOnAim()) return;

        for (int i = 0; i < bullets.Length; i++)
        {
            if (bullets[i].isReady)
            {
                var bulletVelocity = transform.up * (bulletSpeed + currentSpeed);
                bullets[i].Shoot(transform.up, bulletVelocity, bulletDecay);
                fireCooldown = 1;
                break;
            }
        }

    }

    private bool PlayerOnAim()
    {        
        if (playerFutureAngle <= aimAngleTolerance)
        {            
            if (playerDistance <= minPlayerDistance) return true;
        }
        
        return false;
    }

    public void DeathEvent()
    {
        var spawner = FindObjectOfType<Enemy_Spawner>()?.GetComponent<Enemy_Spawner>();
        if (spawner) spawner.RemoveEnemy(isBigEnemy);
        DeathExplosion.transform.parent = null;
        DeathExplosion.Play();
        Destroy(DeathExplosion.gameObject, 4f);
        Destroy(this.gameObject);
    }
}
