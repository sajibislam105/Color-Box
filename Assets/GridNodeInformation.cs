using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Sirenix.OdinInspector;

public class GridNodeInformation : SerializedMonoBehaviour
{
    private GridGraph _aStarGridGraphData;
    
    [SerializeField] private List<GraphNode> _allNodes = new List<GraphNode>();
    [SerializeField] private List<NodeWrapper> _allNodesCustom = new List<NodeWrapper>();

    private void Start()
    {
        _allNodesCustom.Add(new NodeWrapper(null, false));
        _aStarGridGraphData = AstarData.active.data.gridGraph;

        _aStarGridGraphData.GetNodes(GetNode);
    }

    private void GetNode(GraphNode node)
    {
        _allNodes.Add(node);
        _allNodesCustom.Add(new NodeWrapper(node, false));
        
        if (_allNodes.Count >= _aStarGridGraphData.CountNodes())
        {
            Debug.Log("All nodes added");
            //TODO Get player positions and set them to occupied
        }
    }

    [Button]
    public void LogNeighborOfNode(int index)
    {
        var neighbors = _allNodesCustom[index].GetAllNeighbors();
        
        var customNeighborNodes = new List<NodeWrapper>();
        foreach (var neighborNode in neighbors)
        {
            var nodeWrapper = _allNodesCustom[neighborNode.NodeIndex];
            customNeighborNodes.Add(nodeWrapper);
        }
        
        
        foreach (var neighbor in customNeighborNodes)
        {
            Debug.Log("Neighbor found in index " + neighbor.graphNode.NodeIndex);
        }
        
    }
    
    
    
}
[Serializable]
public class NodeWrapper
{
    public GraphNode graphNode;
    public bool isOccupied;

    public NodeWrapper(GraphNode graphNode, bool isOccupied)
    {
        this.graphNode = graphNode;
        this.isOccupied = isOccupied;
    }

    public List<GraphNode> GetAllNeighbors()
    {
        List<GraphNode> neighborNodes = new List<GraphNode>();
        graphNode.GetConnections((GraphNode neighborNode) =>
        {
            neighborNodes.Add(neighborNode);
        });
        return neighborNodes;
    }
}
