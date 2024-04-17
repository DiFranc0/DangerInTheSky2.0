using System.Collections;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace QTESystem
{
    [CustomEditor(typeof(LegacyInputData))]
    public class LegacyInputDataEditor : InputDataEditor
    {
        public override void OnInspectorGUI()
        {
            // Always needs to be done at the top of OnInspectorGUI
            serializedObject.Update();

            var inputData = (LegacyInputData)target;

            EditorGUILayout.Space();


#if (ENABLE_INPUT_SYSTEM)
            HorizontalLine();

            EditorGUILayout.HelpBox("You currently have the new Unity Input System enabled. You likely want to be using the UnityInputData type instead of this legacy option.", MessageType.Warning);
            EditorGUILayout.Space();

#endif
            HorizontalLine();

            DrawKeyboardOptions(inputData);

            HorizontalLine();

            DrawControllerOptions(inputData);

            HorizontalLine();

            DrawMobileOptions(inputData);

            //ShowInputModeProperties(inputData);

            serializedObject.ApplyModifiedProperties();
        }


        protected override void DrawControllerOptions(UnityEngine.Object target)
        {
            base.DrawControllerOptions(target);

            var inputData = (LegacyInputData)target;

            var controllerName = serializedObject.FindProperty("controllerName");

            var valid = (!IsButtonAvailable(inputData.controllerName)) ? Color.red : Color.white;

            GUI.color = valid;

            EditorGUILayout.PropertyField(controllerName);

            EditorGUILayout.Space();

            GUI.color = Color.white;

            if (!IsButtonAvailable(inputData.controllerName))
            {
                EditorGUILayout.HelpBox("Current value for controllerName does not exist in the Input settings. Please go to Project Settings | Input and select one of the options avaliable.", MessageType.Error);
            }
        }

        protected override void DrawKeyboardOptions(UnityEngine.Object target)
        {
            base.DrawKeyboardOptions(target);

            var inputData = (LegacyInputData)target;

            var valid = (!IsButtonAvailable(inputData.keyboardName)) ? Color.red : Color.white;

            GUI.color = valid;

            var keyboardName = serializedObject.FindProperty("keyboardName");
            EditorGUILayout.PropertyField(keyboardName);

            GUI.color = Color.white;

            EditorGUILayout.Space();

            if (!IsButtonAvailable(inputData.keyboardName))
            {
                EditorGUILayout.HelpBox("Current value for keyboardName does not exist in the Input settings. Please go to Project Settings | Input and select one of the options avaliable.", MessageType.Error);
            }
        }

        protected static bool IsAxisAvailable(string axisName)
        {
            try
            {
                Input.GetAxis(axisName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected static bool IsButtonAvailable(string axisName)
        {
            try
            {
                Input.GetButton(axisName);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

        
}

#endif

