using UnityEngine;
using Zenject;

public class PCInputSystem : MonoBehaviour , IInputSystem
{
    [Inject] private Camera _camera;
    [Inject] private SignalBus _signalBus;
    private GameObject _selectedGameObject;
    private int _instanceId;

    private void Update()
    {
        InputSystem();
    }

    public void InputSystem()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var hit =CastRay();
            if (hit.HasValue && hit.Value.collider.gameObject.CompareTag("Player"))
            {
                _selectedGameObject = hit.Value.collider.transform.parent.gameObject;
                _instanceId = _selectedGameObject.GetInstanceID();
                //Debug.Log(_selectedGameObject.name + " Selected");
            }
        }

        if (_selectedGameObject != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                var hit = CastRay();
                if (hit.HasValue && hit.Value.collider.CompareTag("Ground"))
                {
                    var NewDestination = hit.Value.point;
                    //send transform to ai destination
                    _signalBus.Fire(new ColorBoxSignals.SendNewDestinationToAiSignal()
                    {
                        newDestinationTransform = NewDestination, instanceID = _instanceId,
                    });
                    //Debug.Log("Position send " + NewDestination);
                    _selectedGameObject = null;
                }
            }            
        }
        else
        {
          //  Debug.Log("No objectSelected");
        } 
    }

    private RaycastHit? CastRay()
    {
        var mousePositionInScreen = Input.mousePosition;
        Ray ray = _camera.ScreenPointToRay(mousePositionInScreen);
        RaycastHit hit;
        Debug.DrawRay(ray.origin,ray.direction * 50f,Color.red);
        if (Physics.Raycast(ray,out hit))
        {
            return hit;
        }
        return null;
    }
}
