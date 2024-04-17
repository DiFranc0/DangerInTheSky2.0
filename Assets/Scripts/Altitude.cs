using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class Altitude : MonoBehaviour
{
    public TextMeshProUGUI txtAltitude;
    public float altitude;
    public static Vector3 playerPosition;
    public static Vector3 end;
    public static float distanceToPlayer;
    
    
  
    // Start is called before the first frame update
    void Start()
    {
        distanceToPlayer = 99999f;
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = new Vector3(PlayerScript.rb.position.x, PlayerScript.rb.position.y, PlayerScript.rb.position.z);
        end = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        distanceToPlayer = Vector3.Distance(playerPosition, end);
        altitude = distanceToPlayer;
        txtAltitude.SetText(altitude.ToString("N"));

        

    }

   
}
