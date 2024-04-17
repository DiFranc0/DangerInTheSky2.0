using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

namespace QTESystem
{
    public class ChoiceButton : Button
    {
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            SendSuccess();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            SendSuccess();

        }

        void SendSuccess()
        {
            // Check if there is a parent ChoiceMenuQTE
            var qte = gameObject.GetComponentInParent<ChoiceMenuQTE>();

            if (qte != null)
            {
                qte.QTESuccess();
            }
            else
            {
                Debug.LogWarning("Unable to find ChoiceMenuQTE to send success event to");
            }
        }

        public void SelectChoice()
        {
            // Make it so the Event System will display the option
            OnSelect(null);
        }
    }

}