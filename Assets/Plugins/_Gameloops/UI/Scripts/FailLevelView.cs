using DG.Tweening;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameloops.UI
{
    public class FailLevelView : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private StorageManager _storageManager;
        [SerializeField] private TMP_Text levelCompleteText;
        [SerializeField] private UIButton reloadButton;

        private void OnEnable()
        {
            reloadButton.onClickEvent.AddListener(LoseButtonClick);
            _signalBus.Subscribe<GameSignals.LevelFailedSignal>(ShowView);
        }

        private void OnDisable()
        {
            reloadButton.onClickEvent.RemoveListener(LoseButtonClick);
            _signalBus.Unsubscribe<GameSignals.LevelFailedSignal>(ShowView);
        }


        private void ShowView()
        {
            levelCompleteText.text = "LEVEL " + _storageManager.CurrentLevel;
        }
        private void LoseButtonClick()
        {
            _signalBus.Fire(new GameSignals.LevelLoadSameSignal());
        }
    }
}