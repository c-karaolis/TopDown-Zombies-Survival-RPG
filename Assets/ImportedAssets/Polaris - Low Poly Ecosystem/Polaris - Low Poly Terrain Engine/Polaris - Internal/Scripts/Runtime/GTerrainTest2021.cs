using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using Random = UnityEngine.Random;

namespace Pinwheel.Griffin
{
    [ExecuteInEditMode]
    public class GTerrainTest2021 : MonoBehaviour
    {
        public Mesh planeMesh;
        public Material previewMaterial;
        public float waterScale;

        public GStylizedTerrain sourceTerrain;
        public Vector3 terrainSize;

        public ComputeShader erosionShader;
        public float waterSourceAmount;
        public float waterSourceMultiplier;
        public float rainRate;
        public float rainRateMultiplier;
        public float flowRate;
        public float flowRateMultiplier;
        public float erosionRate;
        public float erosionRateMultiplier;
        public float evaporationRate;
        public float evaporationRateMultiplier;
        public float depositAngle;

        public Texture2D maskMap;

        public int iteration;
        public int stepPerFrame;

        public int remainedStep;

        public RenderTexture simulationData;
        public RenderTexture waterFlowVHData;
        public RenderTexture waterFlowDiagData;

        private void OnEnable()
        {
            Camera.onPreCull += OnRenderCamera;
            Init();
        }

        private void OnDisable()
        {
            Camera.onPreCull -= OnRenderCamera;

            if (simulationData != null)
            {
                simulationData.Release();
                simulationData = null;
            }
            if (waterFlowVHData != null)
            {
                waterFlowVHData.Release();
                waterFlowVHData = null;
            }
            if (waterFlowDiagData != null)
            {
                waterFlowDiagData.Release();
                waterFlowDiagData = null;
            }
        }

        private void OnRenderCamera(Camera cam)
        {
            if (planeMesh == null || previewMaterial == null)
                return;
            if (cam.cameraType == CameraType.Preview)
                return;

            for (int i = 0; i < stepPerFrame; ++i)
            {
                if (remainedStep > 0)
                {
                    remainedStep -= 1;
                    Simulate();
                }
            }

            previewMaterial.SetTexture("_SimulationData", simulationData);
            previewMaterial.SetVector("_TerrainSize", terrainSize);
            previewMaterial.SetFloat("_WaterScale", waterScale);
            Graphics.DrawMesh(
                planeMesh,
                transform.localToWorldMatrix,
                previewMaterial,
                0,
                cam,
                0,
                null,
                UnityEngine.Rendering.ShadowCastingMode.Off,
                false,
                null,
                UnityEngine.Rendering.LightProbeUsage.Off,
                null);
        }

        public void Init()
        {
            terrainSize = sourceTerrain.TerrainData.Geometry.Size;
            int dimX = (int)terrainSize.x;
            int dimZ = (int)terrainSize.z;

            if (simulationData != null)
            {
                simulationData.Release();
            }
            simulationData = new RenderTexture(dimX, dimZ, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            simulationData.enableRandomWrite = true;
            simulationData.Create();

            if (waterFlowVHData != null)
            {
                waterFlowVHData.Release();
            }
            waterFlowVHData = new RenderTexture(dimX, dimZ, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            waterFlowVHData.enableRandomWrite = true;
            waterFlowVHData.Create();

            if (waterFlowDiagData != null)
            {
                waterFlowDiagData.Release();
            }
            waterFlowDiagData = new RenderTexture(dimX, dimZ, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            waterFlowDiagData.enableRandomWrite = true;
            waterFlowDiagData.Create();

            int initKernel = erosionShader.FindKernel("Init");
            erosionShader.SetVector("_TerrainSize", terrainSize);
            erosionShader.SetInt("_HeightMapResolution", sourceTerrain.TerrainData.Geometry.HeightMapResolution);
            erosionShader.SetTexture(initKernel, "_HeightMap", sourceTerrain.TerrainData.Geometry.HeightMap);
            erosionShader.SetTexture(initKernel, "_SimulationData", simulationData);
            erosionShader.SetTexture(initKernel, "_WaterFlowVHData", waterFlowVHData);
            erosionShader.SetTexture(initKernel, "_WaterFlowDiagData", waterFlowDiagData);

            Texture2D mask = maskMap != null ? maskMap : Texture2D.whiteTexture;
            Vector2 maskResolution = new Vector2(mask.width, mask.height);
            erosionShader.SetTexture(initKernel, "_MaskMap", mask);
            erosionShader.SetVector("_MaskMapResolution", maskResolution);
            erosionShader.SetFloat("_WaterSourceAmount", waterSourceAmount * waterSourceMultiplier);

            int threadGroupX = (dimX + 7) / 8;
            int threadGroupY = 1;
            int threadGroupZ = (dimZ + 7) / 8;

            erosionShader.Dispatch(initKernel, threadGroupX, threadGroupY, threadGroupZ);
        }

        public void BeginSimulate()
        {
            Init();
            remainedStep = iteration;
        }

        public void StopSimulate()
        {
            remainedStep = 0;
        }

        public void Simulate()
        {
            erosionShader.SetFloat("_WaterSourceAmount", waterSourceAmount * waterSourceMultiplier);
            erosionShader.SetFloat("_RainRate", rainRate * rainRateMultiplier);
            erosionShader.SetFloat("_FlowRate", flowRate * flowRateMultiplier);
            erosionShader.SetFloat("_ErosionRate", erosionRate * erosionRateMultiplier);
            erosionShader.SetFloat("_EvaporationRate", evaporationRate * evaporationRateMultiplier);
            erosionShader.SetFloat("_DepositAngle", depositAngle);

            int simulateKernel = erosionShader.FindKernel("Simulate");
            erosionShader.SetVector("_TerrainSize", terrainSize);
            erosionShader.SetTexture(simulateKernel, "_SimulationData", simulationData);
            erosionShader.SetTexture(simulateKernel, "_WaterFlowVHData", waterFlowVHData);
            erosionShader.SetTexture(simulateKernel, "_WaterFlowDiagData", waterFlowDiagData);

            Texture2D mask = maskMap != null ? maskMap : Texture2D.whiteTexture;
            Vector2 maskResolution = new Vector2(mask.width, mask.height);
            erosionShader.SetTexture(simulateKernel, "_MaskMap", mask);
            erosionShader.SetVector("_MaskMapResolution", maskResolution);

            Vector2 randomSeed = Random.insideUnitCircle;
            erosionShader.SetVector("_RandomSeed", randomSeed);

            int dimX = (int)terrainSize.x;
            int dimZ = (int)terrainSize.z;

            int threadGroupX = (dimX + 7) / 8;
            int threadGroupY = 1;
            int threadGroupZ = (dimZ + 7) / 8;

            erosionShader.Dispatch(simulateKernel, threadGroupX, threadGroupY, threadGroupZ);
        }
    }
}

