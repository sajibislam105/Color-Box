using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Zenject;

public class NeighborStatus : MonoBehaviour
{
    [Inject] private SignalBus _signalBus;
    [Inject] private GridNodeInformation _gridNodeInformation;
    private AIDestinationSetterCustom _aiDestinationSetterCustom;
    [SerializeField] private List<NodeWrapper> NeighborNode = new List<NodeWrapper>();
    [SerializeField] private List<GraphNode> _neighGraphNodes = new List<GraphNode>();
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
        int agentLayer = signal.AgentGameObject.layer;
        int DefaultLayerName = LayerMask.NameToLayer("Default");

        if (signal.AgentGameObject == gameObject && ( agentLayer != DefaultLayerName ))
        {
            /*var gridGraph = AstarPath.active.data.gridGraph;
            LayerMask currentHeightMask = gridGraph.collision.heightMask;
            // Define the layer you want to remove (e.g., "MyLayer" - replace with your layer's name or index)
            int layerToRemove = LayerMask.NameToLayer("Default");
            // Use a bitwise AND operation with the inverted mask of the layer to remove it
            currentHeightMask &= ~(1 << layerToRemove);
            // Assign the modified height mask back to the graph
            gridGraph.collision.heightMask = currentHeightMask;
            gridGraph.Scan();
            Debug.Log("layer removed here");*/
            var agentNode = signal.TargetNode;
            if (_aiDestinationSetterCustom.TargetNode == null)
            {
                Debug.LogWarning("No target node found!");
                return;
            }
            var currentAgentIndexNode = _aiDestinationSetterCustom.TargetNode.NodeIndex;
            
            //Debug.Log("Before Getting ALl Neighbors");
            //Debug.Log($"Received Agent node index: {AgentNode.NodeIndex} and current agent {CurrentAgentIndexNode} position");
            if (agentNode.NodeIndex == currentAgentIndexNode)
            {
                var allNodesCustom = _gridNodeInformation.allNodesCustom;
                //Debug.Log("All Nodes Custom Count: " + allNodesCustom.Count);

                _neighGraphNodes = allNodesCustom[agentNode.NodeIndex].GetAllNeighbors();
                List<NodeWrapper> _neighGraphNodesWithNodeWrapper = new List<NodeWrapper>(); 
                Debug.Log($"Count of neighbor get all {_neighGraphNodes.Count}");
                foreach (var neighGraphNode in _neighGraphNodes)
                {
                    Debug.Log(neighGraphNode.NodeIndex);
                    var neighborNodeWrapper = allNodesCustom[neighGraphNode.NodeIndex];
                    _neighGraphNodesWithNodeWrapper.Add(neighborNodeWrapper);
                }
                
                foreach (var neighGraphNode in _neighGraphNodesWithNodeWrapper)
                {
                    if (neighGraphNode.IsOccupied && neighGraphNode.OccupiedBy != null)
                    {
                        Debug.Log($"Neighbor is occupied");
                        //While Neighbor Node is occupied by an Agent. 
                        var occupiedByGameObject = neighGraphNode.OccupiedBy.GetComponent<Item>();
                        CheckAndMergeColorBox(occupiedByGameObject);
                    }
                    else
                    {
                        Debug.Log($"Neighbor is Empty");
                    }
                }
                
                if (NeighborNode.Count <= 3 )
                {
                    Debug.Log($"Agent Node {agentNode.NodeIndex}");
                    
                    Debug.Log("Left NeighborNode: " + allNodesCustom[agentNode.NodeIndex - 1]);
                    NeighborNode.Add(allNodesCustom[agentNode.NodeIndex - 1]);

                    if (allNodesCustom.Count >= agentNode.NodeIndex + 6)
                    {
                        Debug.Log("Up NeighborNode: " + allNodesCustom[agentNode.NodeIndex + 6]);
                        NeighborNode.Add(allNodesCustom[agentNode.NodeIndex + 6]);
                    }
                    else
                    {
                        Debug.Log("No Node. Exist");
                    }

                    Debug.Log("Right NeighborNode: " + allNodesCustom[agentNode.NodeIndex + 1]);
                    NeighborNode.Add(allNodesCustom[agentNode.NodeIndex + 1]);
                    
                    if (allNodesCustom.Count <= agentNode.NodeIndex - 6 )
                    {
                        Debug.Log("Down NeighborNode: " + allNodesCustom[agentNode.NodeIndex - 6]);
                        NeighborNode.Add(allNodesCustom[agentNode.NodeIndex - 6]);        
                    }
                    else
                    {
                        Debug.Log("No Node. Exist");
                    }
                    
                    
                
                    /*NeighborNode.Add(allNodesCustom[currentAgentIndexNode + 1]);
                    NeighborNode.Add(allNodesCustom[currentAgentIndexNode - 1]);
                    NeighborNode.Add(allNodesCustom[currentAgentIndexNode + 6]);
                    NeighborNode.Add(allNodesCustom[currentAgentIndexNode - 6]);*/
                    
                    Debug.Log("Neighbor Node added manually");      
                }
                

                /*var allOccupiedNodes = new List<NodeWrapper>();
                //Debug.Log(allNodesCustom.Count+ " here. Agent node index: " + agentNode.NodeIndex + " agent information: " + allNodesCustom[agentNode.NodeIndex]);
                Debug.Log(allNodesCustom.Count);
                foreach (var node in allNodesCustom)
                {
                    if (node.IsOccupied && (allOccupiedNodes.Count <= 3))
                    {
                        allOccupiedNodes.Add(node);
                    }
                }*/


                //var neighborNodes = allNodesCustom[agentNode.NodeIndex].GetAllNeighbors(); //got neighbor of agent nodes
                //Debug.Log($"Neighbour nodes count: {neighborNodes.Count} and got all neighbor of AgentNode");

                /*var allNeighborNodesOfAgentNodeCustom = new List<NodeWrapper>();
                foreach (var neighborNode in neighborNodes)
                {
                    var nodeWrapper = allNodesCustom[neighborNode.NodeIndex];
                    allNeighborNodesOfAgentNodeCustom.Add(nodeWrapper);
                }*/
                //Debug.Log("Neighbor Checked");

                /*foreach (var customNeighborNodeOfAgent in allNeighborNodesOfAgentNodeCustom)
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
                }*/
            }
            else
            {
                Debug.Log("Empty Agent Node");
            }
        }
        //gameObject.layer uses only integers, but we can turn a layer name into a layer integer using LayerMask.NameToLayer()
        /*int LayerName = LayerMask.NameToLayer("Default");
        gameObject.layer = LayerName;
        var childs = gameObject.GetComponentsInChildren<Transform>();
        foreach (var child in childs)
        {
            int LayerNameChild = LayerMask.NameToLayer("Default");
            child.gameObject.layer = LayerNameChild;
        }
        var gridGraph = AstarPath.active.data.gridGraph;
        gridGraph.Scan();*/
    }

    private void CheckAndMergeColorBox(Item neighborGameObject)
    {
        Debug.Log("Merge Called");
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
                //Debug.Log("Destroyed");

                //remove selected node area vfx
                _signalBus.Fire(new ColorBoxSignals.NodeSelection()
                {
                    NodePosition = Vector3.zero
                });
            }
        }
        else
        {
            //Debug.Log("No LGBT Please !!!");
        }
    }
}
