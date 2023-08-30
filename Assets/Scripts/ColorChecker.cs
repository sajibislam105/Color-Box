using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ColorChecker : MonoBehaviour
{
    [Inject] private GridNodeInformation _gridNodeInformation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void CheckColor()
    {
        var allNodesCustom = _gridNodeInformation.AllNodesCustom;

        foreach (var nodeWrapper in allNodesCustom)
        {
            
        }
    }
}
