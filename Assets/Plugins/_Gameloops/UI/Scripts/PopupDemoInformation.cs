using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Gameloops.UI
{
    public class PopupDemoInformation: MonoBehaviour
    {
        [SerializeField] private string popupName = "InformationPopup";
        [SerializeField] private string popupTitle = "Information Popup";
        [SerializeField] private string popupMessage = "This is a simple popup to show information to the user with an acknowledgement button";
        [SerializeField] private string popupButtonText = "Confirm";
        [SerializeField] private Sprite popupButtonIcon;

        public UnityEvent onPopupButtonClicked = new UnityEvent();

        [Button]
        public void ShowPopup()
        {
            var popup = UIPopup.Get(popupName);
            if (popup == null)
            {
                Debug.LogWarning("Popup not found!");
                return;
            }

            popup
                .SetTexts(popupTitle, popupMessage, popupButtonText)
                .SetSprites(popupButtonIcon)
                .SetEvents(onPopupButtonClicked)
                .ShowFromQueue();
        }
    }
}