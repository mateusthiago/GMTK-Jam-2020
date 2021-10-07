using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy_Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs = new GameObject[4]; // squads must be in array in order of enemy quantity, big enemy must be 0
    [SerializeField] int amountToSpawn;
    [SerializeField] float minRandomTime;
    [SerializeField] float maxRandomTime;
    [SerializeField] float minRandomDistance;
    [SerializeField] float maxRandomDistance;
    public int currentEnemiesAlive = 0;
    int bigEnemy = 0;
    public int enemiesLeftToSpawn;
    public int enemiesLeftToKill;
    float spawnTimer;
    Player_State player;
    Vector2 cameraUnitSize;

    public float startTimer = 6;

    
    void Start()
    {
        enemiesLeftToSpawn = enemiesLeftToKill = amountToSpawn;
        spawnTimer = Random.Range(minRandomTime, maxRandomTime);
        player = FindObjectOfType<Player_State>();
        cameraUnitSize.y = Camera.main.orthographicSize * 2;
        cameraUnitSize.x = cameraUnitSize.y * Camera.main.aspect;
        print("cameraUnitSize = " + cameraUnitSize);
        UI_Controller.instance?.UpdateScore(enemiesLeftToKill); // update score at start to show enemies left
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer > 0)
        {
            startTimer -= Time.deltaTime;
            return;
        }
        if (!player || enemiesLeftToSpawn <= 0) return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0 && enemiesLeftToSpawn > 0)
        {
            SpawnNewEnemy();
        }
    }

    private void SpawnNewEnemy()
    {
        Vector2 outsideCameraPosition = (Vector2)player.transform.position + Random.insideUnitCircle.normalized * (cameraUnitSize.x + player.currentSpeed);

        int maxRandomEnemy = enemyPrefabs.Length - 1;
        while (maxRandomEnemy > enemiesLeftToSpawn) maxRandomEnemy--;
        int randomEnemy = Random.Range(0 + bigEnemy, maxRandomEnemy);
        var newEnemy = Instantiate(enemyPrefabs[randomEnemy], outsideCameraPosition, Quaternion.identity);
        float timeModifier = 1 + currentEnemiesAlive / 5;
        spawnTimer = Random.Range(minRandomTime, maxRandomTime) * timeModifier;
    }

    public void AddEnemy(bool isBigEnemy)
    {
        currentEnemiesAlive++;
        enemiesLeftToSpawn--;
        if (isBigEnemy) bigEnemy++;        
    }

    public void RemoveEnemy(bool isBigEnemy)
    {
        currentEnemiesAlive--;
        enemiesLeftToKill--;
        if (isBigEnemy) bigEnemy--;
        UI_Controller.instance?.UpdateScore(enemiesLeftToKill);        

        if (enemiesLeftToKill == 0)
        {
            UI_Controller.instance?.EndGame(true);
        }
    }
}
