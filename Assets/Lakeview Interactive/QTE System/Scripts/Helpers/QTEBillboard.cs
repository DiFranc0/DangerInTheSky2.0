/******** Copyright (c) John P. Doran, All rights reserved. ******************
//
// File Name :  QTEBillboard.cs
// Author :     John P. Doran
//
// Purpose :    Helper utility to make it easy to reset QTEs within the scene.
//              Also contains data on if a QTE started the scene being active 
//              or not.
//
 * *****************************************************************************/
using UnityEngine;

namespace QTESystem
{
    public class QTEBillboard : MonoBehaviour
    {
        private Transform camTransform;

        //Quaternion originalRotation;

        void Start()
        {
            camTransform = Camera.main.transform;

            //originalRotation = transform.rotation;
        }

        void Update()
        {
            transform.LookAt(transform.position + camTransform.rotation * Vector3.forward,
               camTransform.transform.rotation * Vector3.up);
        }
    }
}

