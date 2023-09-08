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
        _signalBus.Subscribe<ColorBoxSignals.ProgressBarStatus>(GetCurrentFIll);
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<ColorBoxSignals.ProgressBarStatus>(GetCurrentFIll);
    }

    void Update()
    {
        //GetCurrentFIll();
    }

    private void GetCurrentFIll(ColorBoxSignals.ProgressBarStatus signal)
    {
        
        //var fillAmount = (float)currentCompletion / (float)maximum;
        var fillAmount = signal.BarCompletePercentage;
        Debug.Log(fillAmount);
        Bar.fillAmount = fillAmount;
    }
}
