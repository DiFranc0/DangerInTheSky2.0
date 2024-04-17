using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScript : MonoBehaviour
{
    public Animator an;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Altitude.distanceToPlayer < 3000.0f)
        {
            an.SetTrigger("pTempestade");
        }

        if (Altitude.distanceToPlayer < 2200f)
        {
            an.SetTrigger("pTempoBom");
        }
    }
}
