Shader "Terrian2D/Rtt2Tt2d" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_OffsetX ("OffsetX", float) = 0
		_OffsetY ("OffsetY", float) = 0
	}
	SubShader 
	{
		Lighting Off Cull Off ZTest Off ZWrite Off AlphaTest Off
		Fog { Mode Off }
		
		Pass
		{
			CGPROGRAM

			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float _OffsetX;
			float _OffsetY;

			fixed4 frag(v2f_img i) : Color
			{
				float2 uv = i.uv;
				uv.x += _OffsetX;
				uv.y += _OffsetY;
				return tex2D(_MainTex, uv);
			}

			ENDCG
		}
		
	} 
	FallBack Off
}
