
using UnityEngine;
using Zenject;

namespace Pathfinding {
    
    [UniqueComponent(tag = "ai.destination")]
    public class AIDestinationSetterCustom : VersionedMonoBehaviour
    {
        [SerializeField] private  Vector3 target;

        [Inject] private SignalBus _signalBus;
        
        IAstarAI ai;

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
            }
            else
            {
                //Debug.Log("different instance Id");
            }
            
        }
    }
}