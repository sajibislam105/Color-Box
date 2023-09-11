using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas currencyScreen;
    [SerializeField] private Canvas preGameScreen;
    [SerializeField] private Canvas inGameScreen;
    [SerializeField] private Canvas levelCompleteScreen;
    [SerializeField] private Canvas levelFailedScreen;
    [SerializeField] private Canvas settingMenu;
    
    [Inject] private SignalBus _signalBus;
    [Inject] private ClaimAnimation _claimAnimation;
    private int _totalCoins;

    private int _balance;
    private int _balanceEarnedThisScene;

    private void Awake()
    {
        _totalCoins  = PlayerPrefs.GetInt("TotalCoins");
    }

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
            //Debug.Log("Total Coins at the start of level " + _totalCoins);
            currencyScreen.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>().text = _totalCoins.ToString();
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
        _balanceEarnedThisScene++;
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
        _signalBus.Fire(new ColorBoxSignals.ClaimedAndCoinAddedToBalance()
        {
            AddedAmount = _balanceEarnedThisScene
        });
        _totalCoins  = PlayerPrefs.GetInt("TotalCoins");
        currencyScreen.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>().text = _totalCoins.ToString();
    }

    public void OnSettingButtonClicked()
    {
        settingMenu.gameObject.SetActive(true);
    }
    public void OnSettingCloseButtonClicked()
    {
        settingMenu.gameObject.SetActive(false);
    }

    public void OnTapToStartButtonClicked()
    {
        //First Tapped on Screen
        _signalBus.Fire(new ColorBoxSignals.FirstTappedLevelStart());
        
    }
    
}
