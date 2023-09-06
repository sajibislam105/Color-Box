using System;
using Pathfinding;
using Pathfinding.RVO;
using UnityEngine;
using Zenject;

public class AnimationMovement : MonoBehaviour
{
    [Inject] private SignalBus _signalBus;
    private Animator _animator;
    private RVOController _rvoController;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rvoController = GetComponent<RVOController>();
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<ColorBoxSignals.SelectedDestination>(WalkingAnimation);
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<ColorBoxSignals.SelectedDestination>(WalkingAnimation);
    }
    private void WalkingAnimation(ColorBoxSignals.SelectedDestination signal)
    {
        /*Debug.Log("Walking Animation Called");
        if (_rvoController.locked == false)
        {
            Debug.Log("Inside Walking Animation Called");
            //_animator.SetBool("IsWalking", true);
            _animator.Play("Walking");
        }*/
    }
    
}
