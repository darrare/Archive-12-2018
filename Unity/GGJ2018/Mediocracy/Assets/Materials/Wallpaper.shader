Shader "Hidden/Wallpaper"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Offset("Offset", Float) = 0.01
		_Insanity("Insanity", Float) = 1
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
			
			sampler2D _MainTex;
			float _Offset;
			float _Insanity;

			float3 rgb2hsv(float3 c)
			{
				float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
				float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

				float d = q.x - min(q.w, q.y);
				float e = 1.0e-10;
				return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
			}

			float3 hsv2rgb(float3 c)
			{
				float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
				float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
				return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// Pull the original texture color
				fixed4 col = tex2D(_MainTex, i.uv);

				// Convert the color to hsv
				float3 hsv = rgb2hsv(float3(col.rgb));

				// Set the hue to the offset
				hsv.x = _Offset;

				// Convert the color to rgb
				float3 colNew = hsv2rgb(hsv);

				// Return the new color
				col.rgb = colNew.rgb;

				// Depending on insanity level, lerp between grey and color
				float4 grey = tex2D(_MainTex, i.uv).rrra;
				col.rgb = lerp(float3(grey.rgb), float3(col.rgb), _Insanity);

				return col;
			}
			ENDCG
		}
	}
}
