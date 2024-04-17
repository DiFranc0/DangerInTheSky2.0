using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDanger : MonoBehaviour
{
    public AudioSource auCanvas;
    public AudioClip dangerAudio;
    bool triggerDanger = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Altitude.distanceToPlayer < 3000 && triggerDanger == false)
        {
            auCanvas.clip = dangerAudio;
            auCanvas.Play();
            triggerDanger = true;

        }
    }
}
