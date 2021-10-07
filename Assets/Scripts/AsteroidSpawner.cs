using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] Asteroid asteroidPrefab;
    [SerializeField] float minAngleFromPlayer;
    [SerializeField] float maxAngleFromPlayer;
    [SerializeField] float minDistanceFromCamera;
    [SerializeField] float maxDistanceFromCamera;
    [SerializeField] float minAsteroidSize;
    [SerializeField] float maxAsteroidSize;
    [SerializeField] float minTimeToSpawn;
    [SerializeField] float maxTimeToSpawn;
    [SerializeField] int initialSpawnAmount;    
    [SerializeField] int maxAmountPerSpawn;
    [SerializeField] float clusterDensity;
    [SerializeField] int minActiveAsteroids;
    [SerializeField] int maxActiveAsteroids;    

    Player_State player;
    

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player_State>();
        if (player == null)
        {
            print("PLAYER NOT FOUND!");
            Debug.Break();
        }

        SpawnAsteroids();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnAsteroids()
    {
        Vector2 playerPosition = player.transform.position;        
        float angle = 90 + player.transform.eulerAngles.z + Random.Range(minAngleFromPlayer, maxAngleFromPlayer);
        int chosenSide = angle >= 90 ? -1 : 1; //Random.Range(0, 2) * 2 - 1;
        float angleIteration = clusterDensity;
        float distance = Random.Range(10, 30);

        for (int i = 0; i < initialSpawnAmount; i++)
        {
            Vector2 newPosition = playerPosition + GetSpawnPosition(angle, distance);
            var newAsteroid = Instantiate(asteroidPrefab, newPosition, Quaternion.identity);
            float size = Random.Range(minAsteroidSize, maxAsteroidSize);
            print(string.Format("Spawned at angle {0} and distance {1}, with size {2}", angle, distance, size));
            newAsteroid.SetSize(size);
            newAsteroid.transform.parent = this.transform;
            
            angle += (angleIteration + size * 2) * chosenSide;     
            distance = Random.Range(10, 30);
        }
    }

    private Vector2 GetSpawnPosition(float angle, float distance)
    {
        Vector2 position;
        position.x = Mathf.Cos(angle * Mathf.Deg2Rad);
        position.y = Mathf.Sin(angle * Mathf.Deg2Rad);
        position.Normalize();
        position *= distance;        
        return position;
    }
}
