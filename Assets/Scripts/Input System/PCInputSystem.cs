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
            Debug.Log("mouse button");
            if (_selectedGameObject == null)
            {
                var hit = CastRay();
                if (hit.HasValue && hit.Value.collider.gameObject.CompareTag("Player"))
                {
                    _selectedGameObject = hit.Value.collider.transform.parent.gameObject;
                    _instanceId = _selectedGameObject.GetInstanceID();
                    //Debug.Log(_selectedGameObject.name + " Selected");
                }
            }
            else
            {
                var hit = CastRay();
                if (hit.HasValue && hit.Value.collider.CompareTag("Ground"))
                {
                    var NewDestination = hit.Value.point;
                    //send transform to ai destination
                    _signalBus.Fire(new ColorBoxSignals.SelectedDestination()
                    {
                        newDestinationPosition = NewDestination, instanceID = _instanceId,
                    });
                    //Debug.Log("Position send " + NewDestination);
                    _selectedGameObject = null;
                }
            }
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
