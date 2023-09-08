using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] private SignalBus _signalBus;
    [SerializeField] private int AvailableMoveCountForThisLevel;
    [SerializeField] private int moveCount;
    
    private void Awake()
    {
        moveCount = -4;
        _signalBus.Fire(new ColorBoxSignals.LoadEverything());
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<ColorBoxSignals.MoveCounter>(MoveCountManager);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<ColorBoxSignals.MoveCounter>(MoveCountManager);
    }

    private void Update()
    {
        HasLevelCompleted();
        
        if (moveCount == AvailableMoveCountForThisLevel)
        {
            CalculateRemainingMoveCount();
        }
    }

    private bool HasLevelCompleted()
    {
        var Agents = GameObject.FindGameObjectsWithTag("Agent");
        if (Agents.Length == 0)
        {
            //Debug.Log("No Agents Found on Screen");
            _signalBus.Fire(new ColorBoxSignals.LevelComplete());
            return true;
        }
        return false;
    }

    private void CalculateRemainingMoveCount()
    {
        if (moveCount == ( AvailableMoveCountForThisLevel ) && HasLevelCompleted())
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

    private void MoveCountManager()
    {
        moveCount++;
        var RemainingMoves = AvailableMoveCountForThisLevel - moveCount;
        var progressBar = RemainingMoves / AvailableMoveCountForThisLevel;
        _signalBus.Fire(new ColorBoxSignals.ProgressBarStatus()
        {
            BarCompletePercentage = progressBar
        });
        Debug.Log("Progress bar signal sent");
    }
}