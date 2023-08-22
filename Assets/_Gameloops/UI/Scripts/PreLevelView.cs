using System;
using System.Collections;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gameloops.UI
{
    public class PreLevelView : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private StorageManager _storageManager;
        [Inject] private UIUtils _utils;
        
        [SerializeField] private bool startOnTouch = true;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private UIButton startButton;
        private bool _levelStarted = false;

        private void OnEnable()
        {
            SetPreGameUI();
            if(startButton) startButton.onClickEvent.AddListener(StartGame);
            _signalBus.Subscribe<GameSignals.LevelLoadedSignal>(SetPreGameUI);

        }

        private void OnDisable()
        {
            if(startButton) startButton.onClickEvent.RemoveListener(StartGame);
            _signalBus.Unsubscribe<GameSignals.LevelLoadedSignal>(SetPreGameUI);
        }

        private void Start()
        {
            if (startOnTouch) StartCoroutine(WaitForValidTouch());
        }

        private IEnumerator WaitForValidTouch()
        {
            while (!_levelStarted)
            {
                if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
                {
                    if (!_utils.IsPointOverGui(Input.mousePosition))
                        StartGame();
                }
                yield return null;
            }
        }

        private void SetPreGameUI()
        {
            if (levelText == null) Debug.Log("levelText is null");
            levelText.text = "LEVEL " + 
                             _storageManager.CurrentLevel;
        }
        private void StartGame()
        {
            if (_levelStarted) return;
            _levelStarted = true;
            _signalBus.Fire(new GameSignals.LevelStartedSignal()
            {
                Level = _storageManager.CurrentLevel
            });
        }


    }
}