using Pathfinding;
using Pathfinding.RVO;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;


public class AIDestinationSetterCustom : MonoBehaviour
{
    [SerializeField] private  Vector3 target;
    
    [Inject] private SignalBus _signalBus;
    
    IAstarAI ai;
    
    IAgent iAgent;

    private Vector3 _destinationPosition;

    
    void OnEnable () 
    {
<<<<<<< Updated upstream
        ai = GetComponent<IAstarAI>();
        _signalBus.Subscribe<ColorBoxSignals.SendNewDestinationToAiSignal>(SetDestination);
        if (ai != null) ai.onSearchPath += Update;
=======
        //if (_ai != null) _ai.onSearchPath += Update;
        _signalBus.Subscribe<ColorBoxSignals.SelectedDestination>(CheckDestinationStatus);
>>>>>>> Stashed changes
    }

    void OnDisable () 
    {
<<<<<<< Updated upstream
        _signalBus.Unsubscribe<ColorBoxSignals.SendNewDestinationToAiSignal>(SetDestination);
        if (ai != null) ai.onSearchPath -= Update;
=======
        //if (_ai != null) _ai.onSearchPath -= Update;
        
        _signalBus.Unsubscribe<ColorBoxSignals.SelectedDestination>(CheckDestinationStatus);
    }

    private void OnDestroy()
    {
        //_gridNodeInformation.AllNodesCustom[TargetNode.NodeIndex].ClearingNodeOccupiedObject();
>>>>>>> Stashed changes
    }

    private void Start()
    {
<<<<<<< Updated upstream
=======
        _ai = GetComponent<IAstarAI>();
        // _ai.destination = transform.position;
        // SetDestination(transform.position);
        
        
        
        CheckDestinationStatus(new ColorBoxSignals.SelectedDestination()
        {
            instanceID = gameObject.GetInstanceID(),
            newDestinationPosition = AstarPath.active.GetNearest (transform.position).position
        });
    }
    private void Update ()
    {
       //if (target != null && _ai != null) _ai.destination = target;
       if (HasDestinationReached())
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
               // Debug.Log("Signal TargetNode Reached Fired and Sent");
           }
           else
           {
               Debug.Log("Target node sent null");
           }
       }
    }
    
    private void CheckDestinationStatus(ColorBoxSignals.SelectedDestination signal)
    {
        Vector3 targetDestinationPosition = signal.newDestinationPosition;
        var rcvdInstanceID = signal.instanceID;
        var thisGameObjectInstanceID =gameObject.GetInstanceID();
>>>>>>> Stashed changes
        
        ai.destination = transform.position;
        
        
    }
    void Update ()
    {
       //if (target != null && ai != null) ai.destination = target;
      
    }

<<<<<<< Updated upstream
=======
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
>>>>>>> Stashed changes

    private void SetDestination(ColorBoxSignals.SendNewDestinationToAiSignal sendNewDestinationToAiSignalTransform)
    {
        Vector3 position = sendNewDestinationToAiSignalTransform.newDestinationTransform;
        var rcvdInstanceID = sendNewDestinationToAiSignalTransform.instanceID;
        var thisId =gameObject.GetInstanceID();
        if (rcvdInstanceID == thisId)
        {
            target = position;
            if (target != null && ai != null) ai.destination = target;
        }
        else
        {
        }

        var count = iAgent.NeighbourCount;
        Debug.Log(count);
    }

    [Button]
    void NodeIndex( )
    {
        GraphNode node = AstarPath.active.GetNearest (target).node;
        if (node.Walkable)
        {
        }
    }
}
