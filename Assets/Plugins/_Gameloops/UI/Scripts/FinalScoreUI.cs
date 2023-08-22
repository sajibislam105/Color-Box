using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameloops.UI
{
    public class FinalScoreUI : MonoBehaviour
    {
        [Inject] private UIUtils _utils;
        [Inject] private StorageManager _storageManager;
        [SerializeField] private float scorePnlOpenDuration = 1f;
        [SerializeField] private float spreadFactor = 100f;
        [SerializeField] private TMP_Text entityText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Transform currencyTransform;
        [SerializeField] private Transform finalCurrencyTransform;
        [SerializeField] private Transform currencyPrefab;
        
        private Vector3 _startPosition;
        private int _finalEntityScore;
        private int _finalCurrentScore;
        private int _entityScore;
        public int EntityScore
        {
            get => _entityScore;
            set
            {
                _entityScore = value;
                entityText.text = "x" + _entityScore.ToString();
            }
        }

        public int CurrentScore
        {
            get => _currentScore;
            set
            {
                _currentScore = value;
                scoreText.text = _currentScore.ToString();
            }
        }

        private int _currentScore;
        [Inject] private SignalBus _signalBus;
        [Inject] private HapticManager _hapticManager;

        public void Prepare(int entityScore, int currentScore)
        {
            EntityScore = 0;
            CurrentScore = 0;
            _finalEntityScore = entityScore;
            _finalCurrentScore = currentScore;
            transform.localScale = Vector3.zero;
        }
        
        public void Show(Action onComplete = null)
        {
            transform.DOScale(1, scorePnlOpenDuration).OnComplete(() =>
            {
                StartCoroutine(_utils.IncrementScore(EntityScore, _finalEntityScore, 0.5f, i => EntityScore = i));   
                StartCoroutine(_utils.IncrementScore(CurrentScore, _finalCurrentScore, 0.25f, i => CurrentScore = i,
                    () =>
                    {
                        onComplete?.Invoke();
                        CollectCoins(_finalCurrentScore);
                    }));  
            });
            
            //scoreText.AnimationManager.PlayAnimation();
        }

        public void CollectCoins(int coins = 100, int scale = 10)
        {
            var amount = coins / scale;
            if (amount > 20) amount = 20;
            for (int i = 0; i < amount; i++)
            {
                var currency = Instantiate(currencyPrefab, currencyTransform.position, Quaternion.identity, transform);
                var currentPos = currency.position;
                
                var randomPos = new Vector3(
                    currentPos.x + Random.Range(-spreadFactor, spreadFactor), 
                    currentPos.y + Random.Range(-spreadFactor, spreadFactor), 
                    currentPos.z);

                //finalCurrencyTransform.DOScale(1.25f, 0.5f).SetDelay(1f);
                currency.DOMove(randomPos, 0.5f).SetDelay(Random.Range(0f, 0.5f)).OnComplete(() =>
                {
                    currency.DOMove(finalCurrencyTransform.position, 0.5f)
                        .OnComplete(() =>
                        {
                            finalCurrencyTransform.localScale = Vector3.one;
                            finalCurrencyTransform.DOScale(1.25f, 0.2f)
                                .OnComplete(() => finalCurrencyTransform.DOScale(1, .2f));
                            _hapticManager.HapticNotification(HapticNotificationType.Success);
                            Destroy(currency.gameObject);
                            // TODO Resource Update signal
                        });
                });
                //finalCurrencyTransform.DOScale(1f, 0.5f).SetDelay(2f);
            }
        }

        
    }
}