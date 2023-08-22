using UnityEngine;
using Zenject;

namespace Gameloops.UI
{
    public class UiFeedback : MonoBehaviour
    {
        [Inject] private HapticManager _hapticManager;

        public void PlayHapticSelection()
        {
            _hapticManager.HapticSelection();
            
        }
    }
}