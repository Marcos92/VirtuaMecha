Shader "CustomShaders/MyFirstShader"
{
		Properties
		{
			_Color("Main Color", Color) = (1,1,1,1)
		}
			SubShader
			{
				Pass
			{
			Tags{ "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma multi_compile_fwdbase
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
			};

			uniform fixed4 _Color;
			uniform fixed4 _LightColor0;
			
			v2f vert(appdata IN)
			{
				v2f OUT;
				float3 n = normalize(mul(float4(IN.normal, 0.0), _World2Object).xyz);

				OUT.normal = n;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				return OUT;
			}

			fixed4 frag(v2f IN) : Color
			{
				float3 l = normalize(_WorldSpaceLightPos0.xyz);
				float3 n = IN.normal;

				return _Color * max(dot(n,l), 0.0);
			}
			ENDCG
		}
	}
}
