using TMPro;
using UnityEngine;
using Zenject;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas CurencyScreen;
    [SerializeField] private Canvas PreGameScreen;
    [SerializeField] private Canvas InGameScreen;
    [SerializeField] private Canvas LevelCompleteScreen;
    [SerializeField] private Canvas LevelFailedScreen;
    
    [Inject] private SignalBus _signalBus;

    private int Balance;
    private void OnEnable()
    {
        _signalBus.Subscribe<ColorBoxSignals.FirstTappedLevelStart>(OnLevelStart);
        _signalBus.Subscribe<ColorBoxSignals.LevelComplete>(OnLevelComplete);
        _signalBus.Subscribe<ColorBoxSignals.LevelFailed>(OnLevelFailed); 
        _signalBus.Subscribe<ColorBoxSignals.RemainingMoves>(OnCountingRemainingMoves);
        _signalBus.Subscribe<ColorBoxSignals.CoinEarned>(OnCoinEarned);
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<ColorBoxSignals.FirstTappedLevelStart>(OnLevelStart);
        _signalBus.Unsubscribe<ColorBoxSignals.LevelComplete>(OnLevelComplete);
        _signalBus.Unsubscribe<ColorBoxSignals.LevelFailed>(OnLevelFailed);
        _signalBus.Unsubscribe<ColorBoxSignals.RemainingMoves>(OnCountingRemainingMoves);
        _signalBus.Subscribe<ColorBoxSignals.CoinEarned>(OnCoinEarned);
    }

    private void Start()
    {
        PreGameScreen.gameObject.SetActive(true);
        CurencyScreen.gameObject.SetActive(true);
        InGameScreen.gameObject.SetActive(false);
        Balance = 0;
    }

    private void OnLevelStart()
    {
        PreGameScreen.gameObject.SetActive(false);
        InGameScreen.gameObject.SetActive(true);
        CurencyScreen.gameObject.SetActive(true);
    }
    
    
    private void OnCountingRemainingMoves(ColorBoxSignals.RemainingMoves signal)
    {
        var remainingMoves = signal.remainingMoves.ToString();
        InGameScreen.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>().text = "Moves Left: " + remainingMoves;
    }
    
    
    private void OnLevelComplete()
    {
        PreGameScreen.gameObject.SetActive(false);
        InGameScreen.gameObject.SetActive(false);
        CurencyScreen.gameObject.SetActive(true);
        LevelCompleteScreen.gameObject.SetActive(true);
        LevelCompleteScreen.GetComponentInChildren<Canvas>().GetComponentInChildren<LevelCompleteScreenUI>().youEarnedText
            .text = "Coins Earned " + Balance;
    }
    
    private void OnLevelFailed()
    {
        PreGameScreen.gameObject.SetActive(false);
        InGameScreen.gameObject.SetActive(false);
        CurencyScreen.gameObject.SetActive(true);
        LevelFailedScreen.gameObject.SetActive(true);
    }
    private void OnCoinEarned()
    {
        Balance++;
        CurencyScreen.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>().text = Balance.ToString();
        Debug.Log("balance: "+ CurencyScreen.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>().text);

    }

}
