using UnityEngine;

public class GridCellScript : MonoBehaviour
{
    [SerializeField] private bool _isOccupied;
    //private Object occupiedObject; // cause I need the object script mono behaviour not the whole game object

    public bool IsOccupied
    {
        get { return _isOccupied; }
        private set { _isOccupied = value; }
    }
    /*public Object OccupiedObject
    {
        get { return occupiedObject; }
        private set { occupiedObject = value; }
    }*/
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //occupiedObject = other.GetComponent<Object>();
            //Debug.Log("Occupied");
            _isOccupied = true;
        }
     
    }

    private void OnTriggerExit(Collider other)
    {
        _isOccupied = false;
        //occupiedObject = null;
        //Debug.Log("Not Occupied");
    }

}
