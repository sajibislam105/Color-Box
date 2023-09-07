using System;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class NeighborCheck : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private GridNodeInformation _gridNodeInformation;
        
        private void OnEnable()
        {
            _signalBus.Subscribe<ColorBoxSignals.AgentReachedTargetNode>(NeighborNodesOccupancyCheck);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<ColorBoxSignals.AgentReachedTargetNode>(NeighborNodesOccupancyCheck);
        }

        private void NeighborNodesOccupancyCheck(ColorBoxSignals.AgentReachedTargetNode signal)
        {
            if (signal.AgentGameObject != gameObject) return;
            
            Debug.Log($"Agent {signal.AgentGameObject.name} ReachedTargetNode {signal.TargetNode}");
            
            //Whatever code i right, it directly checks neighbor
            var agents = FindObjectsOfType<Item>();
            foreach (var agent in agents)
            {
                if (agent.gameObject != signal.AgentGameObject)
                {
                    var distanceToOtherAgent = Vector3.Distance(agent.transform.position, signal.AgentGameObject.transform.position);
                    var nodeSize = AstarPath.active.data.gridGraph.nodeSize;
                    var isNeighbor = Math.Abs(distanceToOtherAgent - nodeSize) < 0.05f;
                    if (isNeighbor)
                    {
                        var differentItemId = agent.ItemId != signal.AgentGameObject.GetComponent<Item>().ItemId;
                        if (differentItemId)
                        {
                            Debug.Log("<color = 'red'>Whew!!!!! MATCH FOUND!!!</color>");
                        }
                    }
                }
            }
        }
    }
}