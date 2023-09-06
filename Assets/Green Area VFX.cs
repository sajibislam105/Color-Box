using UnityEngine;
using Zenject;

public class GreenAreaVFX : MonoBehaviour
{
    [Inject] private SignalBus _signalBus;
    
    private void OnEnable()
    {
        _signalBus.Subscribe<ColorBoxSignals.NodeSelection>(MoveGreenArea);
    }
    
    private void OnDisable()
    {
        _signalBus.Unsubscribe<ColorBoxSignals.NodeSelection>(MoveGreenArea);
    }

    private void MoveGreenArea(ColorBoxSignals.NodeSelection nodeSelection)
    {
        if (nodeSelection.nodePosition != Vector3.zero)
        {
            //Debug.Log("Called");
            transform.position = nodeSelection.nodePosition;
        }
        else
        {
            transform.position = new Vector3(0,-50,-50);
        }
    }
}