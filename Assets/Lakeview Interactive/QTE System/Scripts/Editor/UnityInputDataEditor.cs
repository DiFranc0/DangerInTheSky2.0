#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System;

#if (ENABLE_INPUT_SYSTEM)
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace QTESystem
{
    [CustomEditor(typeof(UnityInputData))]
    public class UnityInputDataEditor : InputDataEditor
    {

        ReorderableList controllerButtons;
        ReorderableList keyboardButtons;

        SerializedProperty stickButton;

        private Color editColor = new Color(232/255f, 65/255f, 24/255f, .25f);
        private Color defaultColor = Color.white;

        public override void OnEnable()
        {
            base.OnEnable();

            keyboardButtons = new ReorderableList(serializedObject, serializedObject.FindProperty("keyboardButtons"), true, true, true, true);

            keyboardButtons.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = keyboardButtons.serializedProperty.GetArrayElementAtIndex(index);

                    DrawElement(rect, element);


                };

            keyboardButtons.drawHeaderCallback = (Rect rect) => {
                EditorGUI.LabelField(rect, "Keyboard Buttons");
            };

            controllerButtons = new ReorderableList(serializedObject, serializedObject.FindProperty("controllerButtons"), true, true, true, true);

            controllerButtons.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = controllerButtons.serializedProperty.GetArrayElementAtIndex(index);

                    DrawElement(rect, element);
                };

            controllerButtons.drawHeaderCallback = (Rect rect) => {
                EditorGUI.LabelField(rect, "Controller Buttons");
            };

            stickButton = serializedObject.FindProperty("stickInput");


        }

        private void DrawElement(Rect rect, SerializedProperty element)
        {
            GUI.color = (editMode == EditMode.None) ? defaultColor : editColor;

            EditorGUI.PropertyField(rect, element, GUIContent.none);

            GUI.color = defaultColor;
        }

        public override void OnInspectorGUI()
        {
            // Always needs to be done at the top of OnInspectorGUI
            serializedObject.Update();

            var inputData = (UnityInputData)target;

            EditorGUILayout.Space();

#if (!ENABLE_INPUT_SYSTEM)
            HorizontalLine();
            EditorGUILayout.HelpBox("You do not currently have the new Unity Input System enabled. You should either enable it or make use of the Legacy Input Data options.", MessageType.Warning);
            EditorGUILayout.Space();
#endif
            HorizontalLine();

            EditorGUILayout.BeginHorizontal();
            if (editMode != EditMode.Add)
            {
                if (GUILayout.Button("Add Options", new GUILayoutOption[] {}))
                {
                    editMode = EditMode.Add;
                }
            }
            else
            {
                if (GUILayout.Button("Cancel"))
                {
                    editMode = EditMode.None;
                }
            }

            EditorGUILayout.Space();

            if (editMode != EditMode.Remove)
            {
                if (GUILayout.Button("Remove Options"))
                {
                    editMode = EditMode.Remove;
                }
            }
            else
            {
                if (GUILayout.Button("Cancel"))
                {
                    editMode = EditMode.None;
                }
            }

            if (editMode != EditMode.None)
            {
                ModifyKeyboardOptions();
                ModifyControllerOptions();
            }

            EditorGUILayout.EndHorizontal();

            HorizontalLine();

            DrawKeyboardOptions(inputData);

            HorizontalLine();

            DrawControllerOptions(inputData);

            HorizontalLine();

            DrawMobileOptions(inputData);

            //ShowInputModeProperties(inputData);

            EditorUtility.SetDirty(target);

            serializedObject.ApplyModifiedProperties();
        }

        public enum EditMode
        {
            None,
            Add,
            Remove
        }

        EditMode editMode = EditMode.None;

        protected override void DrawControllerOptions(UnityEngine.Object target)
        {
            base.DrawControllerOptions(target);

            controllerButtons.DoLayoutList();

            EditorGUILayout.LabelField("Controller Stick");
            DrawElement(stickButton);

            EditorGUILayout.Space();
        }

        private void DrawElement(SerializedProperty stickButton)
        {
            GUI.color = (editMode == EditMode.None) ? defaultColor : editColor;
            EditorGUILayout.PropertyField(stickButton, GUIContent.none);
            GUI.color = defaultColor;
        }

        private void ModifyControllerOptions()
        {
            var gamepad = Gamepad.current;

            var inputData = (UnityInputData)target;

            foreach (GamepadButton button in Enum.GetValues(typeof(GamepadButton)))
            {
                if ((gamepad[button]).isPressed)
                {
                    if(!inputData.controllerButtons.Contains(button))
                    {
                        if(editMode == EditMode.Add)
                        {
                            inputData.controllerButtons.Add(button);
                        }
                        
                    }
                    else
                    {
                        if (editMode == EditMode.Remove)
                        {
                            inputData.controllerButtons.Remove(button);
                        }
                    }
                    return;
                }
            }

            foreach(StickInput s in Enum.GetValues(typeof(StickInput)))
            {
                ButtonControl stickInput = inputData.GetButtonControl(s);

                if (stickInput != null && stickInput.isPressed)
                {
                    if (editMode == EditMode.Add)
                    {
                        inputData.stickInput = s;
                    }
                    else if (editMode == EditMode.Remove)
                    {
                        inputData.stickInput = StickInput.NoStickInput;
                    }
                }
            }


        }

        private void ModifyKeyboardOptions()
        {
            var keyboard = Keyboard.current;
            foreach (Key key in Enum.GetValues(typeof(Key)))
            {
                if(key == Key.None || key == Key.IMESelected)
                {
                    continue;
                }

                if (((KeyControl)keyboard[key]).isPressed)
                {
                    var inputData = (UnityInputData)target;

                    if (!inputData.keyboardButtons.Contains(key))
                    {
                        if (editMode == EditMode.Add)
                        {
                            inputData.keyboardButtons.Add(key);
                        }
                    }
                    else
                    {
                        if (editMode == EditMode.Remove)
                        {
                            inputData.keyboardButtons.Remove(key);
                        }
                    }
                    return;
                }
            }
        }

        protected override void DrawKeyboardOptions(UnityEngine.Object target)
        {
            base.DrawKeyboardOptions(target);

            var inputData = (UnityInputData)target;

            keyboardButtons.DoLayoutList();

            EditorGUILayout.Space();
        }
    }


}

#endif

#endif