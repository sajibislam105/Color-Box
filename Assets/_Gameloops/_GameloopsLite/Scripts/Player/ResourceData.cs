using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameloops.Player
{
    [CreateAssetMenu(fileName = "ResourceData", menuName = "Gameloops/New ResourceData", order = 0)]
    public class ResourceData : ScriptableObject
    {
        public int resourceId;
        public Sprite unitPreviewSprite;
        public GameObject unitPreviewObject;
        public ResourceCollectible unitCollectibleObject;
    }
}