using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Pinwheel.Griffin
{
    [CustomEditor(typeof(GTerrainTest2021))]
    public class GTerrainTest2021Inspector : Editor
    {
        private GTerrainTest2021 instance;
        private void OnEnable()
        {
            instance = target as GTerrainTest2021;
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            GEditorCommon.Header("Render");
            instance.planeMesh = EditorGUILayout.ObjectField("Plane Mesh", instance.planeMesh, typeof(Mesh), false) as Mesh;
            instance.previewMaterial = EditorGUILayout.ObjectField("Preview Material", instance.previewMaterial, typeof(Material), false) as Material;
            instance.waterScale = EditorGUILayout.FloatField("Water Scale", instance.waterScale);

            GEditorCommon.Header("Source Data");
            instance.sourceTerrain = EditorGUILayout.ObjectField("Source Terrian", instance.sourceTerrain, typeof(GStylizedTerrain), true) as GStylizedTerrain;
            instance.terrainSize = GEditorCommon.InlineVector3Field("Terrain Size", instance.terrainSize);
            if (GUILayout.Button("Init"))
            {
                instance.Init();
            }


            GEditorCommon.Header("Simulation");
            instance.erosionShader = EditorGUILayout.ObjectField("Erosion Shader", instance.erosionShader, typeof(ComputeShader), false) as ComputeShader;
            instance.waterSourceAmount = EditorGUILayout.FloatField("Water Source", instance.waterSourceAmount);
            instance.waterSourceMultiplier = EditorGUILayout.Slider(" ", instance.waterSourceMultiplier, 0f, 2f);
            instance.rainRate = EditorGUILayout.FloatField("Rain Rate", instance.rainRate);
            instance.rainRateMultiplier = EditorGUILayout.Slider(" ", instance.rainRateMultiplier, 0f, 2f);
            instance.flowRate = EditorGUILayout.FloatField("Flow Rate", instance.flowRate);
            instance.flowRateMultiplier = EditorGUILayout.Slider(" ", instance.flowRateMultiplier, 0f, 2f);
            instance.erosionRate = EditorGUILayout.FloatField("Erosion Rate", instance.erosionRate);
            instance.erosionRateMultiplier = EditorGUILayout.Slider(" ", instance.erosionRateMultiplier, 0f, 2f);
            instance.evaporationRate = EditorGUILayout.FloatField("Evaporation Rate", instance.evaporationRate);
            instance.evaporationRateMultiplier = EditorGUILayout.Slider(" ", instance.evaporationRateMultiplier, 0f, 2f);
            instance.depositAngle = EditorGUILayout.FloatField("Deposit Angle", instance.depositAngle);
            instance.maskMap = GEditorCommon.InlineTexture2DField("Mask Map", instance.maskMap);

            instance.iteration = EditorGUILayout.IntField("Iteration", instance.iteration);
            instance.stepPerFrame = EditorGUILayout.IntField("Step Per Frame", instance.stepPerFrame);
            EditorGUILayout.LabelField("Remained Steps", instance.remainedStep.ToString());

            if (GUILayout.Button("Start Simulate"))
            {
                instance.BeginSimulate();
            }

            if (GUILayout.Button("Stop Simulate"))
            {
                instance.StopSimulate();
            }

            if (GUILayout.Button("Step"))
            {
                instance.remainedStep = 1;
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorSceneManager.MarkAllScenesDirty();
            }
        }

        public override bool RequiresConstantRepaint()
        {
            return true;
        }
    }
}
