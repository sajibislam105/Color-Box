using Pathfinding;
using UnityEngine;
using Zenject;

public class AIDestinationSetterCustom : MonoBehaviour
{
    [SerializeField] private  Vector3 target;
    [Inject] private SignalBus _signalBus;
    [Inject] private GridNodeInformation _gridNodeInformation;

    private IAstarAI _ai;
    public GraphNode TargetNode;
    
    void OnEnable () 
    {
        _signalBus.Subscribe<ColorBoxSignals.SelectedDestination>(CheckDestinationStatus);
    }

    void OnDisable () 
    {
        _signalBus.Unsubscribe<ColorBoxSignals.SelectedDestination>(CheckDestinationStatus);
    }

    private void Awake()
    {
        // Recalculate only the first grid graph
        var graphToScan = AstarPath.active.data.gridGraph;
        AstarPath.active.Scan(graphToScan);
    }

    private void Start()
    {
        _ai = GetComponent<IAstarAI>();
        CheckDestinationStatus(new ColorBoxSignals.SelectedDestination()
        {
            InstanceID = gameObject.GetInstanceID(),
            NewDestinationTransform = AstarPath.active.GetNearest(transform.position).position
        });
    }
    private void Update ()
    { 
        if(HasDestinationReached()) 
        {
           if (TargetNode != null)
           {
               //Debug.Log($"Agent position is in target Position {_ai.reachedDestination}");
               //sending this to Neighbor Status class
               //Debug.Log("sending this to Neighbor Status class");
               _signalBus.Fire(new ColorBoxSignals.AgentReachedTargetNode()
               {
                   AgentGameObject = gameObject,
                   TargetNode = TargetNode
               });
               //Debug.Log(" Agent Reached destination, Signal Fired and Sent");
               
               //Debug.Log("stopping particle effect");//stopping particle effect
               _signalBus.Fire(new ColorBoxSignals.AgentSelectionStatus()
               {
                   Status = false,
                   InstanceID = 0
               });
           }
           else
           {
               Debug.Log("Target node sent null");
           }
        }
    }
    
    private void CheckDestinationStatus(ColorBoxSignals.SelectedDestination signal)
    {
        Vector3 targetDestinationPosition = signal.NewDestinationTransform;
        var receivedInstanceID = signal.InstanceID;
        var thisGameObjectInstanceID = gameObject.GetInstanceID();
        
        GraphNode currentNode = AstarPath.active.GetNearest (transform.position).node; // Agent's current Node
        target = targetDestinationPosition;
        GraphNode destinationNode = AstarPath.active.GetNearest (target).node;
        TargetNode = destinationNode;
        if (TargetNode == null)
        {
            Debug.Log("Target Node Null");
        }
        if (receivedInstanceID == thisGameObjectInstanceID)
        {
            //check if the node is occupied or not.
            if (NodeOccupancyStatusCheck(destinationNode.NodeIndex))
            {
                _gridNodeInformation.allNodesCustom[currentNode.NodeIndex].ClearingNode(); // clearing the node before leaving
                //play destination area particle effect
                _signalBus.Fire(new ColorBoxSignals.NodeSelection()
                {
                    NodePosition = (Vector3)TargetNode.position  
                });
                //Invoking set destination
                SetDestination((Vector3)destinationNode.position);
                _gridNodeInformation.allNodesCustom[destinationNode.NodeIndex].GetOccupied(gameObject); // Initializing the node with values
                //Checking if agent is reached destination in update
            }
        }
    }

    private void SetDestination(Vector3 targetDestinationPosition)
    {
        if (target != null && _ai != null) _ai.destination = targetDestinationPosition;
        //invoke neighborStatus
    }

    private bool HasDestinationReached()
    {
        //keep checking AI position;
        //Debug.Log($"Agent position now {_ai.position} and Target Position {(Vector3)TargetNode.position}");
        if (TargetNode != null && (_ai.position == (Vector3)TargetNode.position))
        {
            //Debug.Log("Agent Destination Reached");
            return true;
        }
        return false;
    }

    private bool NodeOccupancyStatusCheck(int index)
    {
        var allNodesCustom = _gridNodeInformation.allNodesCustom;
        if (allNodesCustom[index].IsOccupied && allNodesCustom[index].OccupiedBy != null)
        {
            //Debug.Log("occupied");
            return false;
        }
        else
        {
            //Debug.Log("Not Occupied / Empty");
            return true;
        }
    }
    private void NodeIndexNumber( )
    {
        GraphNode node = AstarPath.active.GetNearest (target).node;
        if (node.Walkable)
        {
            Debug.Log($"Node Index {node.NodeIndex}");
        }
    }
    
}
