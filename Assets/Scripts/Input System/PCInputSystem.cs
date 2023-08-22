using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class PCInputSystem : MonoBehaviour , IInputSystem
{
    [Inject] private Camera _camera;
    private GameObject _selectedGameObject;
    private void Update()
    {
        InputSystem();
    }

    public void InputSystem()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var hit =CastRay();
            if (hit.HasValue)
            {
                if (hit.Value.collider.gameObject.CompareTag("Player"))
                {
                    _selectedGameObject = hit.Value.collider.gameObject;
                    Debug.Log(_selectedGameObject.name + " Selected");
                }

                if (_selectedGameObject != null)
                {
                    var GridCellGameObject = hit.Value.collider.gameObject; 
                    if (GridCellGameObject.CompareTag("GridCell"))
                    {
                        var gridCellPositionForObjectNewDestination = hit.Value.collider.transform.position;
                        gridCellPositionForObjectNewDestination.y = +0.4f;
                        //check if Cell is occupied or not
                        if (GridCellGameObject.GetComponent<GridCellScript>().IsOccupied == false)
                        {
                            _selectedGameObject.transform.DOMove(gridCellPositionForObjectNewDestination, 1f).SetEase(Ease.Linear);
                            Debug.Log("Reached Destination");
                        }
                        else
                        {
                            Debug.Log("Grid Cell already occupied");
                        }
                    }    
                }
                
            }
            else
            {
                _selectedGameObject = null;
            }
        }
    }

    private RaycastHit? CastRay()
    {
        var mousePositionInScreen = Input.mousePosition;
        Ray ray = _camera.ScreenPointToRay(mousePositionInScreen);
        RaycastHit hit;
        Debug.DrawRay(ray.origin,ray.direction * 20f,Color.red);
        if (Physics.Raycast(ray,out hit))
        {
            return hit;
        }
        return null;
    }
}
