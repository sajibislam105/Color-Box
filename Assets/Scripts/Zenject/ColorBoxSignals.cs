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

      //Animation
      public class WalkingAnimationSignal
      {
         public bool Remote;
         public int InstanceID;
      }
      
      
      //UI 
      public class LoadEverything { }
      
      public class FirstTappedLevelStart { }
      
      public class LevelComplete { }
      public class LevelFailed { }
      
      public class CompletionProgressBarSignal
      {
         public float ProgressBarFillAmount;
      }

      public class MoveCounter{ }

      public class RemainingMoves
      {
         public int remainingMoves;
      }
      
      public class CoupleMergeCount { }
      
      public class CoinEarned {}
   }
}
