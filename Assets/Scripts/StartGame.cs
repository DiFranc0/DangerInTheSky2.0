using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartGame : MonoBehaviour
{
    public Animator toque;
    public static bool startGame = false;
   
    // Start is called before the first frame update
    void Start()
    {
        startGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            toque.SetTrigger("pToqueStart");
            toque.SetTrigger("pStart");


        }
    }


    public void BeginGame()
    {
        startGame = true;
        Spawner.startSpawner = true;
    }

    public void SlowDown()
    {
        GameManager.playerMaxFallingSpeed = 10f;
    }

}
