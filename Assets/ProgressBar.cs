using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image Bar;

    [Inject] private SignalBus _signalBus;

    private void OnEnable()
    {
        _signalBus.Subscribe<ColorBoxSignals.CompletionProgressBarSignal>(GetCurrentFIll);
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<ColorBoxSignals.CompletionProgressBarSignal>(GetCurrentFIll);
    }

    private void Start()
    {
        Bar.fillAmount = 0;
    }

    private void GetCurrentFIll(ColorBoxSignals.CompletionProgressBarSignal signal)
    {
        var fillAmount = signal.ProgressBarFillAmount;
        Bar.fillAmount = fillAmount;
    }
}
