/******** Copyright (c) John P. Doran, All rights reserved. ******************
//
// File Name :  SlidingQTE.cs
// Author :     John P. Doran
//
// Purpose :    A form of QTE which moves back and forth along a path and
//              succeeds if it stops at a particular point.
//
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace QTESystem
{
    public class SlidingQTE : QTE
    {
        [Header("Sliding QTE Settings")]
        public int numberOfTries = 5;

        [Tooltip("Should the icon turn green when it would be successful?")]
        public bool showSuccess = true;

        public InputData inputData;

        /// <summary>
        /// What is the minimumPosition that would be considered a success. Set in the OnEnable function.
        /// </summary>
        private float minSuccessX = -25f;

        /// <summary>
        /// What is the maximumPosiiton that would be considered a success. Set in the OnEnable function.
        /// </summary>
        private float maxSuccessX = 25f;

        /// <summary>
        /// A reference to the failure game object.
        /// </summary>
        RectTransform failure;

        /// <summary>
        /// A reference to the success game object
        /// </summary>
        Image success;

        protected override void OnEnable()
        {
            state = QTEState.Inactive;

            input = transform.Find("Input").GetComponent<Image>();

            switch (inputMode)
            {
                case InputMode.Controller:
                    input.sprite = inputData.controllerSprite;
                    break;
                case InputMode.Keyboard:
                    input.sprite = inputData.keyboardSprite;
                    break;
                case InputMode.Mobile:
                    input.sprite = inputData.mobileSprite;
                    break;
            }

            //Update the input to be at the start of the failure object
            failure = transform.Find("Failure").GetComponent<RectTransform>();
            input.GetComponent<RectTransform>().localPosition = failure.localPosition - new Vector3(failure.rect.width * failure.localScale.x / 2.0f, 0, 0);

            success = transform.Find("Success").GetComponent<Image>();

            minSuccessX = success.rectTransform.anchoredPosition3D.x - success.rectTransform.rect.width * failure.localScale.x / 2.0f;

            maxSuccessX = success.rectTransform.anchoredPosition3D.x + success.rectTransform.rect.width * failure.localScale.x / 2.0f;

            if (startOnAwake)
            {
                BeginQTE();
            }
        }

        protected override IEnumerator QTEAction()
        {
            state = QTEState.Active;

            yield return new WaitForSeconds(delay);

            // Need to account for fact someone could succeed before starting this
            if (state == QTEState.Active)
            {
                var origin = input.rectTransform.anchoredPosition;

                var destination = failure.anchoredPosition;
                destination.x += (failure.rect.width/2);

                // Move the target back and forth
                iTween.ValueTo(input.gameObject, iTween.Hash("from", origin, 
                                                             "to", destination, 
                                                             "time", time / numberOfTries, 
                                                             "looptype", iTween.LoopType.pingPong, 
                                                             "onupdate", "MoveInput", 
                                                             "onupdatetarget", gameObject));

                OnStartEvent();

                yield return new WaitForSeconds(time);

                QTEFailure();
            }

        }

        public void MoveInput(Vector2 position)
        {
            if (input == null)
            {
                return;
            }

            input.rectTransform.anchoredPosition = position;
        }

        public void UpdateAlpha(Image image, float val)
        {
            if(image == null)
            {
                return;
            }

            var newColor = image.color;
            newColor.a = val;
            image.color = newColor;
        }

        public void UpdateSlidingAlpha(float val)
        {
            UpdateAlpha(success, val);
            UpdateAlpha(failure.GetComponent<Image>(), val);
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            if(input != null)
            {
                input.color = Color.white;
                iTween.Stop(input.gameObject);
            }

            iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 0, "delay", 0.5f, "time", 0.25f, "onupdate", "UpdateSlidingAlpha"));
        }

        public override void Reset()
        {
            base.Reset();

            OnResetEvent();

            iTween.Stop(gameObject);

            if(input != null)
            {
                iTween.Stop(input.gameObject);
            }
            

            UpdateAlpha(success, 1.0f);

            if(failure != null)
            {
                UpdateAlpha(failure.GetComponent<Image>(), 1.0f);
            }
            
        }

        protected virtual void Update()
        {
            switch (state)
            {
                case QTEState.Active:
                    {
                        var xPos = input.rectTransform.anchoredPosition3D.x;

                        if (showSuccess && InsideSuccess(xPos))
                        {
                            input.color = new Color32(7, 209, 124, 255);
                        }
                        else
                        {
                            input.color = Color.white;
                        }

                        if (inputData.IsDown())
                        {
                            // Check if inside of the success field
                            if (InsideSuccess(xPos))
                            {
                                QTESuccess();
                            }
                            else
                            {
                                QTEFailure();
                            }
                        }

                        break;
                    }
            }
        }

        /// <summary>
        /// Used to determine if the target fit into the success area
        /// </summary>
        /// <param name="xPos">The current x position of the target</param>
        /// <returns>If the item is inside of the success range or not</returns>
        private bool InsideSuccess(float xPos)
        {
            return (xPos >= minSuccessX && xPos <= maxSuccessX);
        }
    }
}