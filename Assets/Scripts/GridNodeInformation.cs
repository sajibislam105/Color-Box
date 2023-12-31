using System;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


[DefaultExecutionOrder(-1)]
public class GridNodeInformation : MonoBehaviour
{
    [SerializeField] private List<NodeWrapper> AllNodesCustom = new List<NodeWrapper>();
    private GridGraph _aStarGridGraphData;

    private readonly List<GraphNode> _allNodes = new List<GraphNode>();
    
    public List<NodeWrapper> allNodesCustom
    {
        get { return AllNodesCustom; }
    }

    private void Start()
    {
        AllNodesCustom.Add(new NodeWrapper(null, false, null)); //adding first node as Null to fill the index 0
        _aStarGridGraphData = AstarData.active.data.gridGraph; // it has all information about grid.
        _aStarGridGraphData.GetNodes(GetNode);
    }
    private void GetNode(GraphNode node)
    {
        _allNodes.Add(node);
        AllNodesCustom.Add(new NodeWrapper(node, false, null));
        
        if (_allNodes.Count >= _aStarGridGraphData.CountNodes())
        {
            //Debug.Log("All nodes added");
            //Get player positions and set them to occupied
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
    private GraphNode _graphNode;
    [SerializeField] private int NodeIndexNumber;
    [SerializeField] private bool isOccupied;
    [SerializeField] private GameObject occupiedBy;

    public GraphNode graphNode
    {
        get { return _graphNode; }
    }
    public bool IsOccupied
    {
        get { return isOccupied; }
    }
    public GameObject OccupiedBy
    {
        get { return occupiedBy; }
    }
    
    public NodeWrapper(GraphNode graphNode, bool isOccupied, GameObject occupiedBy)
    {
        _graphNode = graphNode;
        this.isOccupied = isOccupied;
        this.occupiedBy = occupiedBy;
        if (graphNode != null) //setting the first index as a 0
        {
            NodeIndexNumber = graphNode.NodeIndex;    
        }
        else
        {
            NodeIndexNumber = 0;
        }
    }

    public List<GraphNode> GetAllNeighbors()
    {
        //Debug.Log("Get all neighbor called");
        List<GraphNode> neighborNodes = new List<GraphNode>();
        _graphNode.GetConnections((GraphNode neighborNode) =>
        {
            neighborNodes.Add(neighborNode);
            //Debug.Log("Neighbor added to list");
        });
        //Debug.Log("list return");
        return neighborNodes;
    }

    public void GetOccupied(GameObject gameObject)
    {
        isOccupied = true;
        occupiedBy = gameObject;
    }

    public void ClearingNode()
    {
        isOccupied = false;
        occupiedBy = null;
        //Debug.Log("Node Cleared");
    }
}
