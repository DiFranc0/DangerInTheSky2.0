/******** Copyright (c) John P. Doran, All rights reserved. ******************
//
// File Name :  UnityInputData.cs
// Author :     John P. Doran
//
// Purpose :    Utilizes the new Unity Input System in order to register input.
//              Requires the Input System package to be installed from the
//              Package Manager (Window | Package Manager)
//
 * *****************************************************************************/
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if (ENABLE_INPUT_SYSTEM)
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace QTESystem
{
    public enum StickInput
    {
        NoStickInput,
        LeftStickUp,
        LeftStickDown,
        LeftStickLeft,
        LeftStickRight,
        RightStickUp,
        RightStickDown,
        RightStickLeft,
        RightStickRight
    }

    [CreateAssetMenu(fileName = "Input", menuName = "QTE System/UnityInputData", order = 1)]
    public class UnityInputData : InputData
    {
        [Tooltip("What options are valid when using the keyboard.")]
        public List<Key> keyboardButtons = new List<Key>();

        [Tooltip("What options are valid when using the controller.")]
        public List<GamepadButton> controllerButtons = new List<GamepadButton>();

        [Tooltip("Optional stick direction to be valid when using the controller.")]
        public StickInput stickInput;

        internal override bool IsPressed()
        {
            var gamepad = Gamepad.current;

            if (gamepad != null)
            {
                // Check all of the controller buttons
                foreach(var button in controllerButtons)
                {
                    if ((gamepad[button]).isPressed)
                        return true;
                }

                // Check sticks
                ButtonControl stickInput = GetButtonControl(this.stickInput);

                if (stickInput != null && stickInput.isPressed)
                    return true;
            }

            var keyboard = Keyboard.current;

            if (keyboard != null)
            {
                // Check each of the valid keyboard keys
                foreach(var button in keyboardButtons)
                {
                    if (((KeyControl)keyboard[button]).isPressed)
                    {
                        return true;
                    }
                }
                
            }

            // Check for mobile if on a mobile device
            if (QTE.inputMode == QTE.InputMode.Mobile)
            {
                GetTouchData();

                return currentMobileInput == mobileInput;
            }

            return false;
           
        }


        /// <summary>
        /// Will check all of the valid inputs for the item to see if any are down or not
        /// </summary>
        /// <returns>Will return true if something is down, false if not</returns>
        internal override bool IsDown()
        {
            var gamepad = Gamepad.current;

            if (gamepad != null)
            {
                foreach (var button in controllerButtons)
                {
                    if ((gamepad[button]).wasPressedThisFrame)
                        return true;
                }

                // Check sticks
                ButtonControl stickInput = GetButtonControl(this.stickInput);

                if (stickInput != null && stickInput.wasPressedThisFrame)
                    return true;
            }

            var keyboard = Keyboard.current;

            if (keyboard != null)
            {
                foreach (var button in keyboardButtons)
                {
                    if (((KeyControl)keyboard[button]).wasPressedThisFrame)
                    {
                        return true;
                    }
                }
            }

            if (QTE.inputMode == QTE.InputMode.Mobile)
            {
                GetTouchData();

                return currentMobileInput == mobileInput;
            }

            return false;
        }

        internal override bool AnotherInputDown()
        {
            // Will check all of the keyboard keys
            if (!IsDown() && Keyboard.current.anyKey.wasPressedThisFrame)
            {
                return true;
            }

            // Will check all of the options given by the InputDatas
            UnityInputData[] otherDatas = Resources.FindObjectsOfTypeAll<UnityInputData>();

            foreach (var data in otherDatas)
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

        internal override bool AnotherInputDown(List<InputData> exceptions)
        {
            UnityInputData[] allInputData = Resources.FindObjectsOfTypeAll<UnityInputData>();
            List<UnityInputData> inputDataList = new List<UnityInputData>(allInputData);

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


        /// <summary>
        /// Converts the StickInput enum into the ButtonControl type that can give information
        /// on the current state of the button
        /// </summary>
        /// <param name="stickInput">What input we would like to get the button control for</param>
        /// <returns>The button control relevant to the StickInput provided</returns>
        public ButtonControl GetButtonControl(StickInput stickInput)
        {
            var gamepad = Gamepad.current;

            ButtonControl buttonControl = null;

            switch (stickInput)
            {
                // Left stick
                case StickInput.LeftStickUp:
                    buttonControl = gamepad.leftStick.up;
                    break;
                case StickInput.LeftStickDown:
                    buttonControl = gamepad.leftStick.down;
                    break;
                case StickInput.LeftStickLeft:
                    buttonControl = gamepad.leftStick.left;
                    break;
                case StickInput.LeftStickRight:
                    buttonControl = gamepad.leftStick.right;
                    break;

                // Right stick
                case StickInput.RightStickUp:
                    buttonControl = gamepad.rightStick.up;
                    break;
                case StickInput.RightStickDown:
                    buttonControl = gamepad.rightStick.down;
                    break;
                case StickInput.RightStickLeft:
                    buttonControl = gamepad.rightStick.left;
                    break;
                case StickInput.RightStickRight:
                    buttonControl = gamepad.rightStick.right;
                    break;
            }

            return buttonControl;
        }

        #region Mobile


        private MobileInput currentMobileInput;

        private static Vector2 fingerDownPosition;
        private static Vector2 fingerUpPosition;
        
        // Minimum number of pixels to move before detecting a swipe, the larger
        // this is the harder it will be to swipe
        private static float minSwipeDistance = 20f;

        /// <summary>
        /// Checks all of the touches to detect if any swipes are done (if any)
        /// </summary>
        private void GetTouchData()
        {
            if (Input.touchCount == 0)
            {
                currentMobileInput = MobileInput.None;
            }

            var touchScreen = Touchscreen.current;

            if(touchScreen != null)
            {
                foreach (var touch in touchScreen.touches)
                {
                    fingerUpPosition = touch.startPosition.ReadValue();
                    fingerDownPosition = touch.position.ReadValue();;
                    DetectSwipe();
                }
            }

            
        }

        /// <summary>
        /// Will detect if a swipe is happening or not
        /// </summary>
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
#endif