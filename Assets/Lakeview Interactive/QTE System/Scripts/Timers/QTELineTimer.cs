/******** Copyright (c) John P. Doran, All rights reserved. ******************
//
// File Name :  QTELineTimer.cs
// Author :     John P. Doran
//
// Purpose :    A timer that moves horizontally to display how much time the 
//              player has to complete a QTE
//
*****************************************************************************/
using UnityEngine;
using UnityEngine.UI;

namespace QTESystem
{
    public class QTELineTimer : QTETimer
    {
        /// <summary>
        /// A reference to the slider used for the time display in this QTE
        /// </summary>
        private Slider slider;

        private CanvasGroup canvasGroup;

        protected override void OnEnable()
        {
            base.OnEnable();

            slider = GetComponent<Slider>();
            slider.value = 1;

            canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void StartTimer(float time)
        {
            if (slider != null)
            {
                iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 0, "time", time, "onupdate", "UpdateTimer"));
            }
        }

        /// <summary>
        /// Will fill the time to the value given with a color reflecting the time left
        /// </summary>
        /// <param name="val">The percentage of time left on the timer from 0-1</param>
        public void UpdateTimer(float val)
        {
            slider.value = val; 
        }

        protected override void StopTimer()
        {
            base.StopTimer();

            iTween.Stop(gameObject);

            if (canvasGroup != null)
            {
                iTween.ValueTo(this.gameObject, iTween.Hash("from", canvasGroup.alpha, "to", 0, "time", 0.25f, "onupdate", "UpdateAlpha"));
            }
        }

        /// <summary>
        /// Will update the alpha of this object, used for fading in and out
        /// </summary>
        /// <param name="val">The current alpha value the object should have</param>
        public void UpdateAlpha(float val)
        {
            if(canvasGroup != null)
            {
                canvasGroup.alpha = val;
            }
        }

        public override void Reset()
        {
            base.Reset();

            StopTimer();

            iTween.Stop(gameObject);

            UpdateAlpha(1.0f);
        }
    }
}

