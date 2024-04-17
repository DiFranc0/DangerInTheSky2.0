/******** Copyright (c) John P. Doran, All rights reserved. ******************
//
// File Name :  MenuHelpers.cs
// Author :     John P. Doran
//
// Purpose :    Helper utility for UI elements on the example levels.
//
 * *****************************************************************************/
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace QTESystem
{
    public class MenuHelpers : MonoBehaviour
    {
        public void SwitchToController()
        {
            QTE.SwitchToController();
            EventSystem.current.SetSelectedGameObject(null);

            ResetElements();
        }

        public void SwitchToKeyboard()
        {
            QTE.SwitchToKeyboard();
            EventSystem.current.SetSelectedGameObject(null);
            ResetElements();
        }

        public void SwitchToMobile()
        {
            QTE.SwitchToMobile();
            EventSystem.current.SetSelectedGameObject(null);
            ResetElements();
        }

        private void ResetElements()
        {
            if(QTEResetter.instance != null)
            {
                QTEResetter.instance.ResetAllQTEs();
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

