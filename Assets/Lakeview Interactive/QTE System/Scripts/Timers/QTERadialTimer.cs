/******** Copyright (c) John P. Doran, All rights reserved. ******************
//
// File Name :  QTERadialTimer.cs
// Author :     John P. Doran
//
// Purpose :    A timer that fills a circle to display how much time the 
//              player has to complete a QTE
//
*****************************************************************************/
using UnityEngine.UI;

namespace QTESystem
{
    public class QTERadialTimer : QTETimer
    {
        /// <summary>
        /// The image to be used to show the time left
        /// </summary>
        protected Image timer;

        protected override void OnEnable()
        {
            base.OnEnable();

            timer = GetComponent<Image>();
            timer.color = startColor;
            timer.fillAmount = 1;
        }

        public override void StartTimer(float time)
        {
            if (timer != null)
            {
                iTween.ValueTo(this.gameObject, iTween.Hash("from", 1, "to", 0, "time", time, "onupdate", "UpdateTimer"));
            }
        }

        /// <summary>
        /// Will fill the time to the value given with a color reflecting the time left
        /// </summary>
        /// <param name="val">The percentage of time left on the timer from 0-1</param>
        public void UpdateTimer(float val)
        {
            timer.fillAmount = val;
            timer.color = ((1f - val) * endColor) + (val * startColor);
        }

        protected override void StopTimer()
        {
            base.StopTimer();

            if (timer != null)
            {
                iTween.ValueTo(this.gameObject, iTween.Hash("from", timer.color.a, "to", 0, "time", 0.25f, "onupdate", "UpdateAlpha"));
            }
        }

        /// <summary>
        /// Will update the alpha of this object, used for fading in and out
        /// </summary>
        /// <param name="val">The current alpha value the object should have</param>
        public void UpdateAlpha(float val)
        {
            if(timer != null)
            {
                var newColor = timer.color;
                newColor.a = val;
                timer.color = newColor;
            }
            
        }

        public override void Reset()
        {
            base.Reset();

            iTween.Stop(gameObject);

            UpdateAlpha(1.0f);
            
        }



    }
}

