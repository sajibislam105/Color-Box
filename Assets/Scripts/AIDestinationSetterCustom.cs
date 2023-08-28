using Pathfinding;
using Pathfinding.RVO;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;


//[UniqueComponent(tag = "ai.destination")]
public class AIDestinationSetterCustom : MonoBehaviour
{
    [SerializeField] private  Vector3 target;
    
    [Inject] private SignalBus _signalBus;
    
    IAstarAI ai;
    
    IAgent iAgent;

    private Vector3 _destinationPosition;

    void OnEnable () 
    {
        ai = GetComponent<IAstarAI>();
        // Update the destination right before searching for a path as well.
        // This is enough in theory, but this script will also update the destination every
        // frame as the destination is used for debugging and may be used for other things by other
        // scripts as well. So it makes sense that it is up to date every frame.
        _signalBus.Subscribe<ColorBoxSignals.SendNewDestinationToAiSignal>(SetDestination);
        if (ai != null) ai.onSearchPath += Update;
    }

    void OnDisable () 
    {
        _signalBus.Unsubscribe<ColorBoxSignals.SendNewDestinationToAiSignal>(SetDestination);
        if (ai != null) ai.onSearchPath -= Update;
    }

    private void Start()
    {
        
        ai.destination = transform.position;
        
        
    }
    void Update ()
    {
       //if (target != null && ai != null) ai.destination = target;
      
    }


    private void SetDestination(ColorBoxSignals.SendNewDestinationToAiSignal sendNewDestinationToAiSignalTransform)
    {
        Vector3 position = sendNewDestinationToAiSignalTransform.newDestinationTransform;
        var rcvdInstanceID = sendNewDestinationToAiSignalTransform.instanceID;
        var thisId =gameObject.GetInstanceID();
        if (rcvdInstanceID == thisId)
        {
            target = position;
            if (target != null && ai != null) ai.destination = target;
            //NodeIndex();
        }
        else
        {
            //Debug.Log("different instance Id");
        }

        var count = iAgent.NeighbourCount;
        Debug.Log(count);
    }

    [Button]
    void NodeIndex( )
    {
        //Find the closest node to this GameObject's position
        GraphNode node = AstarPath.active.GetNearest (target).node;
        if (node.Walkable)
        {
           // Debug.Log("Node Position: " + node.position + " Node Index: "+ node.NodeIndex);
        }
    }
}
