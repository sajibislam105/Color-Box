using System;
using UnityEngine;
using Zenject;

public class NeighborCheck : MonoBehaviour
{
    [Inject] private SignalBus _signalBus;
    
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
        var currentAgent = signal.AgentGameObject;
        var agentNode = signal.TargetNode;

        if (currentAgent != gameObject) return;
        //Debug.Log($"Agent {currentAgent.name} Reached TargetNode {agentNode.NodeIndex}");
        
        //Now checking Agent Neighbor
        var agents = FindObjectsOfType<Item>();
        foreach (var agent in agents)
        {
            if (agent.gameObject != currentAgent)
            {
                var distanceToOtherAgent = Vector3.Distance(agent.transform.position, currentAgent.transform.position);
                var nodeSize = AstarPath.active.data.gridGraph.nodeSize;
                bool isNeighbor = Math.Abs(distanceToOtherAgent - nodeSize) < 0.05f; // < 0.05 is tolerance for floating value
                if (isNeighbor)
                {
                    var neighborAgentItem = agent;
                    CheckAndMerge(neighborAgentItem, currentAgent);
                }
                else
                {
                    //Debug.Log("No Neighbor found");
                }
            }
        }
    }

    private void CheckAndMerge(Item NeighborAgent, GameObject currentAgent)
    {
        var agent = NeighborAgent;
        
        var differentItemId = agent.ItemId != currentAgent.GetComponent<Item>().ItemId;
        if (differentItemId)
        {
            //Debug.Log("<color='blue'>Whew!!!!! MATCH FOUND!!!</color>");
            //Debug.Log($"Neighbor name: {agent.name} and Current Agent name: {currentAgent.name}");
                        
            Destroy(agent.gameObject);
            Destroy(currentAgent);
            //Debug.Log("Merged>");
                        
            //stopping green VFX
            _signalBus.Fire(new ColorBoxSignals.NodeSelection()
            {
                NodePosition = Vector3.zero
            });
            AstarPath.active.data.gridGraph.Scan();
        }
    }
}