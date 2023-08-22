using System;
using Doozy.Runtime.Signals;
using UnityEngine;
using Zenject;

namespace Gameloops.UI
{
    public class DoozySignalAdapter : MonoBehaviour
    {
        [SerializeField] private StreamId.Level levelStart;
        [SerializeField] private StreamId.Level levelComplete;
        [SerializeField] private StreamId.Level levelFail;

        [Inject] private SignalBus _signal;

        private void OnEnable()
        {
            _signal.Subscribe<GameSignals.LevelStartedSignal>(OnLevelStart);
            _signal.Subscribe<GameSignals.LevelCompletedSignal>(OnLevelComplete);
            _signal.Subscribe<GameSignals.LevelFailedSignal>(OnLevelFail);
        }

        private void OnDisable()
        {
            _signal.Unsubscribe<GameSignals.LevelStartedSignal>(OnLevelStart);
            _signal.Unsubscribe<GameSignals.LevelCompletedSignal>(OnLevelComplete);
            _signal.Unsubscribe<GameSignals.LevelFailedSignal>(OnLevelFail);
        }

        private void OnLevelStart()
        {
            Signal.Send(levelStart);
        }

        private void OnLevelComplete()
        {
            Signal.Send(levelComplete);
        }

        private void OnLevelFail()
        {
            Signal.Send(levelFail);
        }
    }
}