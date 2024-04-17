/******** Copyright (c) John P. Doran, All rights reserved. ******************
//
// File Name :  QTEResetter.cs
// Author :     John P. Doran
//
// Purpose :    Helper utility to make it easy to reset QTEs within the scene.
//              Also contains data on if a QTE started the scene being active 
//              or not.
//
 * *****************************************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace QTESystem
{
    public class QTEResetter : MonoBehaviour
    {
        public List<QTE> qtes;

        private List<bool> startActive;

        public static QTEResetter instance;

        public UnityEvent onReset;

        private void Start()
        {
            instance = this;

            startActive = new List<bool>();

            // Get the default state of each QTE
            for (int i = 0; i < qtes.Count; ++i)
            {
                QTE qte = qtes[i];

                if (qte)
                {
                    startActive.Add(qte.gameObject.activeSelf);
                }

            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetAllQTEs();
            }
        }

        public void ResetAllQTEs()
        {
            onReset.Invoke();

            for (int i = 0; i < qtes.Count; ++i)
            {
                QTE qte = qtes[i];

                if(qte  != null)
                {
                    qte.Reset();
                    qte.gameObject.SetActive(startActive[i]);
                }

            }
        }

        public void NextScene()
        {
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
        }

        public void PrevScene()
        {
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex - 1) % SceneManager.sceneCountInBuildSettings);
        }
    }

}