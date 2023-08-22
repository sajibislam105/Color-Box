using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameloops.Economy
{
    public class SampleUseEconomy : MonoBehaviour
    {
        [SerializeField] private CostData costData;
        [SerializeField] private int costOfLevel = 3;

        private void Start()
        {
            if (costData.HasCost(costOfLevel))
            {
                Debug.Log($"The cost of the level {costOfLevel} is {costData.GetCostOfLevel(costOfLevel)}");
            }
            else
            {
                Debug.LogWarning("The cost of the specified level is not defined");
            }
        }
    }
}