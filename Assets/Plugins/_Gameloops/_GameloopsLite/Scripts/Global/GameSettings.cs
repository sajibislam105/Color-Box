using System;
using UnityEngine;

namespace Gameloops
{
    [CreateAssetMenu(menuName = "Gameloops/New GameSettings")]
    public class GameSettings: ScriptableObject
    {
        public ModuleSettings moduleSettings;
    }

    [Serializable]
    public class ModuleSettings
    {
        public bool useAnalytics = true;
    }
}