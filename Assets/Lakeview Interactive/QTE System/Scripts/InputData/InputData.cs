/******** Copyright (c) John P. Doran, All rights reserved. ******************
//
// File Name :  InputData.cs
// Author :     John P. Doran
//
// Purpose :    Contains the base data used by the InputData class. Can
//              be extended from to support whatever input system you would like
//
 * *****************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace QTESystem
{
    [Serializable]
    public enum MobileInput
    {
        None,
        TapScreen,
        SwipeUp,
        SwipeDown,
        SwipeLeft,
        SwipeRight
    }

    public abstract class InputData : ScriptableObject
    {

        [Tooltip("What sprite should be displayed on the screen when the QTE is shown using keyboard")]
        public Sprite keyboardSprite;

        [Tooltip("If the player is using a controller, What sprite should be displayed on the screen when the QTE is shown")]
        public Sprite controllerSprite;


        [Tooltip("If on mobile what input should be used")]
        public MobileInput mobileInput;

        [Tooltip("If the player is using a mobile device, what sprite should be displayed on the screen when the QTE is shown")]
        public Sprite mobileSprite;

        /// <summary>
        /// Will check all of the allowed inputs and return if any of the inputs are pressed or not
        /// </summary>
        /// <returns>If any of the inputs are pressed</returns>
        internal abstract bool IsPressed();

        /// <summary>
        /// Will check all of the allowed inputs and return if any of the inputs are down or not
        /// </summary>
        /// <returns>If any of the inputs are down</returns>
        internal abstract bool IsDown();

        /// <summary>
        /// Will check all of the other allowed inputs and return if any of them inputs are down or not
        /// </summary>
        /// <returns>If any input other than the given one is down</returns>
        internal abstract bool AnotherInputDown();

        /// <summary>
        /// Will check the inputs other than the ones in the list for if they've been pressed
        /// </summary>
        /// <param name="data">The input that is correct</param>
        /// <returns>If they've been pressed</returns>
        internal abstract bool AnotherInputDown(List<InputData> data);

    }
}

