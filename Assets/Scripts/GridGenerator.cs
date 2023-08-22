using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private GridCellScript gridCellPrefab;
    [SerializeField] private int width;
    [SerializeField] private int height;
    
    //for runtime instantiate
    [Inject] private DiContainer _container;
    //for signals/action
    //[Inject] private SignalBus _signalBus;

    private const float CellSize = 1f;
    private const float SpaceBetweenCell = 0.1f;
    private List<GridCellScript> _gridCellObjectsList;

    public List<GridCellScript> GridCellObjectList
    {
        get { return _gridCellObjectsList; }
        private set { value = _gridCellObjectsList; }
    }

    private void Awake()
    {
        _gridCellObjectsList = new List<GridCellScript>();
        GenerateGrid();
    }
    private void GenerateGrid()
    {
        var number = 0;
        float startX = -(width - 1) / 2f; // Calculate the starting X position
        float startY = -(height - 1) / 2f; // Calculate the starting Y position

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPosition = new Vector3(
                    (startX + x) * (CellSize + SpaceBetweenCell),
                    gameObject.transform.position.z,
                    (startY + y) * (CellSize + SpaceBetweenCell)
                );

                var gridCellObject = _container.InstantiatePrefab(gridCellPrefab.gameObject).GetComponent<GridCellScript>();
                var gridCellObjectTransform = gridCellObject.transform;
                gridCellObjectTransform.position = worldPosition;
                gridCellObjectTransform.rotation = Quaternion.identity;
                if (gridCellObject != null)
                {
                    _gridCellObjectsList.Add(gridCellObject);
                }
                gridCellObject.gameObject.transform.parent = gameObject.transform;
                gridCellObject.gameObject.name = "Grid Cell " + number;
                number++;
            }
        }
        //transform.Translate(transform.position.x, transform.position.y, 0);
        //transform.DOScale(0.6f, 0.01f).SetEase(Ease.Linear);
    }
}
