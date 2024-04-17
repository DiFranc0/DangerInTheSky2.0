using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Config : MonoBehaviour
{
    public Transform _configJan;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterConfig()
    {
        _configJan.DOLocalMoveY(-206, 1);
    }

    public void Exit()
    {
        _configJan.DOLocalMoveY(206, 1);
    }
}
