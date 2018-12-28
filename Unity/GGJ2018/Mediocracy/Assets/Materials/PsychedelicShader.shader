Shader "Hidden/PsychoShader2"
{
	// FUCK UNITY

	Properties
	{
		_Seed("Seed", Vector) = (0.5, 0.5, 0.5, 0.5)
		_MainTex("Texture", 2D) = "white" {}
		_Zoom("Zoom", Vector) = (.1,.1,.1,.1)
		_Pan("Pan", Vector) = (0,0,0,0)
		_Aspect("Aspect Ratio", Float) = 1
		_Iterations("Iterations", Range(1,256)) = 256
		_Divisor("Divisor", Float) = 8.0
		_Offset("Offset", Float) = 0.25
		_Roll("Roll", Float) = 0
		_Alpha("Alpha", Float) = 1
		_Insanity("Insanity", Float) = 1

		_R("R", Float) = 5
		_UVOffset("UVOffset", Float) = .25
		_M1("M1", Float) = 4
		_M2("M2", Float) = 5
		_M3("M3", Float) = 7
		_Shift("Shift", Float) = .5
			_Saturation("Saturation", Float) = .25
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" }

		Pass
		{
			ZTest Off
			Blend SrcAlpha OneMinusSrcAlpha

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

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;

			struct Input
			{
				float2 uv_MainTex;
			};

			float4 _Seed;
			float4 _Zoom;
			float4 _Pan;
			float _Iterations;
			float _Aspect;
			float _Divisor;
			float _Offset;
			float _Roll;
			float _Alpha;
			float _Insanity;

			float _R;
			float _UVOffset;
			float _M1;
			float _M2;
			float _M3;
			float _Shift;
			float _Saturation;

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

			float2 multiplyComplex(float2 v1, float2 v2)
			{
				return float2(v1.x * v2.x - v1.y * v2.y, v1.x * v2.y + v1.y * v2.x);
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//////////////////////////////////////////////////////////
				// Masking the sprite's color elements
				//////////////////////////////////////////////////////////

				// Pull the original color from the texture
				fixed4 color = tex2D(_MainTex, i.uv);
				//float4 color = tex2D(_MainTex, uv);

				// If this is transparent, discard the pixel
				if (color.a < .1)
				{
					discard; //col.rbg = float4(1,1,1,1);
				}

				// If this is magenta, replace the color with the fractal
				if (color.g < .05 && color.r > color.g && color.b > color.g)
				{
					// Convert to the polar coordinate.
					float2 sc = i.uv - 0.5;
					float phi = atan2(sc.y, sc.x);
					float root = sqrt(dot(sc, sc));

					// Angular repeating.
					phi += _Offset;
					phi = phi - _Divisor * floor(phi / _Divisor);
					phi = min(phi, _Divisor - phi);
					phi += _Roll - _Offset;

					// Convert back to the texture coordinate.
					float2 uv = float2(cos(phi), sin(phi)) * root + 0.5;

					// Reflection at the border of the screen.
					uv = max(min(uv, 2.0 - uv), -uv);

					// Compute the fractal at the mirrored texture coordinate
					float2 c = _Seed;
					float2 v = (uv - _UVOffset) * _Zoom.xy * float2(1, _Aspect) - _Pan.xy;
					float m = 0;
					float2 v1;

					for (int n = 0; n < _Iterations; ++n) {
						//v1 = multiplyComplex(v, v);
						v = (multiplyComplex(v, v)) + c;

						if (dot(v, v) < (_R * _R - 1))
						{
							m++;
						}

						v = clamp(v, -_R, _R);
					}

					if (m == floor(_Iterations))
					{
						color = float4(.5,.5,.5,1);
					}
					else
					{
						color = float4(sin(m / _M1), sin(m / _M2), sin(m / _M3), 1) / 4 + _Shift;
						if (color.r > .9 && color.g > .9 && color.b > .9)
						{
							color = float4(.5, .5, .5, 1);
						}
					}

					// Increase saturation
					float3 hsvColor = rgb2hsv(color);
					hsvColor.y += _Saturation;
					clamp(hsvColor.y, 0, 1);
					color.rgb = hsv2rgb(hsvColor).xyz;

					// Pull the original contouring shading and reapply
					float4 contour = tex2D(_MainTex, i.uv);
					float aveContour = (contour.r + contour.b) / 2.0;

					color.rgb *= float3(aveContour, aveContour, aveContour);
				}
				else
				{
					color.g = color.r;
				}

				if (color.a > _Alpha)
					color.a = _Alpha;
				else
					color.a = 1;
				
				// Depending on insanity level, lerp between grey and color
				float4 grey = tex2D(_MainTex, i.uv).rrra;
				color.rgb = lerp(float3(grey.rgb), float3(color.rgb), _Insanity);

				return color;
			}
			ENDCG
		}
	}
}
