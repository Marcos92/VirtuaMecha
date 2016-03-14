Shader "CustomShaders/MyFirstDayNightShader"
{
	Properties
	{
		_MainTex ("Day", 2D) = "white" {}
		_NTex("Night", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "LightMode"="ForwardBase" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 texCoord : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 texCoord : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
			};

			sampler2D _MainTex;
			sampler2D _NTex;
			
			v2f vert (appdata IN)
			{
				v2f OUT;
				float3 n = mul(float4(IN.normal, 0.0), _World2Object).xyz;
				OUT.normal = n;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texCoord = IN.texCoord;
				return OUT;
			}
			
			fixed4 frag(v2f IN) : SV_Target
			{
				//fixed4 col = fixed4(IN.texCoord, 0, 1);

				/*fixed4 col = tex2D(_MainTex, IN.texCoord);

				float2 t = IN.texCoord * 20.0;
				if (frac(t.y) > frac(t.x) && frac(t.y) > frac(-t.x)) { col = float4(1, 1, 0, 1); }
				else if (frac(t.y) < frac(t.x) && frac(t.y) < frac(-t.x)) { col = float4(1, 0, 0, 1); }
				else if (frac(t.y) < frac(t.x) && frac(t.y) > frac(-t.x)) { col = float4(0, 0, 1, 1); }
				else if (frac(t.y) > frac(t.x) && frac(t.y) < frac(-t.x)) { col = float4(0, 1, 0, 1); }
				else { discard; }*/

				float3 n = normalize(IN.normal);
				float3 l = normalize(_WorldSpaceLightPos0.xyz);
				float test = max(0.0, dot(n, 1));
				float4 day = tex2D(_MainTex, IN.texCoord);
				float4 night = tex2D(_NTex, IN.texCoord);
				fixed4 col = lerp(night, day, test);

				return col;
			}
			ENDCG
		}
	}
}
