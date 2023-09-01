using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int itemId;
    public int ItemId => itemId;
    private Renderer ItemColorRenderer;
    private Transform _transform;
    public Color MaterialColor { get; private set; }
    private void Awake()
    {
        ItemColorRenderer = GetComponentInChildren<Renderer>();
        _transform = GetComponent<Transform>();
        MaterialColor = ItemColorRenderer.material.color;
        
        //Debug.Log($"This game object {gameObject.name} color is {MaterialColor}");
    }
}
