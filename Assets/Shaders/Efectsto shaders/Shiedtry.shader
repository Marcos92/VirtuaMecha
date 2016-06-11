Shader "Custom/TransparentFresnel"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_InnerColor("Inner Color", Color) = (1.0, 1.0, 1.0, 1.0)
		// cenas por mim 
		
		_RimColor("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		_RimPower("Rim Power", Range(0.5,8.0)) = 3.0

	
	}
		SubShader
	{
		Tags{ "Queue" = "Transparent"}
		Tags{ "RenderType" = "Opaque" }
		LOD 200
		//ZWrite Off
		Cull Back
		Blend One One
		//Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
//#pragma vertex vert
//#pragma fragment frag
//#pragma surface surf Lambert
#pragma surface surf Standard fullforwardshadows
#pragma target 3.0
//#pragma multi_compile_fog
//#include "UnityCG.cginc"

sampler2D _MainTex;

	struct Input
	{
		float3 viewDir;
		float2 uv_MainTex;
	};

	float4 _InnerColor;
	float4 _RimColor;
	float _RimPower;
	fixed4 _Color;
	




	//SE DER MERDA TIRAR;
	//--------------------------------------------------

		void surf(Input IN, inout SurfaceOutputStandard o)
	{
		fixed4 color = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = color.rgb; //_InnerColor.rgb; 
		half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
		o.Emission = _RimColor.rgb * pow(rim, _RimPower);
		o.Alpha = color.a;
	}
		//----------------------------------------------
	ENDCG
	}
		Fallback "Diffuse"
}
