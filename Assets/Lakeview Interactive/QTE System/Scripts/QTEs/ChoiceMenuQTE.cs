using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QTESystem
{
    public class ChoiceMenuQTE : QTE
    {
        /// <summary>
        /// The group that holds all of the QTE choices
        /// </summary>
        private CanvasGroup group;

        private ChoiceButton[] choices;

        [Tooltip("What choice (if any) should be selected by default?")]
        public ChoiceButton firstChoice;

        public override void BeginQTE()
        {
            base.BeginQTE();

            if (firstChoice)
            {
                EventSystem.current.SetSelectedGameObject(firstChoice.gameObject);
            }
        }

        protected override void OnEnable()
        {
            group = GetComponentInParent<CanvasGroup>();
            choices = GetComponentsInChildren<ChoiceButton>();

            foreach (var choice in choices)
            {
                choice.interactable = true;
            }

            if(firstChoice)
            {
                EventSystem.current.SetSelectedGameObject(firstChoice.gameObject);
            }

            base.OnEnable();
        }

        public override void UpdateInputAlpha(float val)
        {
            base.UpdateInputAlpha(val);

            UpdateChoiceAlpha(val);
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            foreach (var choice in choices)
            {
                choice.interactable = false;
            }

            // Hide all of the QTE Choices
            if (group != null)
            {
                iTween.ValueTo(this.gameObject, iTween.Hash("from", group.alpha, "to", 0, "delay", 0.5f, "time", 0.25f, "onupdate", "UpdateChoiceAlpha"));
            }
        }

        public virtual void UpdateChoiceAlpha(float val)
        {
            if (group != null)
            {
                group.alpha = val;
            }
        }
    }
}
