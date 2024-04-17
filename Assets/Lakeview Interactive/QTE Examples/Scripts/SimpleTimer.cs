using UnityEngine;
using UnityEngine.Events;

public class SimpleTimer : MonoBehaviour
{
    // How much time to wait
    public float timeDelay;

    // What to call after the time has finished
    public UnityEvent onFinished;

    void OnEnable()
    {
        // Stops the event if it's already started
        CancelInvoke("CallEvent");

        // Starts a new one
        Invoke("CallEvent", timeDelay);
    }

    void CallEvent()
    {
        Debug.Log("Call Event");
        onFinished.Invoke();
    }
}
