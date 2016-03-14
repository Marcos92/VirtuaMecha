Shader "CustomShaders/MyFirstTextureShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

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
			};

			struct v2f
			{
				float2 texCoord : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texCoord = IN.texCoord;
				return OUT;
			}
			
			fixed4 frag(v2f IN) : SV_Target
			{
				//fixed4 col = fixed4(IN.texCoord, 0, 1);

				fixed4 col = tex2D(_MainTex, IN.texCoord);

				float2 t = IN.texCoord * 20.0;
				if (frac(t.y) > frac(t.x) && frac(t.y) > frac(-t.x)) { col = float4(1, 1, 0, 1); }
				else if (frac(t.y) < frac(t.x) && frac(t.y) < frac(-t.x)) { col = float4(1, 0, 0, 1); }
				else if (frac(t.y) < frac(t.x) && frac(t.y) > frac(-t.x)) { col = float4(0, 0, 1, 1); }
				else if (frac(t.y) > frac(t.x) && frac(t.y) < frac(-t.x)) { col = float4(0, 1, 0, 1); }
				else { discard; }

				return col;
			}
			ENDCG
		}
	}
}
