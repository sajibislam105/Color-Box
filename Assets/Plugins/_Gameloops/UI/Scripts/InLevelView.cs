using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameloops.UI
{
    public class InLevelView : MonoBehaviour
    {
        [Inject] private StorageManager _storageManager;
        [Inject] private SignalBus _signalBus;
        [SerializeField] private TMP_Text currentLevel;
        [SerializeField] private TMP_Text nextLevel;
        [SerializeField] private Image progressFill;

        private void OnEnable()
        {
            _signalBus.Subscribe<GameSignals.ProgressUpdatedSignal>(StatusUpdate);
            _signalBus.Subscribe<GameSignals.LevelStartedSignal>(PrepareUI);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<GameSignals.LevelStartedSignal>(PrepareUI);
            _signalBus.Unsubscribe<GameSignals.ProgressUpdatedSignal>(StatusUpdate);

        }
        private void PrepareUI()
        {
            progressFill.fillAmount = 0f;
            currentLevel.text = _storageManager.CurrentLevel.ToString();
            nextLevel.text = (_storageManager.CurrentLevel+1).ToString();
        }

        private void StatusUpdate(GameSignals.ProgressUpdatedSignal progressUpdatedSignal)
        {
            progressFill.fillAmount = progressUpdatedSignal.Progress;
        }
        
        
    }
}