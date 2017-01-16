Shader "Custom/TerrainMappedStandard" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_GroundTex ("Dirt (RGB)", 2D) = "white" {}
		_GrassTex ("Grass (RGB)", 2D) = "black" {}
		_MountainTex ("Rock (RGB)", 2D) = "blue" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _GroundTex;
		sampler2D _GrassTex;
		sampler2D _MountainTex;


		struct Input {
			float2 uv_GroundTex;
			float2 uv_GrassTex;
			float2 uv_MountainTex;
			float4 vertColors : COLOR;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 ground = tex2D (_GroundTex, IN.uv_GroundTex);
			fixed4 grass = tex2D (_GrassTex, IN.uv_GrassTex);
			fixed4 rock = tex2D (_MountainTex, IN.uv_MountainTex);

			fixed4 c = lerp(fixed4(0,0,0,1),ground, IN.vertColors.r);
			c= lerp(c, grass, IN.vertColors.g);
			c= lerp(c, rock, IN.vertColors.b);

			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
