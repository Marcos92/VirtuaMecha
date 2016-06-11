Shader "Unlit/CallShading2"
{
	Properties{
		_Color("Color", Color) = (1, 1, 1, 1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	}
		SubShader
	{
		Tags{
		"RenderType" = "Opaque"
	}
		LOD 200

		CGPROGRAM

#pragma surface surf CelShadingForward
#pragma target 3.0

		// Calculo da direcção da luz e sombras
		half4 LightingCelShadingForward(SurfaceOutput s, half3 lightDir, half atten)
	{
		// se o valor de NdotL<= 0 vai atribuir uma sombra ao objecto, se este for superior não existe sombra:
		// ou seja se recebe luz não ha destaque de sombra, 
		half NdotL = dot(s.Normal, lightDir);
		if (NdotL <= 0.0)
		{
			NdotL = 0;
		}
		else
		{
			NdotL = 1;
		}

		half4 c;
		// Altera a cor do modelo conforme  a cor da uz que inside nele
		c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 2);
		c.a = s.Alpha;
		return c;
	}

	sampler2D _MainTex;
	fixed4 _Color;

	struct Input {
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		// Albedo vem da junção de cor à textura do modelo	
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG
	}
		FallBack "Diffuse"
}