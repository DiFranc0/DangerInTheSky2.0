/******** Copyright (c) John P. Doran, All rights reserved. ******************
//
// File Name :  UILineRenderer.cs
// Author :     John P. Doran
//
// Purpose :    Helper class used by the ClickableQTE in order to draw a
//              line between the cursor and target. Does not work correctly
//              in World Space.
//
*****************************************************************************/
using UnityEngine;
using UnityEngine.UI;

namespace QTESystem
{
    public class UILineRenderer : MonoBehaviour
    {
        [Header("Object References")]
        [Tooltip("Where the line should start at")]
        public RectTransform origin;

        [Tooltip("Where the line should be going to")]
        public RectTransform target;

        [Header("Line Properties")]
        [Tooltip("How wide the line should be")]
        public float lineWidth = 2;

        [Tooltip("What color should the line be when at the starting point")]
        public Color startColor = Color.red;

        [Tooltip("What color should the line be when at the ending point")]
        public Color endColor = Color.green;


        /// <summary>
        /// A reference to the image for coloring
        /// </summary>
        private Image image;

        /// <summary>
        /// How far we are from the objects at the start
        /// </summary>
        private float startingDistance;

        RectTransform rt;
        private void Start()
        {
            image = GetComponent<Image>();

            startingDistance = Vector3.Distance(target.position, origin.position);

            rt = GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {
            // See how far the target is
            Vector3 direction = target.position - origin.position;

            // Update line's position and width
            rt.sizeDelta = new Vector2(direction.magnitude, lineWidth);
            rt.pivot = new Vector2(0, 0.5f);
            rt.position = origin.position;

            // Rotate
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rt.rotation = Quaternion.Euler(0, 0, angle);


            // Update the color to show the distance left
            var currentDistance = Vector3.Distance(target.position, origin.position);
            image.color = Color.Lerp(startColor, endColor, ((startingDistance - currentDistance) / startingDistance));
        }
    }

}