using Pathfinding;
using UnityEngine;
using Zenject;

public class AIDestinationSetterCustom : MonoBehaviour
{
    [SerializeField] private  Vector3 target;
    
    [Inject] private SignalBus _signalBus;
    [Inject] private GridNodeInformation _gridNodeInformation;
    
    private IAstarAI _ai;

    public IAstarAI AI
    {
        get { return _ai; }
    }
    
    public GraphNode TargetNode;

    void OnEnable () 
    {
        //if (_ai != null) _ai.onSearchPath += Update;
        _signalBus.Subscribe<ColorBoxSignals.SendNewDestinationToAiSignal>(CheckDestinationStatus);
    }

    void OnDisable () 
    {
        //if (_ai != null) _ai.onSearchPath -= Update;
        
        _signalBus.Unsubscribe<ColorBoxSignals.SendNewDestinationToAiSignal>(CheckDestinationStatus);
    }

    private void Start()
    {
        _ai = GetComponent<IAstarAI>();
        _ai.destination = transform.position;
    }
    private void Update ()
    {
       //if (target != null && _ai != null) _ai.destination = target;
       if (CheckDestinationReached())
       {
           if (TargetNode != null)
           {
               //Debug.Log($"Agent position is in target Position {_ai.reachedDestination}");
               //sending this to Neighbor Status class
               //Debug.Log("sending this to Neighbor Status class");
               _signalBus.Fire(new ColorBoxSignals.SendNodeInformationToNeighborStatusSignal()
               {
                   GameObject = gameObject,
                   targetNode = TargetNode
               });
               Debug.Log("Signal Fired and Sent");
           }
           else
           {
               Debug.Log("Target node sent null");
           }
       }
    }
    
    private void CheckDestinationStatus(ColorBoxSignals.SendNewDestinationToAiSignal sendNewDestinationToAiSignalTransform)
    {
        Vector3 targetDestinationPosition = sendNewDestinationToAiSignalTransform.newDestinationTransform;
        var rcvdInstanceID = sendNewDestinationToAiSignalTransform.instanceID;
        var thisGameObjectInstanceID =gameObject.GetInstanceID();
        
        GraphNode currentNode = AstarPath.active.GetNearest (transform.position).node; // current Node Position
        target = targetDestinationPosition;
        GraphNode destinationNode = AstarPath.active.GetNearest (target).node;
        TargetNode = destinationNode;
        if (TargetNode == null)
        {
            Debug.Log("Target Node Null");
        }
        if (rcvdInstanceID == thisGameObjectInstanceID)
        {
            //check if the node is occupied or not.
            if (NodeOccupancyStatusCheck(destinationNode.NodeIndex))
            {
                _gridNodeInformation.AllNodesCustom[destinationNode.NodeIndex].GettingOccupied(null);
                _gridNodeInformation.AllNodesCustom[currentNode.NodeIndex].isOccupied = false; // when leaving the node
                //Invoking set destination
                SetDestination((Vector3)destinationNode.position);
                _gridNodeInformation.AllNodesCustom[destinationNode.NodeIndex].isOccupied = true;
                _gridNodeInformation.AllNodesCustom[destinationNode.NodeIndex].GettingOccupied(gameObject);
                
                //Checking if agent is reached destination in update
            }
        }
    }

    private void SetDestination(Vector3 targetDestinationPosition)
    {
        if (target != null && _ai != null) _ai.destination = targetDestinationPosition;
        //invoke neighborStatus
    }

    private bool CheckDestinationReached()
    {
        //keep checking AI position;
        //Debug.Log($"Agent position now {_ai.position} and Target Position {(Vector3)TargetNode.position}");
        if (TargetNode != null && (_ai.position == (Vector3)TargetNode.position))
        {
            Debug.Log("Agent Destination Reached");
            return true;
        }
        return false;
    }

    private bool NodeOccupancyStatusCheck(int index)
    {
        var allNodesCustom = _gridNodeInformation.AllNodesCustom;
        if (allNodesCustom[index].isOccupied)
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
