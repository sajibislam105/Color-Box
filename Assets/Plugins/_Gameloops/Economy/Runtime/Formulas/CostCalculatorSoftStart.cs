using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameloops.Economy
{
    [CreateAssetMenu(menuName = "Gameloops/Game Economy/Cost Calculator/Soft")]
    public class CostCalculatorSoftStart : CostCalculator
    {
        [InfoBox("It follows a cumulative power curve." +
                 "The cost rise is is more regular compared to exponential. " +
                 "With lower friction, the cost values still change well unlike exponential")]
        [InfoBox("It covers most use cases")]
        [SerializeField] private float baseCost = 20f;
        [SerializeField] private float friction = 0.2f;

        public override List<LevelCost> GetLevelCosts(Vector2Int levels)
        {
            var levelCosts = new List<LevelCost>();
            var lastCost = baseCost;
            levelCosts.Add(new LevelCost(1, lastCost));
            for (int i = levels.x + 1; i <= levels.y; i++)
            {
                var cost = Mathf.CeilToInt(Mathf.Pow(baseCost, friction * i) + lastCost);
                levelCosts.Add(new LevelCost(i, cost));
                lastCost = cost;
            }

            return levelCosts;
        }
    }
}