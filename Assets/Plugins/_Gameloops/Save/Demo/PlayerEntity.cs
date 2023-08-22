using System;
using System.Collections.Generic;

namespace Gameloops.Save.Demo
{
    [Serializable]
    public struct PlayerEntity
    {
        //public int playerLevel etc other data that you might need
        public List<ResourceEntity> resources;

        public PlayerEntity(List<ResourceEntity> resources)
        {
            this.resources = resources;
        }
    }

    [Serializable]
    public struct ResourceEntity
    {
        public int resourceId;
        public float resourceValue;

        public ResourceEntity(int resourceId, float resourceValue)
        {
            this.resourceId = resourceId;
            this.resourceValue = resourceValue;
        }

    }
}