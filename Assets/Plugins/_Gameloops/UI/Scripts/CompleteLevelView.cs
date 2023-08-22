using System;
using DG.Tweening;
using Doozy.Runtime.UIManager.Components;
using Gameloops.Player;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameloops.UI
{
    public class CompleteLevelView : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private PlayerResource _playerResource;
        [Inject] private StorageManager _storageManager;
        [Inject] private UIUtils _utils;
        
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Image resourceImage;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private UIButton nextButton;
        [SerializeField] private float spreadFactor = 10;

        private int _currentScore;
        private int _finalCurrentScore;
        [SerializeField] private float scoreIncrementDuration = 1f;
        [SerializeField] private float moveToResourceDuration = 1f;

        public int CurrentScore
        {
            get => _currentScore;
            set
            {
                _currentScore = value;
                scoreText.text = _currentScore.ToString();
            }
        }

        private void Awake()
        {
            nextButton.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            nextButton.onClickEvent.AddListener(WinButtonClick);
            _signalBus.Subscribe<GameSignals.LevelCompletedSignal>(OnLevelComplete);
        }

        private void OnDisable()
        {
            nextButton.onClickEvent.RemoveListener(WinButtonClick);
            _signalBus.Unsubscribe<GameSignals.LevelCompletedSignal>(OnLevelComplete);
        }

        private void OnLevelComplete(GameSignals.LevelCompletedSignal signal)
        {
            levelText.text = "LEVEL " + (_storageManager.CurrentLevel);
            resourceImage.sprite = _playerResource.DefaultResource.unitPreviewSprite;
            CurrentScore = 0;
            _finalCurrentScore = _storageManager.CurrentScore;
            StartCoroutine(_utils.IncrementScore(CurrentScore, _finalCurrentScore, scoreIncrementDuration, i => CurrentScore = i,
                () =>
                {
                    _utils.AddResourcesWithAnimation(resourceImage.transform, _playerResource.DefaultResource, _finalCurrentScore, 10,
                        spreadFactor, moveToResourceDuration);
                }));

            Debug.Log("Level Complete UI OnLevelComplete");
            Observable.Timer(TimeSpan.FromSeconds(scoreIncrementDuration + moveToResourceDuration * 1.25f)).Subscribe(_ =>
            {
                Debug.Log("set next button to true");
                nextButton.gameObject.SetActive(true);
                // nextButton.transform.localScale = Vector3.zero;
                // nextButton.transform.DOScale(1f, 0.25f);
            }).AddTo(this);
        }
        

        private void WinButtonClick()
        {
            _signalBus.Fire(new GameSignals.LevelLoadNextSignal());
        }

    }
}