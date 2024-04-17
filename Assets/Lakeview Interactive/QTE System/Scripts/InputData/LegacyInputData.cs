/******** Copyright (c) John P. Doran, All rights reserved. ******************
//
// File Name :  LegacyInputData.cs
// Author :     John P. Doran
//
// Purpose :    Utilizes the Input.GetButtonDown/GetAxis and Input.touches in
//              order to get input. Requires use of the 
//              Project Settings | Input menu.
//
 * *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//if (ENABLE_LEGACY_INPUT_MANAGER)

namespace QTESystem
{

    [CreateAssetMenu(fileName = "Input", menuName = "QTE System/LegacyInputData", order = 1)]
    public class LegacyInputData : InputData
    {
        [Tooltip("The name to be used when using the input function if using keyboard")]
        public string keyboardName;

        [Tooltip("The name to be used when using the input function from the controller.")]
        public string controllerName;


        /// <summary>
        /// If the controller stick is held down. Set to true initially in order to prevent multiple inputs hitting at once.
        /// </summary>
        private bool stickDownLast = true;


        internal override bool IsPressed()
        {
            if (QTE.inputMode == QTE.InputMode.Mobile)
            {
                GetTouchData();

                return currentMobileInput == mobileInput;
            }

            return (Input.GetButtonDown(keyboardName) || Input.GetButtonDown(controllerName) || Input.GetAxis(controllerName) > 0);
            }

        internal override bool IsDown()
        {
            if ((Input.GetAxis(controllerName) == 0) && (currentMobileInput == MobileInput.None))
            {
                stickDownLast = false;
            }

            bool isDown = (Input.GetButtonDown(keyboardName) ||
                           Input.GetButtonDown(controllerName) ||
                           (Input.GetAxis(controllerName) > 0) &&
                            !stickDownLast);

            if (QTE.inputMode == QTE.InputMode.Mobile && !stickDownLast)
            {
                GetTouchData();

                return currentMobileInput == mobileInput;
            }

            if (isDown)
            {
                stickDownLast = true;
            }
            else if (Input.GetAxis(controllerName) == 0)
            {
                stickDownLast = false;
            }

            return isDown;
        }

        internal override bool AnotherInputDown()
        {
            // Using the old input system, we can just check if they pressed any key
            if (!IsDown() && Input.anyKey)
            {
                return true;
            }

            LegacyInputData[] otherDatas = Resources.FindObjectsOfTypeAll<LegacyInputData>();

            foreach (var data in otherDatas)
            {
                // skip ourselves
                if(data == this)
                {
                    continue;
                }
                else if(data.IsDown())
                {
                    return true;
                }
            }

            return false;
        }

        internal override bool AnotherInputDown(List<InputData> exceptions)
        {
            LegacyInputData[] allInputData = Resources.FindObjectsOfTypeAll<LegacyInputData>();
            List<LegacyInputData> inputDataList = new List<LegacyInputData>(allInputData);

            // Remove the given data from the total list
            var result = inputDataList.Except(exceptions).ToList();

            foreach (var data in result)
            {
                // skip ourselves
                if (data == this)
                {
                    continue;
                }
                else if (data.IsDown())
                {
                    return true;
                }
            }

            return false;
        }

        #region Mobile

        private static Vector2 fingerDownPosition;
        private MobileInput currentMobileInput;
        private static Vector2 fingerUpPosition;

        private static float minSwipeDistance = 20f;

        private void GetTouchData()
        {
            if (Input.touchCount == 0)
            {
                currentMobileInput = MobileInput.None;
            }

            foreach (Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case UnityEngine.TouchPhase.Began:
                        fingerUpPosition = touch.position;
                        fingerDownPosition = touch.position;
                        currentMobileInput = MobileInput.TapScreen;
                        break;
                    case UnityEngine.TouchPhase.Moved:
                        fingerDownPosition = touch.position;
                        DetectSwipe();
                        break;
                    case UnityEngine.TouchPhase.Ended:
                        fingerDownPosition = touch.position;
                        DetectSwipe();
                        break;
                }
            }
        }

        private void DetectSwipe()
        {
            if (SwipeDistanceCheckMet())
            {
                MobileInput direction;

                if (IsVerticalSwipe())
                {
                    direction = fingerDownPosition.y - fingerUpPosition.y > 0 ? MobileInput.SwipeUp : MobileInput.SwipeDown;
                }
                else
                {
                    direction = fingerDownPosition.x - fingerUpPosition.x > 0 ? MobileInput.SwipeRight : MobileInput.SwipeLeft;
                }

                currentMobileInput = direction;

                fingerUpPosition = fingerDownPosition;
            }
        }

        private bool IsVerticalSwipe()
        {
            return VerticalMovementDistance() > HorizontalMovementDistance();
        }

        private bool SwipeDistanceCheckMet()
        {
            return VerticalMovementDistance() > minSwipeDistance || HorizontalMovementDistance() > minSwipeDistance;
        }

        private float VerticalMovementDistance()
        {
            return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
        }

        private float HorizontalMovementDistance()
        {
            return Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
        }


#endregion


    }
}

