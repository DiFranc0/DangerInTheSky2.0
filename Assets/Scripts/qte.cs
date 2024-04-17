using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class qte : MonoBehaviour
{
    public GameObject quickTime;
    // Start is called before the first frame update
    void Start()
    {
        quickTime.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Altitude.distanceToPlayer < 400f)
        {
            quickTime.SetActive(true);
        }
    }

    public void Success()
    {
        Score.pontos += 50;
    }

    public void Failure()
    {
        Score.pontos -= 50;
    }
}
