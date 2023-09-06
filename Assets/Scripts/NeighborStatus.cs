using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NeighborStatus : MonoBehaviour
{
    [Inject] private SignalBus _signalBus;
    [Inject] private GridNodeInformation _gridNodeInformation;
    private AIDestinationSetterCustom _aiDestinationSetterCustom;

    private void Awake()
    {
        _aiDestinationSetterCustom = GetComponent<AIDestinationSetterCustom>();
    }
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
        if (signal.AgentGameObject == gameObject)
        {
            var agentNode = signal.targetNode;
            if (_aiDestinationSetterCustom.TargetNode == null)
            {
                Debug.LogWarning("No target node found!");
                return;
            }
            
            var currentAgentIndexNode = _aiDestinationSetterCustom.TargetNode.NodeIndex;
            //Debug.Log($"Received Agent node index: {AgentNode.NodeIndex} and current agent {CurrentAgentIndexNode} position");
            if (agentNode.NodeIndex == currentAgentIndexNode)
            {
                var allNodesCustom = _gridNodeInformation.allNodesCustom;
                //Debug.Log(allNodesCustom.Count+ " here. Agent node index: " + AgentNode.NodeIndex + "agent information " + allNodesCustom[AgentNode.NodeIndex]);

                var neighborNodes =
                    allNodesCustom[agentNode.NodeIndex].GetAllNeighbors(); //got neighbor of agent nodes
                //Debug.Log("Neighbour nodes count: " + neighborNodes.Count + " and got all neighbor of AgentNode");

                var allNeighborNodesOfAgentNodeCustom = new List<NodeWrapper>();
                foreach (var neighborNode in neighborNodes)
                {
                    var nodeWrapper = allNodesCustom[neighborNode.NodeIndex];
                    allNeighborNodesOfAgentNodeCustom.Add(nodeWrapper);
                }
                foreach (var customNeighborNodeOfAgent in allNeighborNodesOfAgentNodeCustom)
                {
                    if (customNeighborNodeOfAgent.IsOccupied && customNeighborNodeOfAgent.OccupiedBy != null)
                    {
                        //Debug.Log($"Neighbor is occupied");
                        //While Neighbor Node is occupied by an Agent. 
                        var occupiedByGameObject = customNeighborNodeOfAgent.OccupiedBy.GetComponent<Item>();
                        CheckAndMergeColorBox(occupiedByGameObject);
                    }
                    else
                    {
                        //Debug.Log($"Neighbor is Empty");
                    }
                }
            }
            else
            {
                Debug.Log("Empty Agent Node");
            }
        }
    }

    private void CheckAndMergeColorBox(Item neighborGameObject)
    {
        //Debug.Log($"Agent Id {_aiDestinationSetterCustom.GetComponent<Item>().ITemId} and Neighbour Id {neighborGameObject.ITemId}");
        if (_aiDestinationSetterCustom.GetComponent<Item>().ItemId != neighborGameObject.ItemId)
        {
            var neighbourAgentNodeWrapper = _gridNodeInformation.allNodesCustom[neighborGameObject.GetComponent<AIDestinationSetterCustom>().TargetNode.NodeIndex];
            if (neighbourAgentNodeWrapper != null)
            {
                //Debug.Log("Index Number of Neighbour Node:" +  NeighbourAgentNodeWrapper.OccupiedBy);
                neighbourAgentNodeWrapper.ClearingNode();
                neighborGameObject.gameObject.SetActive(false);
                Destroy(neighborGameObject.gameObject);
            }
            var agentNodeWrapper = _gridNodeInformation.allNodesCustom[_aiDestinationSetterCustom.TargetNode.NodeIndex];
            if (agentNodeWrapper != null)
            {
                //Debug.Log("Index Number of Agent Node:" +  agentNodeWrapper);
                agentNodeWrapper.ClearingNode();
                gameObject.SetActive(false);
                Destroy(gameObject);    
                
                //remove selected node area vfx
                _signalBus.Fire(new ColorBoxSignals.NodeSelection()
                {
                    nodePosition = Vector3.zero
                });
            }
        }
        else
        {
            //Debug.Log("No LGBT Please !!!");
        }
    }
}
