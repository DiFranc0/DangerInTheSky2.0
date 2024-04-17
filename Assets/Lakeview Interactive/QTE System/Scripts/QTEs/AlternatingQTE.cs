using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace QTESystem
{
    public class AlternatingQTE : QTE
    {

        [Header("Alternating QTE Settings")]

        [Tooltip("The information used by the QTE in order to determine what input sprite to show and what button/key to press in order to succeed")]
        public InputData inputData1;

        [Tooltip("The information used by the QTE in order to determine what input sprite to show and what button/key to press in order to succeed")]
        public InputData inputData2;

        [Tooltip("Assuming no lost progress, how many times would the player need to press the input in order to trigger a success.")]
        public int timesToHit = 5;

        /// <summary>
        /// Displays what key should be pressed
        /// </summary>
        protected Image input2;

        /// <summary>
        /// How much progress has the player made so far on the QTE
        /// </summary>
        private float progress = 0;

        [Tooltip("Should the player lose progress over time?")]
        public bool loseProgress = true;

        [Tooltip("The rate at which the player loses progress, the higher the number the harder the QTE")]
        public float lossRate = 1;

        [Tooltip("If enabled, will have the input button animate to simulate button mashing")]
        public bool animateInput = true;

        /// <summary>
        /// What object holds progress
        /// </summary>
        private Image progressObj;

        /// <summary>
        /// The overlay to be placed over the progress bar
        /// </summary>
        private Image overlay;

        /// <summary>
        /// Store the original scale of the object to ensure it doesn't grow/shrink over time
        /// </summary>
        Vector2 originalScale;

        private void Awake()
        {
            input = transform.Find("Input").GetComponent<Image>();
            originalScale = input.rectTransform.sizeDelta;
        }

        protected override void CleanUp()
        {
            iTween.Stop(gameObject);

            if (progressObj)
            {
                progressObj.gameObject.SetActive(false);
            }

            if(overlay)
            {
                overlay.gameObject.SetActive(false);
            }

            previousInput = null;

            base.CleanUp();
        }

        /// <summary>
        /// Used by iTween in order to animate the input button
        /// </summary>
        /// <param name="val"></param>
        void UpdateScale(float val)
        {
            input.rectTransform.sizeDelta = originalScale * val;

            // Do the opposite on the other side
            input2.rectTransform.sizeDelta = originalScale * (1 + (1-val));
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            input2 = transform.Find("Input 2").GetComponent<Image>();

            switch (inputMode)
            {
                case InputMode.Controller:
                    input.sprite = inputData1.controllerSprite;
                    input2.sprite = inputData2.controllerSprite;
                    break;
                case InputMode.Keyboard:
                    input.sprite = inputData1.keyboardSprite;
                    input2.sprite = inputData2.keyboardSprite;
                    break;
                case InputMode.Mobile:
                    input.sprite = inputData1.mobileSprite;
                    input2.sprite = inputData2.mobileSprite;
                    break;
            }

            overlay = transform.Find("Overlay").GetComponent<Image>();

            // Set up the progress object
            progressObj = transform.Find("Progress").GetComponent<Image>();
            progressObj.fillAmount = 0;

            progress = 0;

            progressObj.gameObject.SetActive(true);

            if (overlay)
            {
                overlay.gameObject.SetActive(true);
            }


            // If should animate start animating
            if (animateInput)
            {
                iTween.ValueTo(this.gameObject, iTween.Hash("from", 1f, "to", 1.1f, "time", 0.1f, "onupdate", "UpdateScale", "looptype", iTween.LoopType.pingPong));
            }
        }

        protected void Update()
        {
            switch (state)
            {
                case QTEState.Active:
                    {
                        // If pressed, increase progress
                        if (GetDown())
                        {
                            progress += (1.0f / timesToHit);

                            if (progress >= 1)
                            {
                                QTESuccess();
                            }

                        }
                        // If not, lose progress
                        else if (loseProgress)
                        {
                            progress -= Time.deltaTime * lossRate;
                        }

                        // Ensure the progress bar is within valid parameters
                        progress = Mathf.Clamp(progress, 0, 1);

                        // Adjust the progress bar
                        progressObj.fillAmount = progress;

                        break;
                    }
            }
        }

        InputData previousInput;

        private bool GetDown()
        {
            if(previousInput == null)
            {
                if (inputData1.IsDown())
                {
                    previousInput = inputData1;
                    return true;
                }
                else if (inputData2.IsDown())
                {
                    previousInput = inputData2;
                    return true;
                }
            }
            else
            {
                if(previousInput == inputData1)
                {
                    if (inputData2.IsDown())
                    {
                        previousInput = inputData2;
                        return true;
                    }
                }
                else
                {
                    if (inputData1.IsDown())
                    {
                        previousInput = inputData1;
                        return true;
                    }
                }
            }

            return false;
        }

        public override void QTESuccess()
        {
            base.QTESuccess();

            input2.sprite = successSprite;
        }

        public override void QTEFailure()
        {
            base.QTEFailure();

            input2.sprite = failureSprite;
        }


        public override void UpdateInputAlpha(float val)
        {
            base.UpdateInputAlpha(val);

            if (input2 != null)
            {
                var newColor = input2.color;
                newColor.a = val;
                input2.color = newColor;
            }
        }
    
    }

}

