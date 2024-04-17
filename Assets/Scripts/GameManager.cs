using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Vector3 gravity = new Vector3(0f, 0f, 9.81f);
    public GameObject player;
    public static float playerMaxFallingSpeed = 40.0f;
    public static float playerMovementRadius = 7f;
    public static float objectSpawnRadius = 2f;
    public static float playerMovementSpeed = 2f;
    public static float spawnMinInterval = 0.5f;
    public static float spawnMaxInterval = 1.0f;
    public static float spawnMinDistance = 140f;
    public static float spawnMaxDistance = 150f;
    public static float powerUpFrequency = 0.07f;
    public static float coinFrequency = 0.5f;
    public static float spawnDelay = 3f;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = gravity;
        playerMaxFallingSpeed = 40.0f;
        playerMovementRadius = 7f;


    }

    // Update is called once per frame
    void Update()
    {

        
    }
}
