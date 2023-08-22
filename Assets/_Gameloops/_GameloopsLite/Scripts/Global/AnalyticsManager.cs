using System;
using UnityEngine;
using Zenject;

namespace Gameloops
{
    public class AnalyticsManager: IInitializable, IDisposable
    {
        // [Inject] private Lightneer.Analytics.AnalyticsManager _analyticsManager;
        [Inject] private GameSettings _gameSettings;
        [Inject] private SignalBus _signalBus;

        public void Initialize()
        {
            if (!_gameSettings.moduleSettings.useAnalytics) return;
            _signalBus.Subscribe<GameSignals.LevelStartedSignal>(OnLevelStart);
            _signalBus.Subscribe<GameSignals.LevelCompletedSignal>(OnLevelComplete);
            _signalBus.Subscribe<GameSignals.LevelFailedSignal>(OnLevelFail);
        }
        
        public void Dispose()
        {
            if (!_gameSettings.moduleSettings.useAnalytics) return;
            _signalBus.Unsubscribe<GameSignals.LevelStartedSignal>(OnLevelStart);
            _signalBus.Unsubscribe<GameSignals.LevelCompletedSignal>(OnLevelComplete);
            _signalBus.Unsubscribe<GameSignals.LevelFailedSignal>(OnLevelFail);
        }

        private void OnLevelStart(GameSignals.LevelStartedSignal signal)
        {
            LevelStarted(signal.Level);
        }

        private void OnLevelComplete(GameSignals.LevelCompletedSignal signal)
        {
            LevelComplete(signal.Level);
        }

        private void OnLevelFail(GameSignals.LevelFailedSignal signal)
        {
            LevelFail(signal.Level);
        }


        /// <summary>
        /// Called when level is started.
        /// </summary>
        /// <param name="level"> -1 indicates that game isn't level based</param>
        public void LevelStarted(int level = -1)
        {
            if (!_gameSettings.moduleSettings.useAnalytics) return;
            
            //TODO Call the required function here

            // _analyticsManager.OnGameStarted(level.ToString());
            //SupersonicWisdom.Api.NotifyLevelStarted(level, null);
            Debug.Log("Sending level start of level " + level);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level">-1 indicates that game isn't level based</param>
        /// <param name="score">0 indicates that there is no score based system</param>
        public void LevelComplete(int level = -1, int score = 0)
        {
            if (!_gameSettings.moduleSettings.useAnalytics) return;
            
            //TODO Call the required function here
            
            // _analyticsManager.OnGameCompleted(level.ToString());
            //SupersonicWisdom.Api.NotifyLevelCompleted(level, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level">-1 indicates that game isn't level based</param>
        /// <param name="score">0 indicates that there is no score based system</param>
        public void LevelFail(int level = -1, int score = 0)
        {
            if (!_gameSettings.moduleSettings.useAnalytics) return;
            
            //TODO Call the required function here

            // _analyticsManager.OnGameFailed(level.ToString());
            //SupersonicWisdom.Api.NotifyLevelFailed(level, null);
        }

        
    }
}