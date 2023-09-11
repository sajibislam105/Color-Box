using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ClaimAnimation : MonoBehaviour
{
    [SerializeField] private Image coinImage1;
    [SerializeField] private Image coinImage2;
    [SerializeField] private Image coinImage3;
    [SerializeField] private Image coinImage4;
    [SerializeField] private Image _currencyScreenCoinImage;

    [Inject] private SignalBus _signalBus;
    private RectTransform _rectTransform;
    private float DurationOfMove;
    private float DurationOfRotation;
    private float _move;
    private float _rotate;
    
    
    private void OnEnable()
    {
        _signalBus.Subscribe<ColorBoxSignals.ClaimedAndCoinAddedToBalance>(OnClickedClaimAnimation);
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<ColorBoxSignals.ClaimedAndCoinAddedToBalance>(OnClickedClaimAnimation);
    }

    private void Start()
    {
        DurationOfMove = 1f;
        DurationOfRotation = 2f;
        coinImage1.enabled = false;
        coinImage2.enabled = false;
        coinImage3.enabled = false;
        coinImage4.enabled = false;
    }

    private void OnClickedClaimAnimation()
    {
        coinImage1.enabled = true;
        coinImage2.enabled = true;
        coinImage3.enabled = true;
        coinImage4.enabled = true;
        
        
        coinImage1.rectTransform.DOMoveX(coinImage1.rectTransform.position.x - 350f,DurationOfMove).SetEase(Ease.OutBack).OnComplete((() =>
        {
            coinImage1.rectTransform.DORotate(new Vector3(0f, 0f, 90), DurationOfRotation).SetLoops(3,LoopType.Incremental).SetEase(Ease.Flash);
            coinImage1.rectTransform.DOMove(_currencyScreenCoinImage.rectTransform.position, DurationOfMove);
                
        }));
        coinImage2.rectTransform.DOMoveX(coinImage2.rectTransform.position.x + 350f,DurationOfMove).SetEase(Ease.OutBack).OnComplete((() =>
        {
            coinImage2.rectTransform.DORotate(new Vector3(0f, 0f, 90), DurationOfRotation).SetLoops(3,LoopType.Incremental).SetEase(Ease.Flash);
            coinImage2.rectTransform.DOMove(_currencyScreenCoinImage.rectTransform.position, DurationOfMove);
                
        }));
        
        coinImage3.rectTransform.DOMoveY(coinImage3.rectTransform.position.y - 150f,DurationOfMove).SetEase(Ease.OutBack).OnComplete((() =>
        {
            coinImage3.rectTransform.DORotate(new Vector3(0f, 0f, 90), DurationOfRotation).SetLoops(3,LoopType.Incremental).SetEase(Ease.Flash);
            coinImage3.rectTransform.DOMove(_currencyScreenCoinImage.rectTransform.position, DurationOfMove);
                
        }));
        coinImage4.rectTransform.DOMoveY(coinImage4.rectTransform.position.y + 150f,DurationOfMove).SetEase(Ease.OutBack).OnComplete((() =>
        {
            coinImage4.rectTransform.DORotate(new Vector3(0f, 0f, 90), DurationOfRotation).SetLoops(3,LoopType.Incremental).SetEase(Ease.Flash);
            coinImage4.rectTransform.DOMove(_currencyScreenCoinImage.rectTransform.position, DurationOfMove);
        }));
           
    }
}
