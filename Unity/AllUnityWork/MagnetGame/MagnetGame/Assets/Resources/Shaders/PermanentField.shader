Shader "Unlit/PermanentField"
{
	Properties
	{
		//_Color("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_ScrollSpeed("Scroll Speed", vector) = (0, -20, 0, 0)
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			//float4 _Color;
			float4 _ScrollSpeed;
			
			v2f vert(appdata v)
			{
				v2f o;

				//float cosX = dot(_ScrollSpeed, float4(1, 0, 0, 0));
				//float sinX = sqrt(1 - pow(cosX, 2));
				//float cosX = cos(-90);
				//float sinX = sin(-90);
				//float2x2 rotationMatrix = float2x2(cosX, -sinX, sinX, cosX);
				//v.uv = v.uv + .5f;
				//v.uv = mul(v.uv, rotationMatrix);

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				//Shift over time
				o.uv += _ScrollSpeed * _Time.x;

				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
