using UnityEngine;

public class ColorBoxSignals : MonoBehaviour
{
   public class SendNewDestinationToAiSignal
   {
      public Vector3 newDestinationTransform;
      public int instanceID;
   }
}
