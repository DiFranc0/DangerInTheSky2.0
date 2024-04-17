using System.Collections;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace QTESystem
{
    [CustomEditor(typeof(InputData))]
    public class InputDataEditor : Editor
    {
        protected SerializedProperty mobileSprite;
        protected SerializedProperty mobileName;

        protected SerializedProperty controllerSprite;

        protected SerializedProperty keyboardSprite;
        public virtual void OnEnable()
        {
            mobileSprite = serializedObject.FindProperty("mobileSprite");
            mobileName = serializedObject.FindProperty("mobileInput");

            controllerSprite = serializedObject.FindProperty("controllerSprite");
            
            keyboardSprite = serializedObject.FindProperty("keyboardSprite");
        }

        public override void OnInspectorGUI()
        {
            // Always needs to be done at the top of OnInspectorGUI
            serializedObject.Update();

            var inputData = (InputData)target;

            EditorGUILayout.Space();

            DrawKeyboardOptions(inputData);

            DrawControllerOptions(inputData);

            DrawMobileOptions(inputData);

            //ShowInputModeProperties(inputData);

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawMobileOptions(UnityEngine.Object target)
        {
            var inputData = (InputData)target;

            EditorGUILayout.LabelField("Mobile Options", EditorStyles.boldLabel);
            
            EditorGUILayout.PropertyField(mobileSprite);

            EditorGUILayout.PropertyField(mobileName);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        protected virtual void DrawControllerOptions(UnityEngine.Object target)
        {
            EditorGUILayout.LabelField("Controller Options", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(controllerSprite);
        }

        protected virtual void DrawKeyboardOptions(UnityEngine.Object target)
        {
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Keyboard Options", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(keyboardSprite);

        }

        protected void ShowInputModeProperties(LegacyInputData inputData)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Controller Mode: ", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(QTE.inputMode.ToString(), EditorStyles.label);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            Texture texture = null;

            switch (QTE.inputMode)
            {
                case QTE.InputMode.Controller:
                    texture = inputData.controllerSprite.texture;
                    break;
                case QTE.InputMode.Keyboard:
                    texture = inputData.keyboardSprite.texture;
                    break;
                case QTE.InputMode.Mobile:
                    texture = inputData.mobileSprite.texture;
                    break;
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            GUILayout.Box(texture, new GUILayoutOption[] { GUILayout.Height(50), GUILayout.Width(50) });
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();

            HorizontalLine();

            EditorGUILayout.LabelField("Switch Input Mode:", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Keyboard"))
            {
                QTE.inputMode = QTE.InputMode.Keyboard;
            }
            if (GUILayout.Button("Controller"))
            {
                QTE.inputMode = QTE.InputMode.Controller;
            }
            if (GUILayout.Button("Mobile"))
            {
                QTE.inputMode = QTE.InputMode.Mobile;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        protected void HorizontalLine()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }


    }
}

#endif

