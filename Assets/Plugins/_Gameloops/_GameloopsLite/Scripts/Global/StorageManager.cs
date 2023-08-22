using System;
using UnityEngine;
using Zenject;

namespace Gameloops
{
    public class StorageManager: IInitializable, IDisposable
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private HapticManager _hapticManager;
        
        public void Initialize()
        {
            _signalBus.Subscribe<GameSignals.LevelLoadedSignal>(ResetCurrentScore);    
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<GameSignals.LevelLoadedSignal>(ResetCurrentScore);    
        }

        private void ResetCurrentScore()
        {
            CurrentScore = 0;
        }

        public int CurrentLevel
        {
            get => LoadOrCreateKeyInt(CurrentLevelKey, 1);
            set => PlayerPrefs.SetInt(CurrentLevelKey, value);
        }
        public bool IsHapticOn
        {
            get => LoadOrCreateKeyInt(HapticsKey, 1) == 1;
            set
            {
                PlayerPrefs.SetInt(HapticsKey, value ? 1 : 0);
                if(value != _hapticManager.HapticEnabled) _hapticManager.ToggleHaptic();
            }
        }

        public bool IsSfxOn
        {
            get => LoadOrCreateKeyInt(SfxKey, 1) == 1;
            set => PlayerPrefs.SetInt(SfxKey, value ? 1 : 0);
        }
        public bool IsMusicOn
        {
            get => LoadOrCreateKeyInt(MusicKey, 1) == 1;
            set => PlayerPrefs.SetInt(MusicKey, value ? 1 : 0);
        }
        public bool IsLevelInProgress
        {
            get => LoadOrCreateKeyInt(IsLevelInProgressKey, 1) == 1;
            set => PlayerPrefs.SetInt(IsLevelInProgressKey, value ? 1 : 0);
        }
        public int CurrentScore
        {
            get;
            set;
        }

        private const string CurrentLevelKey = "CurrentLevel";
        private const string HapticsKey = "HapticsKey";
        private const string SfxKey = "SfxKey";
        private const string MusicKey = "MusicKey";
        private const string IsLevelInProgressKey = "IsLevelInProgress";

        private int LoadOrCreateKeyInt(string key, int defaultValue = 1)
        {
            if (PlayerPrefs.HasKey(key))
                return PlayerPrefs.GetInt(key);
            else
            {
                PlayerPrefs.SetInt(key, defaultValue);
                return defaultValue;
            }
        }

        
    }
}