using OpenCover.Framework.Model;
using Pathfinding;
using UnityEngine;

public abstract class ColorBoxSignals : MonoBehaviour
{
   public class SelectedDestination
   {
      public Vector3 newDestinationTransform;
      public int instanceID;
   }

   public class AgentReachedTargetNode
   {
      public GameObject AgentGameObject;
      public GraphNode targetNode;
   }
   
   //particle system
   public class AgentSelectionStatus
   {
      public bool Status;
      public int instanceID;
   }
   public class NodeSelection
   {
      public Vector3 nodePosition;
   }
}
