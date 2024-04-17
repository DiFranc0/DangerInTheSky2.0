/******** Copyright (c) John P. Doran, All rights reserved. ******************
//
// File Name :  SimpleCameraRotation.cs
// Author :     John P. Doran
//
// Purpose :    Continuously rotates the camera. Used to demonstrate how 
//              QTEs work in World Space
//
 * *****************************************************************************/
using UnityEngine;

public class SimpleCameraRotation : MonoBehaviour
{
    public Transform target; 


    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(target.position, Vector3.up,  15 * Time.deltaTime);
    }
}
