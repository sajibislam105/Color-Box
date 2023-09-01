using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int itemId;
    
    private Renderer ItemColorRenderer;
    private Transform _transform;
    public Color MaterialColor { get; private set; }
    public int ITemId  { get;}
    private void Awake()
    {
        ItemColorRenderer = GetComponentInChildren<Renderer>();
        _transform = GetComponent<Transform>();
        MaterialColor = ItemColorRenderer.material.color;
        //Debug.Log($"This game object id {itemId}, name {gameObject.name}  and color is {MaterialColor}");
    }
}
