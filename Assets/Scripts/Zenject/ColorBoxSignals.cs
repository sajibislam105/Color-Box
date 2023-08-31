using Pathfinding;
using UnityEngine;

public abstract class ColorBoxSignals : MonoBehaviour
{
   public class SendNewDestinationToAiSignal
   {
      public Vector3 newDestinationTransform;
      public int instanceID;
   }

   public class SendNodeInformationToNeighborStatusSignal
   {
      public GameObject GameObject;
      public GraphNode targetNode;
   }
}
