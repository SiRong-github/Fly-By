Shader "Unlit/WaterShader"
{
    Properties
    {
		_MainTex ("Texture", 2D) = "white" {}
        _TintColour("Tint Color", Color) = (1,1,1,1)
		_SpeedX ("SpeedX", Range(0,20)) = 3.0
		_SpeedY ("SpeedY", Range(0,20)) = 3.0
		_Speed ("Speed", Range(0,20)) = 3.0
		_Amplitude ("Amplitude", Range(0,10)) = 2.0
		_Transparency ("Transparency", Range(0.0,1)) = 0.25
		_Distance ("Distance", Range(0,5)) = 1.0
		_Amount ("Amount", Range(0,5)) = 1.0 
    }
    SubShader
    {
		Tags
		{
			"Queue" = "Transparent" "RenderType" = "Transparent"
		}

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
			Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST; //offset & tiling 
			float4 _TintColour;
			float _SpeedX;
			float _SpeedY;
			float _Speed;
			float _Amplitude;
			float _Distance;
			float _Amount;
			float _Transparency;

			struct MeshData
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Interpolators
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			Interpolators vert(MeshData v)
			{
				float waveXDirection = sin((v.uv.x + _Time.y * _Speed) * _Distance * _Amount);
				float waveYDirection = sin((v.uv.y + _Time.y * _Speed) * _Distance * _Amount);
				v.vertex.y += waveXDirection * waveYDirection;
				Interpolators o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv.x += _Time.y * _SpeedX;
				o.uv.y += _Time.y * _SpeedY;
				return o;
			}
			
			fixed4 frag(Interpolators v) : SV_Target
			{
				fixed4 colour;
				colour = tex2D(_MainTex, v.uv) * _TintColour;
				colour.a = _Transparency;
				return colour;
			}
			ENDCG
		}
	}
}