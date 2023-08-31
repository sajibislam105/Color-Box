using System;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Sirenix.OdinInspector;

public class GridNodeInformation : SerializedMonoBehaviour
{
    private GridGraph _aStarGridGraphData;
    
    [SerializeField] private List<GraphNode> _allNodes = new List<GraphNode>();
    [SerializeField] private List<NodeWrapper> _allNodesCustom = new List<NodeWrapper>();

    public List<NodeWrapper> AllNodesCustom
    {
        get { return _allNodesCustom; }
    }
    private void Start()
    {
        _allNodesCustom.Add(new NodeWrapper(null, false, null)); //adding first node as Null to fill the index 0
        _aStarGridGraphData = AstarData.active.data.gridGraph; // it has all information about grid.

        _aStarGridGraphData.GetNodes(GetNode);
    }

    private void GetNode(GraphNode node)
    {
        _allNodes.Add(node);
        _allNodesCustom.Add(new NodeWrapper(node, false, null));
        
        if (_allNodes.Count >= _aStarGridGraphData.CountNodes())
        {
            //Debug.Log("All nodes added");
            //TODO Get player positions and set them to occupied
        }
    }
    /*[Button]
    public void LogNeighborOfNode(int index)
    {
        if (index > 0)
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
    }*/
    
    
    
}
[Serializable]
public class NodeWrapper
{
    public GraphNode graphNode;
    public bool isOccupied;
    public GameObject OccupiedBy;

    public NodeWrapper(GraphNode graphNode, bool isOccupied, GameObject occupiedBy)
    {
        this.graphNode = graphNode;
        this.isOccupied = isOccupied;
        this.OccupiedBy = OccupiedBy;
    }

    public List<GraphNode> GetAllNeighbors()
    {
        //Debug.Log("Get all neighbor called");
        List<GraphNode> neighborNodes = new List<GraphNode>();
        graphNode.GetConnections((GraphNode neighborNode) =>
        {
            neighborNodes.Add(neighborNode);
            //Debug.Log("Neighbor added to list");
        });
        //Debug.Log("list return");
        return neighborNodes;
        
    }

    public void GettingOccupied(GameObject gameObject)
    {
        OccupiedBy = gameObject;
    }
    
}
