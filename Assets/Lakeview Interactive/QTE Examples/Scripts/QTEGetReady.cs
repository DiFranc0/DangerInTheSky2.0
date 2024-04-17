using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QTEGetReady : MonoBehaviour
{
    public UnityEvent onComplete;

    public float flashTime = 3;

    public float timePerFlash = 0.25f;

    public float timeBeforeStart = 0.5f;

    private Text text;

    void Awake()
    {
        text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        StartCoroutine(GetReadyFlash());
    }

    private IEnumerator GetReadyFlash()
    {
        float time = 0;

        while(time < flashTime)
        {
            text.enabled = !text.enabled;

            yield return new WaitForSeconds(timePerFlash);

            time += timePerFlash;
        }

        text.enabled = true;

        yield return new WaitForSeconds(timeBeforeStart);

        text.enabled = false;

        onComplete.Invoke();

        gameObject.SetActive(false);
    }
}
