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

    private void OnEnable()
    {
        _signalBus.Subscribe<ColorBoxSignals.FirstTappedLevelStart>(OnLevelStart);
        _signalBus.Subscribe<ColorBoxSignals.LevelComplete>(OnLevelComplete);
        _signalBus.Subscribe<ColorBoxSignals.LevelFailed>(OnLevelFailed);
    }


    private void OnDisable()
    {
        _signalBus.Unsubscribe<ColorBoxSignals.FirstTappedLevelStart>(OnLevelStart);
        _signalBus.Unsubscribe<ColorBoxSignals.LevelComplete>(OnLevelComplete);
        _signalBus.Unsubscribe<ColorBoxSignals.LevelFailed>(OnLevelFailed);
    }

    private void Start()
    {
        PreGameScreen.gameObject.SetActive(true);
        CurencyScreen.gameObject.SetActive(true);
    }

    private void OnLevelStart()
    {
        PreGameScreen.gameObject.SetActive(false);
        InGameScreen.gameObject.SetActive(true);
        CurencyScreen.gameObject.SetActive(true);
    }
    
    private void OnLevelComplete()
    {
        PreGameScreen.gameObject.SetActive(false);
        InGameScreen.gameObject.SetActive(false);
        CurencyScreen.gameObject.SetActive(true);
        LevelCompleteScreen.gameObject.SetActive(true);
    }
    
    private void OnLevelFailed()
    {
        PreGameScreen.gameObject.SetActive(false);
        InGameScreen.gameObject.SetActive(false);
        CurencyScreen.gameObject.SetActive(true);
        LevelFailedScreen.gameObject.SetActive(true);
    }

}
