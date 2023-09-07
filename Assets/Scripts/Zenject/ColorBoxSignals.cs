using Pathfinding;
using UnityEngine;

namespace Zenject
{
   public abstract class ColorBoxSignals : MonoBehaviour
   {
      public class SelectedDestination
      {
         public Vector3 NewDestinationTransform;
         public int InstanceID;
      }

      public class AgentReachedTargetNode
      {
         public GameObject AgentGameObject;
         public GraphNode TargetNode;
      }
   
      //particle system
      public class AgentSelectionStatus
      {
         public bool Status;
         public int InstanceID;
      }
      public class NodeSelection
      {
         public Vector3 NodePosition;
      }
   }
}
