using System;
using Gameloops;
using Gameloops.Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace GameloopsLite.Demo
{
    public class LevelManagerDemo : MonoBehaviour
    {
        [Inject] private StorageManager _storageManager;
        [Inject] private PlayerResource _playerResource;
        [Inject] private SignalBus _signalBus;
        private float _progress;

        private void Awake()
        {
            _signalBus.Fire(new GameSignals.LevelLoadedSignal());
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.A))
            {
                AddScore();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                LevelComplete();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                LevelLoadNext();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                LevelLoadSame();
            }
#endif
        }

        [Button]
        public void LevelStart()
        {
            _signalBus.Fire(new GameSignals.LevelStartedSignal()
            {
                Level = _storageManager.CurrentLevel
            });
            Debug.Log("LevelManager: LevelStart " + _storageManager.CurrentLevel);
        }

        [Button]
        public void AddScore(int scoreToAdd = 13) => _storageManager.CurrentScore += scoreToAdd;
        
        [Button]
        public void AddProgress()
        {
            _progress += 0.1f;
            _signalBus.Fire(new GameSignals.ProgressUpdatedSignal() { Progress = _progress });
        }

        [Button]
        public void LevelComplete()
        {
            Debug.Log("LevelManager: LevelComplete " + _storageManager.CurrentLevel);
            
            _signalBus.Fire(new GameSignals.LevelCompletedSignal()
            {
                Level = _storageManager.CurrentLevel
            });
            
            //Convert the score to resource if you want
            // _playerResource.GainResourceDefault(_storageManager.CurrentScore);
        }
      
        [Button]
        public void LevelFail()
        {
            Debug.Log("LevelManager: LevelFail " + _storageManager.CurrentLevel);
            _signalBus.Fire(new GameSignals.LevelFailedSignal(){ Level = _storageManager.CurrentLevel});
        }
        
        
        [Button]
        public void LevelLoadSame()
        {
            _signalBus.Fire(new GameSignals.LevelLoadSameSignal());
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        [Button]
        public void LevelLoadNext()
        {
            _storageManager.CurrentLevel++;
            _signalBus.Fire(new GameSignals.LevelLoadNextSignal());
        }
    }
}