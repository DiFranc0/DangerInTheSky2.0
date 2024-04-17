using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AviaoScript : MonoBehaviour
{
    public Material greenLight;
    public GameObject luz;
    public Light lightredgreen;
    public AudioSource auAviao;
    public AudioClip[] aviaoSounds;
    bool triggerGame = false;
    // Start is called before the first frame update
    void Start()
    {
        auAviao.clip = aviaoSounds[0];
        auAviao.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && triggerGame == false)
        {
            luz.GetComponent<MeshRenderer>().material = greenLight;
            lightredgreen.color = Color.green;

            StartCoroutine(AudioChange());
            triggerGame = true;
        }
    }

    IEnumerator AudioChange()
    {
        yield return new WaitForSeconds(2);
        auAviao.clip = aviaoSounds[1];
        auAviao.loop = false;
        auAviao.Play();
        

    }
}
