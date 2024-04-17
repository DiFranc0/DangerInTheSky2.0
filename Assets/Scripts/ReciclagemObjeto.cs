using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReciclagemObjeto : MonoBehaviour
{
    public float autoCollectMinDelay;
    public float autoCollectMaxDelay;

    public void TriggerAutoCollect()
    {
        CancelInvoke();
        Invoke("Collect", Random.Range(autoCollectMinDelay, autoCollectMaxDelay));
    }

    public void Collect()
    {
        Spawner.Instance.Collect(this);
    }
}
