using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Doozy.Runtime.UIManager.Events;
using UnityEngine;
using Zenject;

namespace Gameloops.UI
{
    public class SettingsView : MonoBehaviour
    {
        [SerializeField] private UIView view;
        [Inject] private StorageManager _storage;
        [SerializeField] private UIToggle hapticToggle;
        [SerializeField] private UIToggle sfxToggle;
        [SerializeField] private UIToggle musicToggle;
        [SerializeField] private UIButton privacyButton;
        [SerializeField] private string privacyUrl = "https://www.gameloops.io/privacy-policy";


        private void Start()
        {
            Init();
        }

        private void OnEnable()
        {
            hapticToggle.onToggleValueChangedCallback += OnHapticValueChanged;
            sfxToggle.onToggleValueChangedCallback += OnSfxValueChanged;
            musicToggle.onToggleValueChangedCallback += OnMusicValueChanged;
            privacyButton.onClickEvent.AddListener(PrivacyClick);
            view.OnShowCallback.Event.AddListener(OnShow);

        }

        private void OnDisable()
        {
            hapticToggle.onToggleValueChangedCallback -= OnHapticValueChanged;
            sfxToggle.onToggleValueChangedCallback -= OnSfxValueChanged;
            musicToggle.onToggleValueChangedCallback -= OnMusicValueChanged;
            privacyButton.onClickEvent.RemoveListener(PrivacyClick);
            view.OnShowCallback.Event.RemoveListener(OnShow);
        }
        private void OnShow()
        {
            Init();
        }

        private void Init()
        {
            hapticToggle.isOn = _storage.IsHapticOn;
            sfxToggle.isOn = _storage.IsSfxOn;
            musicToggle.isOn = _storage.IsMusicOn;
        }

        private void PrivacyClick()
        {
            Application.OpenURL(privacyUrl);
        }

        private void OnHapticValueChanged(ToggleValueChangedEvent evt)
        {
            //Use evt.newValue
            _storage.IsHapticOn = evt.newValue;
        }
        
        private void OnSfxValueChanged(ToggleValueChangedEvent evt)
        {
            _storage.IsSfxOn = evt.newValue;
        }
        private void OnMusicValueChanged(ToggleValueChangedEvent evt)
        {
            _storage.IsMusicOn = evt.newValue;
        }
    }
}