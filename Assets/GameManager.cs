using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int availableMoveCountForThisLevel;
    [SerializeField] private int moveCount;
    private int _remainingMoves;
    
    private int _totalAgentsOnScreen;
    private int _coupleMatchedCount;
    private int _coinCount;

    private int totalCoins { get; set; }


    [Inject] private SignalBus _signalBus;
    private void Awake()
    {
        moveCount = -4;
        _signalBus.Fire(new ColorBoxSignals.LoadEverything());
    }
    private void OnEnable()
    {
        _signalBus.Subscribe<ColorBoxSignals.MoveCounter>(RemainingMoveCounter);
        _signalBus.Subscribe<ColorBoxSignals.CoupleMergeCount>(CompletionProgressBarCalculation);
        _signalBus.Subscribe<ColorBoxSignals.CoinEarned>(OnCoinEarned);
        _signalBus.Subscribe<ColorBoxSignals.CoinAddedToBalance>(TotalCoinCalculation);
    }

    private void TotalCoinCalculation(ColorBoxSignals.CoinAddedToBalance signal)
    {
        totalCoins += signal.AddedAmount;
        PlayerPrefs.SetInt("TotalCoins",totalCoins);
        PlayerPrefs.Save();
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<ColorBoxSignals.MoveCounter>(RemainingMoveCounter);
        _signalBus.Unsubscribe<ColorBoxSignals.CoupleMergeCount>(CompletionProgressBarCalculation);
        _signalBus.Unsubscribe<ColorBoxSignals.CoinEarned>(OnCoinEarned);
        _signalBus.Unsubscribe<ColorBoxSignals.CoinAddedToBalance>(TotalCoinCalculation);
    }
    private void Start()
    {
        _totalAgentsOnScreen = GameObject.FindGameObjectsWithTag("Agent").Length;
        //Debug.Log($"total couples on screen: {_totalAgentsOnScreen * 0.5}");
    }
    private void Update()
    {
        HasLevelCompleted();
        
        if (moveCount == availableMoveCountForThisLevel)
        {
            CalculateRemainingMoveCountAndLevelStatus();
        }
    }
    private bool HasLevelCompleted()
    {
        var agents = GameObject.FindGameObjectsWithTag("Agent");
        if (agents.Length == 0)
        {
            //Debug.Log("No Agents Found on Screen");
            _signalBus.Fire(new ColorBoxSignals.LevelComplete());
            return true;
        }
        return false;
    }
    private void CalculateRemainingMoveCountAndLevelStatus()
    {
        if (moveCount == ( availableMoveCountForThisLevel ) && HasLevelCompleted())
        {
            Debug.Log("Level Complete and Moves are complete");
            HasLevelCompleted();
        }
        else
        {
            Debug.Log("Level Failed and Moves are Finished");
            _signalBus.Fire(new ColorBoxSignals.LevelFailed());
        }
    }
    private void RemainingMoveCounter()
    {
        moveCount++;
        //Debug.Log($"Move Count: {moveCount}");
        _remainingMoves = availableMoveCountForThisLevel - moveCount;
        _signalBus.Fire(new ColorBoxSignals.RemainingMoves()
        {
            remainingMoves = _remainingMoves
        });
    }
    private void CompletionProgressBarCalculation(ColorBoxSignals.CoupleMergeCount signal)
    {
        _coupleMatchedCount++;
        var totalCouples = _totalAgentsOnScreen *  0.5f;        
        if (_coupleMatchedCount >= 0)
        {            
            var remainingCouples = (totalCouples - _coupleMatchedCount);
            var remainingAgents = remainingCouples * 2;
            // Completion Progress Percentage = [(Available Moves - Remaining Moves) / Available Moves] * 100
            var progressCompleted = ((_totalAgentsOnScreen - remainingAgents) / _totalAgentsOnScreen * 100.0f) / 100.0f; 
            //Debug.Log($"Remaining Agents {remainingAgents} and complete percentage is: {progressCompleted} "); 
            _signalBus.Fire(new ColorBoxSignals.CompletionProgressBarSignal()
            {
                ProgressBarFillAmount = progressCompleted
            });
        }
    }
    private void OnCoinEarned()
    {
        _coinCount++;
        
        //saving the balance
        PlayerPrefs.SetInt("PlayerBalance", _coinCount);
        PlayerPrefs.Save();
    }
}