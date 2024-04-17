using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressoAltitude : MonoBehaviour
{
    Slider alt;
    // Start is called before the first frame update
    void Start()
    {
        alt = gameObject.GetComponent<Slider>();
        alt.maxValue = 4000f;
        alt.minValue = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        alt.value = Altitude.distanceToPlayer;
    }
}
