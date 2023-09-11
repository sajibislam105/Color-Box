using UnityEngine;
using Zenject;

namespace Input_System
{
    public class PCInputSystem : MonoBehaviour
    {
        [Inject] private Camera _camera;
        [Inject] private SignalBus _signalBus;
        private GameObject _selectedGameObject;
        private int _instanceId;
    
        private void Update()
        {
            InputSystem();
        }

        private void InputSystem()
        {
            if (Input.GetMouseButtonDown(0) && _selectedGameObject == null)
            {
                var hit = CastRay();
                if (hit.HasValue && hit.Value.collider.gameObject.CompareTag("Player"))
                {
                    _selectedGameObject = hit.Value.collider.transform.parent.gameObject;
                    _instanceId = _selectedGameObject.GetInstanceID();
                    //Debug.Log(_selectedGameObject.name + " Selected");

                    if (_selectedGameObject != null)
                    {
                        //play particle effect
                        _signalBus.Fire(new ColorBoxSignals.AgentSelectionStatus()
                        {
                            Status = true,
                            InstanceID = _instanceId

                        });
                        //gameObject.layer uses only integers, but we can turn a layer name into a layer integer using LayerMask.NameToLayer()
                        int LayerName = LayerMask.NameToLayer("Player");
                        _selectedGameObject.layer = LayerName;
                        var childs = _selectedGameObject.gameObject.GetComponentsInChildren<Transform>();
                        foreach (var child in childs)
                        {
                            int LayerNameChild = LayerMask.NameToLayer("Player");
                            child.gameObject.layer = LayerNameChild;
                            //Debug.Log("Child Layer name changed");
                        }
                        //Debug.Log("Layer name changed");
                    }
                    else
                    {
                        _signalBus.Fire(new ColorBoxSignals.AgentSelectionStatus()
                        {
                            Status = false,
                            InstanceID = 0
                        });
                    }
                }
            }
            else
            {
                if (_selectedGameObject != null)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        var hit = CastRay();
                        if (hit.HasValue && hit.Value.collider.CompareTag("Ground"))
                        {
                            var newDestination = hit.Value.point;
                            //send transform to ai destination
                            _signalBus.Fire(new ColorBoxSignals.SelectedDestination()
                            {
                                NewDestinationTransform = newDestination,
                                InstanceID = _instanceId
                            });
                            var gridGraph = AstarPath.active.data.gridGraph;
                            LayerMask currentHeightMask = gridGraph.collision.heightMask;
                            // Define the layer you want to add
                            int layerToAdd = LayerMask.NameToLayer("Default");
                            // Use a bitwise OR operation to add the layer to the height mask
                            currentHeightMask |= (1 << layerToAdd);
                            // Assign the modified height mask back to the graph
                            gridGraph.collision.heightMask = currentHeightMask;
                            gridGraph.Scan();
                            //Debug.Log("Graph scanned after layer name changed to player");


                            //gameObject.layer uses only integers, but we can turn a layer name into a layer integer using LayerMask.NameToLayer()
                            int LayerName = LayerMask.NameToLayer("Default");
                            _selectedGameObject.layer = LayerName;
                            var childs = _selectedGameObject.gameObject.GetComponentsInChildren<Transform>();
                            foreach (var child in childs)
                            {
                                int LayerNameChild = LayerMask.NameToLayer("Default");
                                child.gameObject.layer = LayerNameChild;
                                //Debug.Log("Child Layer name reset to Default");
                            }

                            //Debug.Log("Layer name reset to Default");
                            _selectedGameObject = null;
                        }
                    }
                }
                else
                {
                    //  Debug.Log("No objectSelected");
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
}