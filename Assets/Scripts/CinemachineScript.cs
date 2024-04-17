using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineScript : MonoBehaviour
{
    public Animator an;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (StartGame.startGame == true)
        {
            an.SetTrigger("ChangeCamera");

        }

        if (Input.GetButtonDown("Jump"))
        {
            an.SetTrigger("pJumpCam");
        }

        if(Altitude.distanceToPlayer < 400f)
        {
            an.SetTrigger("toOpening");
            GameManager.playerMovementRadius = 20f;
        }
    }

    
}
