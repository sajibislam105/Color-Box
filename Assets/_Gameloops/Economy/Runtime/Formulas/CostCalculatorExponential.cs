using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameloops.Economy
{
    [CreateAssetMenu(menuName = "Gameloops/Game Economy/Cost Calculator/Exponential")]
    public class CostCalculatorExponential : CostCalculator
    {
        [InfoBox("It follows exponential curve. Beware of its massive rise")]
        [SerializeField] private float baseCost = 20f;
        [SerializeField] private float friction = 0.2f;
        public override List<LevelCost> GetLevelCosts(Vector2Int levels)
        {
            var levelCosts = new List<LevelCost>();
            var currentCost = baseCost;
            levelCosts.Add(new LevelCost(1, currentCost));
            for (int i = levels.x+1; i <= levels.y; i++)
            {
                var cost = Mathf.CeilToInt(baseCost * Mathf.Exp(friction * i));
                levelCosts.Add(new LevelCost(i, cost));
            }

            return levelCosts;
        }
    }
}