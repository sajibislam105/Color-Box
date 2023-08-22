using Gameloops.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameloops.UI
{
    public class ResourcePanel : MonoBehaviour
    {
        [Inject] private UIUtils _utils;
        
        [SerializeField] private Image resourceImage;
        [SerializeField] private TMP_Text resourceText;
        private ResourceData _resourceData;
        public ResourceData ResourceData => _resourceData;
        public Transform Pivot => resourceImage.transform;


        public void InitResource(ResourceData resourceData)
        {
            _resourceData = resourceData;
            resourceImage.sprite = _resourceData.unitPreviewSprite;
        }

        public void SetResource(float amount)
        {
            var amountInt = Mathf.FloorToInt(amount);
            var resourceString = _utils.MakeKmb(amountInt);
            resourceText.text = resourceString;
        }
    }
}