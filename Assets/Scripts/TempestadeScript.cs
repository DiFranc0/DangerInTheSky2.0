using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TempestadeScript : MonoBehaviour
{
    public TextMeshProUGUI danger;
    public Animator anDanger;
    public AudioClip[] audios;
    public AudioSource aud;
    
    bool triggerSound = false;
    
    // Start is called before the first frame update
    void Start()
    {
        danger.enabled = false;
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Altitude.distanceToPlayer < 3000.0f && Altitude.distanceToPlayer > 2800.0f)
        {
            danger.enabled = true;
            anDanger.SetBool("pDanger", true);
            triggerSound = true;
            
            
        }
        else
        {
            danger.enabled = false;
            anDanger.SetBool("pDanger", false);
           
        }


        if(Altitude.distanceToPlayer < 2800f && triggerSound == true)
        {
            aud.clip = audios[1];
            aud.Play();
            triggerSound = false;
        }

        if(Altitude.distanceToPlayer < 2200f)
        {
            aud.Stop();
        }
    }

}
