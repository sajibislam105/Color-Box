using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameloops.Economy
{
    [CreateAssetMenu(menuName = "Gameloops/Game Economy/Cost Calculator/Relative Linear")]
    public class CostCalculatorRelativeLinear : CostCalculator
    {
        [InfoBox("It follows another calculator with a defined offset directly")]
        [SerializeField] private int levelOffset = 14;
        [InlineEditor(InlineEditorModes.GUIAndPreview, Expanded = true)] [PropertyOrder(1)]
        [SerializeField] private CostCalculator source;
        
        public override List<LevelCost> GetLevelCosts(Vector2Int levels)
        {
            var sourceCosts = source.GetLevelCosts(new Vector2Int(levels.x, levels.y + levelOffset));
            var levelCosts = new List<LevelCost>();
            for (int i = levels.x; i <= levels.y; i++)
            {
                var cost = Mathf.CeilToInt(sourceCosts.FirstOrDefault(_ => _.level == (i-1)+levelOffset).cost);
                levelCosts.Add(new LevelCost(i, cost));
            }

            return levelCosts;
        }
    }
}