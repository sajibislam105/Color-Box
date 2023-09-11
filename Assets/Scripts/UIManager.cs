using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Zenject;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas currencyScreen;
    [SerializeField] private Canvas preGameScreen;
    [SerializeField] private Canvas inGameScreen;
    [SerializeField] private Canvas levelCompleteScreen;
    [SerializeField] private Canvas levelFailedScreen;
    
    [Inject] private SignalBus _signalBus;

    private int _balance;
    private int _balanceEarnedThisScene;
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
        _signalBus.Unsubscribe<ColorBoxSignals.CoinEarned>(OnCoinEarned);
    }

    private void Start()
    {
        preGameScreen.gameObject.SetActive(true);
        currencyScreen.gameObject.SetActive(true);
        inGameScreen.gameObject.SetActive(false);
        if (PlayerPrefs.HasKey("TotalCoins")) 
        {
            _balance = PlayerPrefs.GetInt("TotalCoins");
            Debug.Log(_balance);
            currencyScreen.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>().text = _balance.ToString();
        }
        else
        {
            // The key doesn't exist; handle accordingly (e.g., set a default value).
            //Balance = PlayerPrefs.GetInt("PlayerScore", 0);
            //Debug.Log("The balance is Reset.");
        }
        
    }

    private void OnLevelStart()
    {
        preGameScreen.gameObject.SetActive(false);
        inGameScreen.gameObject.SetActive(true);
        currencyScreen.gameObject.SetActive(true);
    }

    private void OnCountingRemainingMoves(ColorBoxSignals.RemainingMoves signal)
    {
        var remainingMoves = signal.remainingMoves.ToString();
        inGameScreen.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>().text = "Moves Left: " + remainingMoves;
    }

    private void OnLevelComplete()
    {
        preGameScreen.gameObject.SetActive(false);
        inGameScreen.gameObject.SetActive(false);
        currencyScreen.gameObject.SetActive(true);
        levelCompleteScreen.gameObject.SetActive(true);
        levelCompleteScreen.GetComponentInChildren<Canvas>().GetComponentInChildren<LevelCompleteScreenUI>().youEarnedText
            .text = "Coins Earned " + _balanceEarnedThisScene;
        
    }
    private void OnLevelFailed()
    {
        preGameScreen.gameObject.SetActive(false);
        inGameScreen.gameObject.SetActive(false);
        currencyScreen.gameObject.SetActive(true);
        levelFailedScreen.gameObject.SetActive(true);
    }
    private void OnCoinEarned()
    {
        _balance++;
        _balanceEarnedThisScene++;
        currencyScreen.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>().text = _balance.ToString();
        //Debug.Log("balance: "+ CurrencyScreen.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>().text);
        //Debug.Log("BalanceEarnedThisScene: "+ BalanceEarnedThisScene);

    }
    public void ReloadLevel()
    {
        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Reload the current scene
        SceneManager.LoadScene(currentSceneIndex);
        }

    public void OnClaimButtonClicked()
    {
        _signalBus.Fire(new ColorBoxSignals.CoinAddedToBalance()
        {
            AddedAmount = _balanceEarnedThisScene
        });
    }
    
}
