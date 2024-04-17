/******** Copyright (c) John P. Doran, All rights reserved. ******************
//
// File Name :  QTE.cs
// Author :     John P. Doran
//
// Purpose :    Base class from which all other QTEs come from, must be 
//              inherited in order to be used.
//
*****************************************************************************/
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace QTESystem
{
    // declare delegates
    public delegate void QTEActionEvent(float time);
    public delegate void QTEEvent();

    public abstract class QTE : MonoBehaviour
    {
        /// <summary>
        /// The current state of the QTE
        /// </summary>
        public enum QTEState
        {
            Inactive,
            Active,
            Success,
            Failed
        }

        public enum InputMode
        {
            Keyboard,
            Controller,
            Mobile
        }

        //declare event of type delegate
        public event QTEActionEvent startEvent;
        public event QTEEvent cleanupEvent;
        public event QTEEvent resetEvent;

        [Header("Base QTE Settings")]
        [Tooltip("How long to wait until the QTE starts")]
        public float delay = 1f;

        [Tooltip("After starting, how long should the QTE last")]
        public float time = 3f;

        [Tooltip("Should the QTE start immediately?")]
        public bool startOnAwake = true;

        /// <summary>
        /// Displays what key should be pressed
        /// </summary>
        protected Image input;

        [HideInInspector]
        [Tooltip("The current state of the QTE")]
        public QTEState state = QTEState.Inactive;

        [Header("Success")]
        [Tooltip("The sprite that the input will change to upon the player pressing the correct input in time")]
        public Sprite successSprite;

        [Tooltip("A list of callbacks which will be called upon the player successfully completing the QTE")]
        public UnityEvent onSuccess;

        [Header("Failure")]
        [Tooltip("The sprite that the input will change to upon the player not pressing the correct input in time")]
        public Sprite failureSprite;

        [Tooltip("A list of callbacks which will be called upon the player failing to complete the QTE")]
        public UnityEvent onFailure;

        public static InputMode inputMode
        {
            get
            {
                if(Application.platform == RuntimePlatform.Android || 
                    Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    return InputMode.Mobile;
                }

                return (InputMode) PlayerPrefs.GetInt("InputMode");
            }
            set
            {
                PlayerPrefs.SetInt("InputMode", (int) value);
            }
        }

        /// <summary>
        /// Called when object is active and enabled, handles setup
        /// </summary>
        protected virtual void OnEnable()
        {
            var inputGO = transform.Find("Input");

            if(inputGO)
            {
                input = inputGO.GetComponent<Image>();
            }

            state = QTEState.Inactive;

            // Reset actions required
            StopAllCoroutines();

            iTween.Stop(gameObject);

            UpdateInputAlpha(1.0f);

            if (startOnAwake)
            {
                BeginQTE();
            }

        }

        /// <summary>
        /// Takes care of the implementation of the QTE
        /// </summary>
        /// <returns>Information needed by the coroutine</returns>
        protected virtual IEnumerator QTEAction()
        {
            state = QTEState.Active;

            yield return new WaitForSeconds(delay);

            // Need to account for fact someone could succeed before starting this
            if (state == QTEState.Active)
            {
                OnStartEvent();

                yield return new WaitForSeconds(time);

                QTEFailure();
            }

        }

        protected virtual void OnStartEvent()
        {
            if (startEvent != null)
            {
                startEvent.Invoke(time);
            }
        }

        /// <summary>
        /// Called upon a QTE being failed
        /// </summary>
        public virtual void QTEFailure()
        {
            state = QTEState.Failed;

            if(input)
            {
                input.sprite = failureSprite;
            }
            
            CleanUp();

            onFailure.Invoke();
        }

        /// <summary>
        /// Called upon a QTE being successful
        /// </summary>
        public virtual void QTESuccess()
        {
            state = QTEState.Success;

            if(input)
            {
                input.sprite = successSprite;
            }
            
            CleanUp();

            onSuccess.Invoke();
        }

        /// <summary>
        /// Hides elements of the UI from the screen and take care of the end of the QTE
        /// </summary>
        protected virtual void CleanUp()
        {
            StopAllCoroutines();

            OnCleanUpEvent();

            iTween.Stop(gameObject);

            if(input != null)
            {
                iTween.ValueTo(this.gameObject, iTween.Hash("from", input.color.a, "to", 0, "delay", 0.5f, "time", 0.25f, "onupdate", "UpdateInputAlpha"));
            }
            
        }

        public virtual void UpdateInputAlpha(float val)
        {
            if(input != null)
            {
                var newColor = input.color;
                newColor.a = val;
                input.color = newColor;
            }
           
        }

        protected void OnCleanUpEvent()
        {
            if (cleanupEvent != null)
            {
                cleanupEvent.Invoke();
            }
        }

        /// <summary>
        /// Used to reset the QTE back to where it would be before starting.
        /// </summary>
        public virtual void Reset()
        {
            CleanUp();

            OnResetEvent();

            StopCoroutine(QTEAction());

            iTween.Stop(gameObject);

            UpdateInputAlpha(1.0f);

            if (gameObject.activeSelf)
            {
                OnEnable();
            }
        }

        protected void OnResetEvent()
        {
            if (resetEvent != null)
            {
                resetEvent.Invoke();
            }
        }

        /// <summary>
        /// Helper function to start a QTE after a delay
        /// </summary>
        /// <param name="time"></param>
        public void BeginAfterDelay(float time)
        {
            Invoke("BeginQTE", time);
        }

        /// <summary>
        /// Helper function to start QTE from UI or elsewhere
        /// </summary>
        public virtual void BeginQTE()
        {
            gameObject.SetActive(true);

            StartCoroutine(QTEAction());
        }

#if UNITY_EDITOR
        [MenuItem("QTE System/Switch to Controller")]
#endif
        public static void SwitchToController()
        {
            SetInputMode(InputMode.Controller);
        }
#if UNITY_EDITOR
        [MenuItem("QTE System/Switch to Keyboard")]
#endif
        public static void SwitchToKeyboard()
        {
            SetInputMode(InputMode.Keyboard);
        }
#if UNITY_EDITOR
        [MenuItem("QTE System/Switch to Mobile")]
#endif
        public static void SwitchToMobile()
        {
            SetInputMode(InputMode.Mobile);
        }

        public static void SetInputMode(InputMode newMode)
        {
            inputMode = newMode;
        }


    }

}
