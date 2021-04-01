Shader "PreviewTerrain2021"
{
	Properties
	{
		_TessellationLevel ("Tessellation Level", Int) = 1
		_DisplacementStrength ("Displacement Strength", Float) = 0
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex TessVertexProgram
			#pragma fragment FragmentProgram
			#pragma hull HullProgram
			#pragma domain DomainProgram
			#pragma target 4.6
			#include "UnityCG.cginc"

			uniform float _TessellationLevel;
			uniform float _DisplacementStrength;
			
			//StructuredBuffer<float> _HeightData;
			//StructuredBuffer<float> _WaterData;
			uniform sampler2D _SimulationData;
			uniform float3 _TerrainSize;
			uniform float _WaterScale;

			struct appdata
			{
				float4 positionOS: POSITION;
				float2 uv: TEXCOORD0;
				float3 normalOS: NORMAL;
				float4 tangentOS: TANGENT;
			};
			
			struct v2f
			{
				float4 positionCS: SV_POSITION;
				float2 uv: TEXCOORD0;
				float3 normalWS: NORMAL;
				float3 tangentWS: TANGENT;
				float3 binormalWS: TEXCOORD1;
				float3 positionWS: TEXCOORD2;
			};

			struct TessellationFactor
			{
				float edge[3]: SV_TessFactor;
				float inside: SV_InsideTessFactor;
			};

			struct TessellationControlPoint
			{
				float4 positionOS: INTERNALTESSPOS;
				float2 uv: TEXCOORD0;
				float3 normalOS: NOMRAL;
				float4 tangentOS: TANGENT;
			};

			int To1DIndex(int x, int z, int width)
			{
				return z * width + x;
			}

			float GetValueBilinear(StructuredBuffer < float > buffer, float2 uv)
			{
				float value = 0;
				float2 pixelCoord = float2(lerp(0, _TerrainSize.x - 1, uv.x), lerp(0, _TerrainSize.z - 1, uv.y));
				//apply a bilinear filter
				int xFloor = floor(pixelCoord.x);
				int xCeil = ceil(pixelCoord.x);
				int yFloor = floor(pixelCoord.y);
				int yCeil = ceil(pixelCoord.y);

				float f00 = buffer[To1DIndex(xFloor, yFloor, _TerrainSize.x)];
				float f01 = buffer[To1DIndex(xFloor, yCeil, _TerrainSize.x)];


				float f10 = buffer[To1DIndex(xCeil, yFloor, _TerrainSize.x)];
				float f11 = buffer[To1DIndex(xCeil, yCeil, _TerrainSize.x)];

				float2 unitCoord = float2(pixelCoord.x - xFloor, pixelCoord.y - yFloor);

				value = f00 * (1 - unitCoord.x) * (1 - unitCoord.y) + f01 * (1 - unitCoord.x) * unitCoord.y + f10 * unitCoord.x * (1 - unitCoord.y) + f11 * unitCoord.x * unitCoord.y;

				return value;
			}

			TessellationControlPoint TessVertexProgram(appdata v)
			{
				TessellationControlPoint o;
				o.positionOS = v.positionOS;
				o.uv = v.uv;
				o.normalOS = v.normalOS;;
				o.tangentOS = v.tangentOS;
				return o;
			}

			[UNITY_domain("tri")]
			[UNITY_outputcontrolpoints(3)]
			[UNITY_outputtopology("triangle_cw")]
			[UNITY_partitioning("integer")]
			[UNITY_patchconstantfunc("PatchConstantFunction")]
			TessellationControlPoint HullProgram(InputPatch < TessellationControlPoint, 3 > patch, uint id: SV_OutputControlPointID)
			{
				return patch[id];
			}

			TessellationFactor PatchConstantFunction(InputPatch < TessellationControlPoint, 3 > patch)
			{
				TessellationFactor f;
				f.edge[0] = _TessellationLevel;
				f.edge[1] = _TessellationLevel;
				f.edge[2] = _TessellationLevel;
				f.inside = _TessellationLevel;
				return f;
			}

			v2f VertexProgram(appdata v)
			{
				v2f o;
				
				float4 simData = tex2Dlod(_SimulationData, float4(v.uv, 0,0));
				float height = simData.r+simData.a;
				v.positionOS.y = height;

				o.positionCS = UnityObjectToClipPos(v.positionOS);
				o.uv = v.uv;
				o.normalWS = mul(unity_ObjectToWorld, v.normalOS.xyz);
				o.tangentWS = mul(unity_ObjectToWorld, v.tangentOS.xyz);
				o.binormalWS = normalize(cross(o.normalWS, o.tangentWS) * v.tangentOS.w);
				o.positionWS = mul(unity_ObjectToWorld, float4(v.positionOS.xyz, 1));
				return o;
			}

			#define DOMAIN_INTEPOLATE(field) o.field = \
					patch[0].field * bary.x + \
					patch[1].field * bary.y + \
					patch[2].field * bary.z;

			[UNITY_domain("tri")]
			v2f DomainProgram(TessellationFactor f, OutputPatch < TessellationControlPoint, 3 > patch, float3 bary: SV_DomainLocation)
			{
				appdata o;
				DOMAIN_INTEPOLATE(positionOS);
				DOMAIN_INTEPOLATE(uv);
				DOMAIN_INTEPOLATE(normalOS);
				DOMAIN_INTEPOLATE(tangentOS);

				return VertexProgram(o);
			}

			float3 CalculateDiffuseLighting(float3 normalWS, float3 lightDir, float3 lightColor, float lightIntensity)
			{
				float nDotL = dot(normalWS, -lightDir);
				float atten = max(0.15, nDotL);
				float3 light = atten * lightColor * lightIntensity;
				return light;
			}

			float4 FragmentProgram(v2f i): SV_TARGET
			{
				float3 dpdx = ddx(i.positionWS);
				float3 dpdy = ddy(i.positionWS);
				float3 normalWS = normalize(cross(dpdy, dpdx));
				
				float3 lightDirWS = normalize(_WorldSpaceLightPos0);
				float nDotL = max(0.3, dot(normalWS, lightDirWS));

				float4 terrainColor = float4(0.8, 0.8, 0.8, 1);

				float waterAlpha = tex2D(_SimulationData, i.uv).a/_WaterScale;				

				float4 waterColor = float4(0,0,1,waterAlpha);
				float4 blend = waterColor * waterColor.a + terrainColor * (1 - waterColor.a);

				float4 color = blend * nDotL;
				return color;
			}

			ENDCG

		}
	}
}
