using Pathfinding;
using UnityEngine;
using Zenject;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject gridCellPrefab;
    
    [Inject] private DiContainer _container;
    private GridGraph _gridGraph;

    private float _width;
    private float _height;
    private float _cellSize;
    private const float SpaceBetweenCell = 0.025f;

    void Awake()
    {
        _gridGraph = AstarPath.active.data.gridGraph;
        _width = _gridGraph.width;
        _height = _gridGraph.depth;
        _cellSize = _gridGraph.nodeSize;
        //Debug.Log($"Width {_width}, height {_height}, node size {_cellSize}");
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        var number = 0;
        float startX = -(_width - 1) / 2f; // Calculate the starting X position
        float startY = -(_height - 1) / 2f; // Calculate the starting Y position

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector3 worldPosition = new Vector3((startX + x) * (_cellSize + SpaceBetweenCell), 0,
                    (startY + y) * (_cellSize + SpaceBetweenCell));
                
                // Instantiate a grid cell from the prefab using Zenject
                var gridCellObject = _container.InstantiatePrefab(gridCellPrefab.gameObject);
                var gridCellObjectTransform = gridCellObject.transform;
                gridCellObjectTransform.position = worldPosition;
                gridCellObjectTransform.rotation = Quaternion.identity;
                
                gridCellObject.gameObject.transform.parent = gameObject.transform;
                gridCellObject.gameObject.name = "Grid Cell UI " + number;
                number++;
            }
        }
    }
}