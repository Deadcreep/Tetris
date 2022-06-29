Shader "Unlit/SimpleGrid"
{
	Properties
	{
		DisplacementX("DisplacementX",Range(0.0, 1.0)) = 1
		DisplacementY("DisplacementY",Range(0.0, 1.0)) = 1
		Thickness("Thickness",Range(0.0, 1.0)) = 1
		Rows("Rows", int) = 20
		Columns("Columns", int) = 10
		LineColor("LineColor", Color) = (1,1,1,1)
		CellColor("CellColor", Color) = (0,0,0,1)
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" "DisableBatching" = "true"}

		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 localPos : TEXCOORD1;
			};

			float DisplacementX;
			float DisplacementY;
			int Rows;
			int Columns;
			float Thickness;
			float4 CellColor;
			float4 LineColor;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.localPos = v.vertex.xy;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 uFrac = frac((i.localPos - float2(DisplacementX,DisplacementY)) * float2(Columns,Rows));
				return lerp(LineColor, CellColor, (uFrac.x > Thickness && uFrac.y > Thickness));
			}
		ENDCG
		}
	}
}
