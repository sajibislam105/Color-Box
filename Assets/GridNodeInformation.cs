using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Sirenix.OdinInspector;

public class GridNodeInformation : SerializedMonoBehaviour
{
    private GridGraph _aStarGridGraphData;
    private Dictionary<int, bool> graphInfo;
    void Start()
    {
        graphInfo = new Dictionary<int, bool>(); // bool = status of the node
        _aStarGridGraphData = AstarData.active.data.gridGraph;

        Debug.Log(_aStarGridGraphData.CountNodes());

        List<GraphNode> nodes = new List<GraphNode>();

        /*_aStarGridGraphData.GetNodes(nodes.Add);
        for (int i = 0; i < _aStarGridGraphData.CountNodes(); i++)
        {
            Debug.Log(_aStarGridGraphData);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
