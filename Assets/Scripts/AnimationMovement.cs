using UnityEngine;
using Zenject;

public class AnimationMovement : MonoBehaviour
{
    [Inject] private SignalBus _signalBus;
    private Animator _animator;
    private void Start()
    {
        _animator = GetComponent<Animator>();        
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<ColorBoxSignals.WalkingAnimationSignal>(WalkingAnimation);
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<ColorBoxSignals.WalkingAnimationSignal>(WalkingAnimation);
    }
    private void WalkingAnimation(ColorBoxSignals.WalkingAnimationSignal signal)
    {
        var selectedStatus = signal.Remote;
        _animator.SetBool("IsWalking", selectedStatus);
    }
}
