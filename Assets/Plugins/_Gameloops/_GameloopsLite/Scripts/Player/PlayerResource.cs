using System;
using System.Collections.Generic;
using System.Linq;
using Gameloops.Save;
using Gameloops.Save.Demo;
using UnityEngine;

namespace Gameloops.Player
{
    public class PlayerResource : MonoBehaviour, ISaveableEntity<PlayerEntity>
    {
        [Header("Data")]
        [SerializeField] protected List<ResourceData> possibleResources;
        [SerializeField] private int defaultResourceIndex = 0;
        public List<ResourceData> PossibleResources => possibleResources;
        protected Dictionary<ResourceData, float> _resources = new Dictionary<ResourceData, float>();
        public Dictionary<ResourceData, float> Resources
        {
            get => _resources;
            set
            {
                _resources = value;
                ForceClampResources();
            }
        }
        public ResourceData DefaultResource => possibleResources[defaultResourceIndex];
        public Action<Dictionary<ResourceData, float>> OnResourceUpdated;
        public Action<ResourceData, float> OnGainUnit;
        public Action OnSave { get; set; }
        public virtual void GainResourceDefault(float value)
        {
            GainResource(possibleResources[defaultResourceIndex], value);
        }
        public virtual void GainResource(ResourceData resourceData, float value)
        {
            if (Resources.ContainsKey(resourceData))
                Resources[resourceData] += value;
            else
                Resources.Add(resourceData, value);
            
            if (Math.Abs(value - 1) < .1f) OnGainUnit?.Invoke(resourceData, Resources[resourceData]);
            ResourceUpdateEvent();
        }
        public void ReduceResourceBy(ResourceData requiredResource, float reduceBy)
        {
            Resources[requiredResource] -= reduceBy;
            Resources[requiredResource] =
                Mathf.Max(Resources[requiredResource], 0f);
            ResourceUpdateEvent();
        }
        
        public virtual void ResourceUpdateEvent()
        {
            ForceClampResources();
            OnResourceUpdated?.Invoke(_resources);
            OnSave?.Invoke();
        }
        
        public void SetEntity(PlayerEntity entity)
        {
            foreach (var entityResource in entity.resources)
            {
                var resourceData = possibleResources.FirstOrDefault(_ => _.resourceId == entityResource.resourceId);
                if(resourceData == null) continue;
                if (!_resources.ContainsKey(resourceData)) _resources[resourceData] = 0;
                _resources[resourceData] = entityResource.resourceValue;
            }
        }

        public PlayerEntity GetEntity()
        {
            var resourceEntities = new List<Save.Demo.ResourceEntity>();
            foreach (var resource in _resources)
            {
                resourceEntities.Add(new ResourceEntity(resource.Key.resourceId, resource.Value));
            }

            return new PlayerEntity(resourceEntities);
        }

        public PlayerEntity GetEntityDefault()
        {
            return GetEntity();
        }
        private void ForceClampResources()
        {
            var tempResources = new Dictionary<ResourceData, float>(_resources);
            foreach (var resource in _resources)
            {
                tempResources[resource.Key] = Mathf.Max(resource.Value, 0f);
            }

            _resources = tempResources;
        }

        [ContextMenu("Log Resource Values")]
        public void LogResources()
        {
            foreach (var resource in _resources)
            {
                Debug.Log($"{resource.Key.name}: {resource.Value}");
            }
        }
        
        #if UNITY_EDITOR
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.G))
                foreach (var possibleResource in possibleResources)
                    GainResource(possibleResource, 5);
            if(Input.GetKeyDown(KeyCode.H))
                foreach (var possibleResource in possibleResources)
                    GainResource(possibleResource, 50);
        }
        #endif
        
    }
    
}