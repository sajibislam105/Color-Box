using UnityEngine;
using Zenject;

public class ParticleSystemManage : MonoBehaviour
{
    [Inject] private SignalBus _signalBus;
    private ParticleSystem _particleSystemInChild;

    void Start()
    {
        _particleSystemInChild = GetComponentInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<ColorBoxSignals.AgentSelectionStatus>(ParticleSystemStatus);
    }
    
    private void OnDisable()
    {
        _signalBus.Unsubscribe<ColorBoxSignals.AgentSelectionStatus>(ParticleSystemStatus);
    }

    private void ParticleSystemStatus(ColorBoxSignals.AgentSelectionStatus signal)
    {
        bool status = signal.Status;
        if (status && (gameObject.GetInstanceID() == signal.instanceID))
        {
            _particleSystemInChild.Play();
        }
        else
        {
            _particleSystemInChild.Stop();
        }
    }
}
