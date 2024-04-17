/******** Copyright (c) John P. Doran, All rights reserved. ******************
//
// File Name :  ClickableQTE.cs
// Author :     John P. Doran
//
// Purpose :    This form of QTE is used in order to utilize mouse, mobile, or
//              controller input in order to select it. Also optionally 
//              includes a visual line to show players the path to the target.
//
// Note:        The line property is not supported in world view.
//              Position of the cursor object in the Scene will dictate where 
//              the controller cursor will start.
//
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

#if (ENABLE_INPUT_SYSTEM)
using UnityEngine.InputSystem;
#endif

using UnityEngine.UI;

namespace QTESystem
{
    public class ClickableQTE : QTE
    {
        [Header("Clickable QTE Settings")]
        public Sprite targetSprite;

        [Tooltip("Should the scope object be displayed?")]
        public bool showScope = true;

        [Tooltip("The cursor that will be shown on the screen while the QTE" +
                 " is active")]
        public Sprite scope;

        [Header("Controller Settings")]

        [Tooltip("How many pixels per second the controller should be able" +
                 " to travel")]
        public float controllerMoveSpeed = 600;

        [Tooltip("Should the scope be moved to a random position before " +
                 "starting the QTE?")]
        public bool randomScopePos = true;

        [Tooltip("If randomControllerPos is enabled, how far the scope " +
                 "should be from the target")]
        public float distanceFromTarget = 200;

        [Header("Mouse Settings")]

        [Tooltip("Should the mouse cursor be displayed?")]
        public bool hideMouse = true;

        [Tooltip("Should the mouse be locked in place during the QTE. " +
                 "Locking the mouse position for the duration of the QTE " +
                 "will have the player use the scope's offset for movement.")]
        public bool lockMouse;

        [Tooltip("Should the scope's speed by limited when using mouse input?")]
        public bool clampMouseSpeed = false;

        [Header("Line Settings")]

        [Tooltip("Should the line be displayed or not? Note: Does not work " +
                 "correctly on Mobile or in World Space")]
        public bool showLine;

        /// <summary>
        /// A reference to the line object
        /// </summary>
        private Transform lineObj;

        /// <summary>
        /// A reference to the button that can be pressed
        /// </summary>
        private Button inputButton;

        /// <summary>
        /// Checking lock mode in order to use correct form of QTE
        /// </summary>
        private bool mouseLocked;

        /// <summary>
        /// Is the mouse visible by default, returns to this value after the QTE is over
        /// </summary>
        private bool defaultMouseVisibility;

        /// <summary>
        /// Is the cursor locked by default, returns to this value after the QTE is over
        /// </summary>
        private CursorLockMode defaultLockMode;

        /// <summary>
        /// A reference to the scope game object
        /// </summary>
        private Transform scopeObj;

        private void Awake()
        {
            // Get the current status of the cursor to revert back to later
            defaultMouseVisibility = Cursor.visible;
            defaultLockMode = Cursor.lockState;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

#if !UNITY_EDITOR
            if (Application.platform == RuntimePlatform.Android ||
                    Application.platform == RuntimePlatform.IPhonePlayer)
            {
                showScope = false;
                showLine = false;
            }
#endif

            // If not confined at all, keep on the screen until over
            if(Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Confined;
            }

            input.sprite = targetSprite;
            inputButton = input.GetComponent<Button>();

            // Enable scope/line if needed
            scopeObj = transform.Find("Scope");
            if (scopeObj)
            {
                scopeObj.gameObject.SetActive(showScope);
            }

            lineObj = transform.Find("Line");
            if (lineObj)
            {
                lineObj.gameObject.SetActive(showLine);
            }

            // Update lock state if needed
            if (lockMouse)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            // If the mouse is locked use controller mode
            mouseLocked = (Cursor.lockState == CursorLockMode.Locked);

            // If using controller mode, set random scope pos if needed
            if (((inputMode == InputMode.Controller) || mouseLocked) && randomScopePos)
            {
                var randomDirection = Random.insideUnitCircle.normalized * distanceFromTarget * transform.lossyScale;

                scopeObj.position = input.transform.position + new Vector3(randomDirection.x, randomDirection.y);
            }

            if (hideMouse)
            {
                UnityEngine.Cursor.visible = false;
            }
        }

        protected override IEnumerator QTEAction()
        {
            if(input != null)
            {
                // Has to use GetComponent because is called in parent OnEnable
                input.GetComponent<Button>().interactable = true;
            }

            return base.QTEAction();
        }

        protected override void CleanUp()
        {
            // Should no longer be interactable after the QTE is over
            if (inputButton)
            {
                inputButton.interactable = false;
            }
            

            // No longer need the scope to be displayed
            if (scopeObj)
            {
                scopeObj.gameObject.SetActive(false);
            }

            // Revert back to original cursor visibility
            UnityEngine.Cursor.visible = defaultMouseVisibility;
            Cursor.lockState = defaultLockMode;


            if (lineObj)
            {
                lineObj.gameObject.SetActive(false);
            }

            base.CleanUp();
        }

        protected void Update()
        {
            switch (state)
            {
                case QTEState.Active:
                    {
                        if (inputMode == InputMode.Controller || mouseLocked)
                        {
                            // Check for WASD/Controller movement
                            var cursorMovement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

                            // Utilize mouse movement as well
                            cursorMovement += new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);

                            // Move the cursor based on controller movement
                            scopeObj.position += cursorMovement * controllerMoveSpeed * Time.deltaTime;

                            // Data required by pointer events
                            var data = new PointerEventData(EventSystem.current);

                            // If the scope is close enough, select the button
                            if (Vector2.Distance(scopeObj.position, transform.position) < (input.rectTransform.rect.width / 2.0f) * input.transform.lossyScale.x)
                            {
                                inputButton.OnPointerEnter(data);
                                EventSystem.current.SetSelectedGameObject(input.gameObject);

                                // Check for mouse here, Submit is controlled by Unity's Input System
                                if (Input.GetMouseButton(0))
                                {
                                    QTESuccess();
                                }
                            }
                            else
                            {
                                // Otherwise turn it off and deselect the object
                                inputButton.OnPointerExit(data);
                                EventSystem.current.SetSelectedGameObject(null);
                            }

                        }
                        else
                        {
                            Vector3 cursorPos = new Vector3();

#if (ENABLE_INPUT_SYSTEM)
                            var pos = Mouse.current.position.ReadValue();
                            cursorPos.x = pos.x;
                            cursorPos.y = pos.y;
#else

                            cursorPos = Input.mousePosition;
                            cursorPos.z = 0;
#endif

                            // If clamping the mouse's speed, only move up to a certain amount
                            if (clampMouseSpeed)
                            {
                                scopeObj.position = Vector3.MoveTowards(scopeObj.position, cursorPos, controllerMoveSpeed * Time.deltaTime);
                            }
                            else
                            {
                                scopeObj.position = cursorPos;
                            }

                        }

                        break;
                    }
            }
        }
    }
}