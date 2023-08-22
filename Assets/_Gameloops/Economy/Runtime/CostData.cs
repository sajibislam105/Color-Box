using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameloops.Economy
{
    [Serializable]
    public struct LevelCost
    {
        [ReadOnly] [HorizontalGroup("", 0.25f)]
        [HideLabel]
        public int level;
        [HorizontalGroup("")]
        public float cost;

        public LevelCost(int level, float cost)
        {
            this.level = level;
            this.cost = cost;
        }
    }
    
    [CreateAssetMenu(menuName = "Gameloops/Game Economy/New Cost Data")]
    public class CostData : ScriptableObject
    {
        [SerializeField] 
        [InlineEditor(InlineEditorModes.GUIAndPreview, Expanded = true)] [PropertyOrder(1)]
        [OnValueChanged(nameof(UseCalculator))]
        private CostCalculator calculator;
        
        [SerializeField] [PropertyOrder(1)] 
        private Vector2Int levels = new Vector2Int(1, 20);
        
        [SerializeField] [PropertyOrder(1)]
        private AnimationCurve costPerLevelCurve = new AnimationCurve();
        
        [SerializeField] 
        //[TableList(DrawScrollView = true, MaxScrollViewHeight = 200)] 
        [PropertyOrder(3)] 
        [ListDrawerSettings(DraggableItems = false, ShowIndexLabels = false, ShowPaging = true, ShowItemCount = true, HideRemoveButton = true)]
        [InfoBox("Costs broke down as max int value reached", InfoMessageType.Warning, nameof(ValidationLevelCostsFail))]
        private List<LevelCost> levelCosts = new List<LevelCost>();
        
        public CostCalculator Calculator => calculator;
        public Vector2Int Levels => levels;
        public AnimationCurve CostPerLevelCurve => costPerLevelCurve;
        public List<LevelCost> LevelCosts => levelCosts;
        private int brokenValue = -214748400;

        [PropertyOrder(2)] 
        [ShowIf(nameof(RequiresUpdateLevelCosts))]
        [InfoBox("Cost values are overriden")]
        [ButtonGroup(), Button(ButtonSizes.Medium, ButtonStyle.Box, Name = "Recalculate", Icon = SdfIconType.Recycle)] 
        private void UseCalculator()
        {
            levelCosts.Clear();
            levelCosts = calculator.GetLevelCosts(levels);
            SetCurveFromCosts();
        }
        private bool RequiresUpdateLevelCosts()
        {
            var newLevelCosts = calculator.GetLevelCosts(levels);
            var requiresUpdate = (newLevelCosts.Count != levelCosts.Count);
            if (requiresUpdate) return true;
            var searchFor = levels.y;
            for (var i = 0; i < searchFor; i++)
                if (newLevelCosts[i].cost != levelCosts[i].cost)
                {
                    requiresUpdate = true;
                    break;
                }
            return requiresUpdate;
        }
        private bool ValidationLevelCostsFail()
        {
            foreach (var levelCost in levelCosts)
            {
                if (levelCost.cost <= brokenValue)
                {
                    return true;
                }
            }

            return false;
        }
        private void UseCurveValues()
        {
            levelCosts.Clear();
            for (int i = levels.x; i <= levels.y; i++)
            {
                var cost = Mathf.CeilToInt(costPerLevelCurve.Evaluate(i));
                levelCosts.Add(new LevelCost(i, cost));
            }
            SetCurveFromCosts();
        }

        public bool HasCost(int level)
        {
            if (levels.y < level)
            {
                Debug.LogError("Trying to get cost of level that is less than the max defined level on CostData");
                return false;
            }

            if (!levelCosts.Exists(x => x.level == level))
            {
                Debug.LogError("The specified level is not found on CostData");
                return false;
            }
            return true;
        }
        public float GetCostOfLevel(int level)
        {
            return levelCosts.First(x => x.level == level).cost;
        }
        private void SetCurveFromCosts()
        {
            costPerLevelCurve.keys = null;
            foreach (var levelCost in levelCosts)
            {
                costPerLevelCurve.AddKey(levelCost.level, levelCost.cost);
            }
        }

    }
}