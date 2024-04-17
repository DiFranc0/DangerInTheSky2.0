/******** Copyright (c) John P. Doran, All rights reserved. ******************
//
// File Name :  QTETimer.cs
// Author :     John P. Doran
//
// Purpose :    An abstract class meant to provide the fundamental properties
//              for other types of timers. Provides interface with base QTE
//              system.
//
*****************************************************************************/
using UnityEngine;

namespace QTESystem
{
    public abstract class QTETimer : MonoBehaviour
    {
        [Header("Object References")]
        [Tooltip("The QTE that this timer is referring to. Will look at the parent if this is null at start")]
        public QTE qte;

        [Tooltip("Color to use at the start of the timer's life")]
        public Color startColor = Color.green;

        [Tooltip("Color to use at the end of the timer's life")]
        public Color endColor = Color.red;

        // Ensures that delegates will only be added once
        private bool delegatesAdded = false;

        /// <summary>
        /// Called when object is active and enabled, handles setup
        /// </summary>
        protected virtual void OnEnable()
        {
            if(qte == null)
            {
                qte = transform.parent.GetComponent<QTE>();

                if(qte == null)
                {
                    Debug.LogWarning("Unable to find QTE for timer");
                    return;
                }
            }

            // Events should only be added once, this is what caused the previous issues
            

            if(!delegatesAdded)
            {
                qte.startEvent += StartTimer;
                qte.resetEvent += Reset;
                qte.cleanupEvent += StopTimer;

                delegatesAdded = true;
            }
            
        }

        /// <summary>
        /// Called at the start of the QTE
        /// </summary>
        /// <param name="time">How long the QTE will be until it is completed</param>
        public abstract void StartTimer(float time);

        /// <summary>
        /// Hides elements of the UI from the screen and take care of the end of the QTE
        /// </summary>
        protected virtual void StopTimer()
        {
            iTween.Stop(gameObject);
        }

        /// <summary>
        /// Used to reset the QTE back to where it would be before starting.
        /// </summary>
        public virtual void Reset()
        {
            StopTimer();

            iTween.Stop(gameObject);

            if (gameObject.activeSelf)
            {
                OnEnable();
            }
        }

    }
}

