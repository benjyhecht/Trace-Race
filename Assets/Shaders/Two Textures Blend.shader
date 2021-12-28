Shader "Custom/TwoTexturesBlend" {
    Properties {
	    _Color("Tint", Color) = (0.5, 0.5, 0.5, 1)
	    _Mask("Mask", 2D) = "white" {}

        [NoScaleOffset]_MainTex("Albedo", 2D) = "white" {}
        [NoScaleOffset]_BumpMap("Normal Map", 2D) = "bump" {}
        [NoScaleOffset]_MainTex2("Albedo 2", 2D) = "white" {}
        [NoScaleOffset]_BumpMap2("Normal Map 2", 2D) = "bump" {}


    }

    SubShader {
        Tags { 
            "Queue" = "Geometry-4"
            "RenderType" = "Opaque" 
            "PreviewType" = "Plane" 
            "ForceNoShadowCasting" = "True"
        }
        //LOD 200

        //ZWrite Off

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf StandardSpecular

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap;
		float _BumpScale = 0.5f;

        sampler2D _MainTex2;
        sampler2D _BumpMap2;
		float _BumpScale2 = 0.5f;

        sampler2D _Mask;

        struct Input {
            float2 uv_MainTex: TEXCOORD0;
            float2 uv2_MainTex2: TEXCOORD2;
            float2 uv3_Mask: TEXCOORD1;
        };

        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandardSpecular o) {
            float2 uvterrain1 = IN.uv_MainTex;
            float2 uvterrain2 = IN.uv2_MainTex2;
            float2 uvmask = IN.uv3_Mask;

            fixed4 c1 = tex2D(_MainTex, uvterrain1);
            fixed4 n1 = tex2D(_BumpMap, uvterrain1);

            fixed4 c2 = tex2D(_MainTex2, uvterrain2);
            fixed4 n2 = tex2D(_BumpMap2, uvterrain2);

            fixed4 m = tex2D(_Mask, uvmask);

            o.Albedo = _Color * (c1 * m.r + c2 * (1 - m.r));

            float3 fn1 = UnpackScaleNormal(n1, _BumpScale);
            float3 fn2 = UnpackScaleNormal(n2, _BumpScale2);
            o.Normal = fn1 * m.r + fn2 * (1 - m.r);
        }
        ENDCG
    }
    FallBack "Diffuse"
}