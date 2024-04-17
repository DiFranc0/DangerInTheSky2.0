using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;


public class ActiveAltitude : MonoBehaviour
{
    
    bool triggerAlt = false;
    public TextMeshProUGUI txtAltitude1;
    public TextMeshProUGUI txtAltitude2;
    public Transform _altitudeTransform;
    public Image fill;
    public Image background;
    public Image meter;
    // Start is called before the first frame update
    void Start()
    {
        triggerAlt = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (PlayerScript.rb.velocity.z > 1 && triggerAlt)
        {
            DOTween.Sequence().Join(_altitudeTransform.DOLocalMoveX(-332, 1)).Join(txtAltitude1.DOFade(1, 1)).Join(txtAltitude2.DOFade(1, 1)).Join(fill.DOFade(1, 1)).Join(background.DOFade(1, 1)).Join(meter.DOFade(1, 1));
            triggerAlt = false;

        }
    }
}
