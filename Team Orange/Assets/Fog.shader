// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Fog"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex, _CameraDepthTexture;
            float4 _MainTex_ST;
            float _FogCoeff;
            float4 _FogColour;

            float4 frag (v2f i) : SV_Target
            {
                // Fragment Shader
                // Referenced and modified from https://github.com/GarrettGunnell/Post-Processing
                
                float4 col = tex2D(_MainTex, 
                    UnityStereoScreenSpaceUVAdjust(
                    i.uv, _MainTex_ST));

                // Distance between object and camera in world
                float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);

                // Calculate view distance from depth
                float viewDistance = min(depth * _ProjectionParams.z, 1000.0f);

                // Calculate the fog factor
                float fogFactor = exp(-1.0f * (_FogCoeff / sqrt(log(2))) * max(0.0f, viewDistance));

                return lerp(col, _FogColour, saturate(fogFactor));
            }
            ENDCG
        }
    }
}
