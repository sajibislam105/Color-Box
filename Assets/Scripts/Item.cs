using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int itemId;
    [SerializeField] private string itemName;

    //properties
    public int ItemId => itemId;
    public string ItemName => itemName;
}
