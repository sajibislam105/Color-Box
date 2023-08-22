using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GridCellOccupancyStatus : MonoBehaviour
{
    [Inject] private GridGenerator _gridGenerator;

    public List<bool> GridCellStatus()
    {
        var status = _gridGenerator.GridCellObjectList;
        List<bool> occupancyStatusList = new List<bool>();
        foreach (GridCellScript gridCellGameObject in status)
        {
            occupancyStatusList.Add(gridCellGameObject.IsOccupied);
        }
        return occupancyStatusList;
    }
}
