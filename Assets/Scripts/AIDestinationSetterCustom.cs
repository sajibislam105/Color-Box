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
        ai = GetComponent<IAstarAI>();
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
