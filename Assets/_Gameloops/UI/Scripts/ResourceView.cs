using System;
using System.Collections.Generic;
using System.Linq;
using Gameloops.Player;
using UnityEngine;
using Zenject;

namespace Gameloops.UI
{
    public class ResourceView : MonoBehaviour
    {
        [Inject] private PlayerResource _playerResource;
        [SerializeField] private ResourcePanel resourcePanelPrefab;
        [SerializeField] private Transform listRoot;
        private List<ResourcePanel> _resourcePanels = new List<ResourcePanel>();

        private void OnEnable()
        {
            _playerResource.OnResourceUpdated += OnResourceUpdate;
        }

        private void OnDisable()
        {
            _playerResource.OnResourceUpdated -= OnResourceUpdate;
        }

        private void Start()
        {
            OnResourceUpdate(_playerResource.Resources);
        }

        private void OnResourceUpdate(Dictionary<ResourceData, float> resource)
        {
            foreach (var resourceValuePair in resource)
            {
                var found = false;
                foreach (var resourcePanel in _resourcePanels.Where(resourcePanel => resourcePanel.ResourceData == resourceValuePair.Key))
                {
                    found = true;
                    resourcePanel.SetResource(resourceValuePair.Value);
                }

                if (found) continue;
                {
                    var resourcePanel = CreateResourcePanel(resourceValuePair.Key);
                    resourcePanel.SetResource(resourceValuePair.Value);
                }
            }
        }

        private ResourcePanel CreateResourcePanel(ResourceData resource)
        {
            var resourcePanel = Instantiate(resourcePanelPrefab, listRoot);
            _resourcePanels.Add(resourcePanel);
            resourcePanel.InitResource(resource);
            resourcePanel.SetResource(0);
            return resourcePanel;
        }

        public Transform GetResourceTransform(ResourceData resourceData)
        {
            foreach (var resourcePanel in _resourcePanels)
            {
                if (resourcePanel.ResourceData == resourceData)
                    return resourcePanel.Pivot;
            }

            var panel = CreateResourcePanel(resourceData);
            return panel.Pivot;
        }
    }
}