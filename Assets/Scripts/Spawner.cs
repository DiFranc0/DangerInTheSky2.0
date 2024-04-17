using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;

    Queue<ReciclagemObjeto> obstaclePool;

    public GameObject coin;
    public static bool startSpawner;
    public GameObject powerUp;
    private float distance;
    private float radius;
    private float interval;
    public int preInstantiate;
    private Vector3 spawnPos;
    public GameObject[] obstaclePrefabs;
    private int obstacleIndex;
    public PlayerScript playerController;
    bool toggleSpawn;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(Instance.gameObject);
            Instance = this;
        }

        obstaclePool = new Queue<ReciclagemObjeto>();
        for (int i = 0; i < preInstantiate; ++i)
        {
            spawnPos = Vector3.one * 10000;
            CreateNew();
        }

        
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void FixedUpdate()
    {
        if (PlayerScript.rb.velocity.z > 0f && startSpawner == true)
        {
            StartCoroutine(SpawnRepeating());
            startSpawner = false;
            
        }

        if (Altitude.distanceToPlayer < 3000.0f && Altitude.distanceToPlayer > 2300.0f)
        {
            StopAllCoroutines();
        }
        else if(Altitude.distanceToPlayer < 2299 && toggleSpawn == false)
        {
            StartCoroutine(SpawnRepeating());
            toggleSpawn = true;
        }
        else if(Altitude.distanceToPlayer < 520f)
        {
            StopAllCoroutines();
        }
    }

    public IEnumerator SpawnRepeating()
    {
        yield return new WaitForSeconds(GameManager.spawnDelay);
        while (true)
        {
            Spawn();
            interval = Random.Range(GameManager.spawnMinInterval, GameManager.spawnMaxInterval);
            yield return new WaitForSeconds(interval);
        }
    }

    public void Spawn()
    {
        distance = Random.Range(GameManager.spawnMinDistance, GameManager.spawnMaxDistance);
        radius = GameManager.objectSpawnRadius;

        spawnPos = new Vector3(
            Random.Range(-radius, radius),
            Random.Range(-radius, radius),
            playerController.transform.position.z + distance);
        
            if (TrySpawnPowerUp())
                return;
            if (TrySpawnCoin())
                return;
        if (obstaclePool.Count == 0)
        {
            CreateNew();
        }
        
        Reuse();
    }

    private void CreateNew()
    {
        obstacleIndex = Random.Range(0, obstaclePrefabs.Length);
        GameObject g = Instantiate(obstaclePrefabs[obstacleIndex], spawnPos, obstaclePrefabs[obstacleIndex].transform.rotation) as GameObject;
        obstaclePool.Enqueue(g.GetComponent<ReciclagemObjeto>());
    }

    private void Reuse()
    {
        ReciclagemObjeto r = obstaclePool.Dequeue();
        r.transform.position = spawnPos;
     
        r.TriggerAutoCollect();
    }

    public void Collect(ReciclagemObjeto toCollect)
    {
        obstaclePool.Enqueue(toCollect);
    }
    

    public bool TrySpawnPowerUp()
    {
        if (powerUp == null)
            return false;
        float probability = Random.value;
        if (probability < GameManager.powerUpFrequency)
        {
            GameObject g = Instantiate(powerUp, spawnPos, Quaternion.identity) as GameObject;
            Destroy(g, 10);
            return true;
        }
        return false;
    }

    public bool TrySpawnCoin()
    {
        if (coin == null)
            return false;
        float probability = Random.value;
        if (probability < GameManager.coinFrequency)
        {
            GameObject g = Instantiate(coin, spawnPos, coin.transform.rotation) as GameObject;
            Destroy(g, 10);
            return true;
        }
        return false;
    }
}
