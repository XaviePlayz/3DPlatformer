Shader "Custom/FlashingColorShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags {"RenderType"="Opaque" "Queue"="Background" }

		ZWrite Off
		Cull Back
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
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _FlashColor;
			float _FlashMultiplier;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 distuv = float2(i.uv.x, i.uv.y + _Time.x);
				fixed4 texColor = tex2D(_MainTex, distuv);
				fixed4 finalColor;
				fixed newAlphaFlashColor = _FlashMultiplier;

				_FlashColor.a = newAlphaFlashColor;					
				if(_FlashColor.a > 0)
					finalColor = texColor * _FlashColor;
				else
					finalColor = texColor;

				return finalColor;
			}
			ENDCG
		}
	}
}
