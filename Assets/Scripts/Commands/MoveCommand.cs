using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class MoveCommand : Command
{
    [Inject] private Camera _camera;
    [Inject] private SignalBus _signalBus;
    private GameObject _selectedGameObject;
    private int _instanceId;

    
    
    public MoveCommand( AIDestinationSetterCustom Agent, GameObject SelectedGameObject/* here i need to construct the class */ )
    {
        _selectedGameObject = SelectedGameObject;
        _instanceId = _selectedGameObject.GetInstanceID();
    }

    protected override async Task AsyncExecuter()
    {
        Debug.Log("protected override async Task Async Executer called");
        if (_selectedGameObject == null)
        {
            if (_selectedGameObject.gameObject.CompareTag("Player"))
            {
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
                    
                    if (_selectedGameObject.CompareTag("Ground"))
                    {
                        var newDestination = _selectedGameObject.transform.position;
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
            await Task.Delay(20); 
        }
    }
    
}
