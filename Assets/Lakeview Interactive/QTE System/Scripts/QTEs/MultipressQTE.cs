/******** Copyright (c) John P. Doran, All rights reserved. ******************
//
// File Name :  MultipressQTE.cs
// Author :     John P. Doran
//
// Purpose :    A form of QTE that utilizes pressing multiple buttons at the 
//              same time in order to function properly.
//
*****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace QTESystem
{
    public class MultipressQTE : QTE
    {
        [Header("Multi-Press QTE Settings")]

        [Tooltip("Prefab to be used if we need to add more elements")]
        public RectTransform qteObj;

        [Tooltip("Prefab to be used if we need to add more elements")]
        public RectTransform plusObj;

        [Tooltip("All of the inputs that need to be pressed in order to count as successful")]
        public List<InputData> inputDatas;
        [Tooltip("Should the player fail the QTE if they hit any other key")]
        public bool failIfIncorrect = false;

        /// <summary>
        /// A list of all of the inputs
        /// </summary>
        private List<Image> inputs;

        /// <summary>
        /// A reference to the elements game object to place all of the new additions if needed
        /// </summary>
        private Transform elements;

        public override void Reset()
        {
            CleanUp();

            iTween.Stop(gameObject);

            OnResetEvent();

            // Trying to reset before calling once
            if(inputs == null)
            {
                return;
            }

            foreach (var input in inputs)
            {
                iTween.Stop(input.gameObject);

                var color = input.color;
                color.a = 1;
                input.color = color;
            }

            // Check for validity
            if(elements == null)
            {
                return;
            }

            foreach (Transform child in elements)
            {
                if (child.name.Contains("+"))
                {
                    var image = child.GetComponent<Image>();

                    iTween.Stop(image.gameObject);

                    var color = image.color;
                    color.a = 1;
                    image.color = color;
                }
            }

            if (gameObject.activeSelf)
            {
                OnEnable();
            }
        }

        protected override void OnEnable()
        {
            inputs = new List<Image>();

            elements = transform.Find("Elements");

            InitializeInputs();

            state = QTEState.Inactive;

            if (startOnAwake)
            {
                BeginQTE();
            }
        }

        /// <summary>
        /// Inititalizes all of the inputs and will create new ones if needed to fit the settings in the QTE
        /// </summary>
        private void InitializeInputs()
        {
            for (int i = 1; i <= inputDatas.Count; ++i)
            {
                var qte = elements.Find("QTE " + i);

                if (qte)
                {
                    var input = qte.Find("Input").GetComponent<Image>();

                    SetInputSprite(input, i);
                }
                else
                {
                    var parent = transform.Find("Elements");

                    var newPlus = Instantiate(plusObj, parent);
                    newPlus.gameObject.name = "+";

                    var newQTE = Instantiate(qteObj, parent);

                    newQTE.name = "QTE " + i;

                    var newInput = newQTE.Find("Input").GetComponent<Image>();

                    SetInputSprite(newInput, i);
                }
            }
        }

        /// <summary>
        /// Used to set up the correct sprite for each input provided
        /// </summary>
        /// <param name="input">What image should be adjusted</param>
        /// <param name="i">What index should be used</param>
        private void SetInputSprite(Image input, int i)
        {
            switch (inputMode)
            {
                case InputMode.Controller:
                    input.sprite = inputDatas[i - 1].controllerSprite;
                    break;
                case InputMode.Keyboard:
                    input.sprite = inputDatas[i - 1].keyboardSprite;
                    break;
                case InputMode.Mobile:
                    input.sprite = inputDatas[i - 1].mobileSprite;
                    break;
            }

            inputs.Add(input);
        }

        protected override IEnumerator QTEAction()
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

        public override void QTESuccess()
        {
            state = QTEState.Success;

            foreach (var input in inputs)
            {
                if(input != null)
                {
                    input.sprite = successSprite;
                }
                
            }

            CleanUp();

            onSuccess.Invoke();
        }

        public override void QTEFailure()
        {
            state = QTEState.Failed;

            foreach (var input in inputs)
            {
                if (input != null)
                {
                    input.sprite = failureSprite;
                }
            }

            CleanUp();

            onFailure.Invoke();
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            StopAllCoroutines();

            iTween.Stop(gameObject);

            OnCleanUpEvent();

            iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 0, "delay", 0.5f, "time", 0.25f, "onupdate", "UpdateMultipressAlpha"));

        }

        public void UpdateMultipressAlpha(float val)
        {
            foreach (var input in inputs)
            {
                if (input != null)
                {
                    UpdateAlpha(input, val);
                }
            }

            foreach (Transform child in elements)
            {
                if (child.name.Contains("+"))
                {
                    UpdateAlpha(child.GetComponent<Image>(), val);
                }
            }
        }

        public void UpdateAlpha(Image image, float val)
        {
            if(image != null)
            {
                var newColor = image.color;
                newColor.a = val;
                image.color = newColor;
            }
            
        }


        protected virtual void Update()
        {
            switch (state)
            {
                case QTEState.Active:
                    {
                        bool success = true;

                        // If that's not correct, try all the controller options
                        success = true;

                        foreach (var data in inputDatas)
                        {
                            if (!data.IsPressed())
                            {
                                success = false;
                            }
                        }

                        if (success)
                        {
                            // Check if any other keys are pressed
                            bool fail = inputDatas.Count > 0 && inputDatas[0].AnotherInputDown(inputDatas);

                            if (failIfIncorrect && fail) 
                            {
                                QTEFailure();
                            }
                            else
                            {
                                QTESuccess();
                            }

                            
                        }

                        break;
                    }
            }
        }
    }

}