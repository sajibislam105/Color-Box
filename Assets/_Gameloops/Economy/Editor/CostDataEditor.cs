using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gameloops.Economy
{
    // [CustomEditor(typeof(CostData))]
    public class CostDataEditor: Editor
    {
        private Vector2 _scroll;
        private Color _defaultBackgroundColor = new Color(0.278f, 0.278f, 0.278f, .5f);

        public override void OnInspectorGUI()
        {
            var costData = (CostData)target;
            
            base.OnInspectorGUI();
            
            if (costData.Calculator == null)
            {
                EditorGUILayout.HelpBox("Add a cost calculator to initialize values", MessageType.Warning);
                return;
            }
            
            if (costData.LevelCosts.Count == 0)
            {
                // if(GUILayout.Button("Initialize")) costData.UseCalculator();
                return;
            }
            
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("Curve Values");
            _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.MaxHeight(300));

            GUI.backgroundColor = Color.gray;
            var allCostsMatch = true;
            for (var i = costData.Levels.x; i <= costData.Levels.y; i++)
            {
                var costOnArray = Mathf.CeilToInt(costData.LevelCosts.FirstOrDefault(_ => _.level == i).cost);
                var costOnCurve = Mathf.CeilToInt(costData.CostPerLevelCurve.Evaluate(i));
                var costMatch = costOnArray == costOnCurve;
                if (!costMatch) allCostsMatch = false;
                GUI.backgroundColor = costMatch ? _defaultBackgroundColor : Color.red;

                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField("Level " + i);
                EditorGUILayout.LabelField(costMatch ? costOnCurve.ToString() : costOnArray + " -> " + costOnCurve);
                EditorGUILayout.EndHorizontal();
            }
            GUI.backgroundColor = _defaultBackgroundColor;
            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();

            if (!allCostsMatch)
            {
                // if (GUILayout.Button("Save")) costData.UseCurveValues();
            }
            // if (GUILayout.Button("Recalculate")) costData.UseCalculator();
            EditorGUILayout.EndHorizontal();
        }
    }
}