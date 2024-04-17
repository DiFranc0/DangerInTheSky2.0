/******** Copyright (c) John P. Doran, All rights reserved. ******************
//
// File Name :  MashingQTE.cs
// Author :     John P. Doran
//
// Purpose :    This type of QTE requires the user to press the specific 
//              button a certain number of times.
//
*****************************************************************************/
using UnityEngine;
using UnityEngine.UI;

namespace QTESystem
{
    public class MashingQTE : SimpleQTE
    {
        [Header("Mashing QTE Settings")]
        [Tooltip("Assuming no lost progress, how many times would the player need to press the input in order to trigger a success.")]
        public int timesToHit = 5;

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
        /// The min and max size of the progress fill
        /// </summary>
        private float minSize = 100;
        private float maxSize = 150;

        /// <summary>
        /// Store the original scale of the object to ensure it doesn't grow/shrink over time
        /// </summary>
        Vector2 originalScale;

        private void Awake()
        {
            input = transform.Find("Input").GetComponent<Image>();
            originalScale = input.rectTransform.sizeDelta;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            // Set up the progress object
            progressObj = transform.Find("Progress").GetComponent<Image>();
            progressObj.rectTransform.sizeDelta = Vector2.one * (minSize);

            progress = 0;

            progressObj.gameObject.SetActive(true);

            
            // If should animate start animating
            if (animateInput)
            {
                iTween.ValueTo(this.gameObject, iTween.Hash("from", 1f, "to", 1.1f, "time", 0.1f, "onupdate", "UpdateScale", "looptype", iTween.LoopType.pingPong));
            }
            
        }

        protected override void Update()
        {
            switch (state)
            {
                case QTEState.Active:
                    {
                        // If pressed, increase progress
                        if (inputData && inputData.IsDown())
                        {
                            progress += (1.0f / timesToHit);

                            if (progress >= 1)
                            {
                                QTESuccess();
                            }

                        }
                        else if (failIfIncorrect && inputData.AnotherInputDown())
                        {
                            QTEFailure();
                        }
                        // If not, lose progress
                        else if (loseProgress)
                        {
                            progress -= Time.deltaTime * lossRate;
                        }

                        // Ensure the progress bar is within valid parameters
                        progress = Mathf.Clamp(progress, 0, 1);

                        // Adjust the progress bar
                        progressObj.rectTransform.sizeDelta = Vector2.one * (minSize + ((maxSize - minSize) * progress));

                        // Uncomment to have the timer change color over time
                        //progressObj.color = Color.Lerp(endColor, startColor, progress);



                        break;
                    }
            }
        }

        protected override void CleanUp()
        {
            iTween.Stop(gameObject);

            if(progressObj)
            {
                progressObj.gameObject.SetActive(false);
            }

            base.CleanUp();
        }

        /// <summary>
        /// Used by iTween in order to animate the input button
        /// </summary>
        /// <param name="val"></param>
        void UpdateScale(float val)
        {
            input.rectTransform.sizeDelta = originalScale * val;
        }
    }

}