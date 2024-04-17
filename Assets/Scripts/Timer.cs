using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI txtTimer;
    public static float timeLeft;
    bool timerOn = false;
    // Start is called before the first frame update
    void Start()
    {
        timerOn = false;
        timeLeft = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerScript.rb.velocity.z > 1)
        {
            timerOn = true;
            txtTimer.DOFade(1, 1);
        }

        if (timerOn)
        {
            timeLeft += Time.deltaTime;
            UpdateTimer(timeLeft);
        }
    }

    void UpdateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        txtTimer.SetText("{0:00} : {1:00}", minutes, seconds);
    }
}
