// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Unlit/Floor"{
	Properties{
		_MainTex ("Texture", 2D)  = "white" {}
		_Color ("Color",color)    = (1,1,1,1)
		_Center("Center", Vector) = (0,0,0,0) // any point in plane in world space
        _Normal("Normal", Vector) = (0,1,0,0) // the normal vector of plane in world space
        _ShadowColor("Shadow Color",color) = (0,0,0,1)

	}
	SubShader{
		Tags { "RenderType"="Opaque" "LightMode"="ForwardBase"}
		LOD 100

		Pass{
			//texture with half lambert
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f{
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
				float4 posWorld : TEXCOORD2;
				float4 vertex : SV_POSITION;
			};

			uniform float4 _LightColor0; 
			sampler2D _MainTex;
			fixed4 _Color;

			v2f vert (appdata v){
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.posWorld = mul(unity_ObjectToWorld,v.vertex);
				o.normal = normalize(mul(float4(v.normal,0) , unity_WorldToObject).xyz);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				float3 normal= normalize(i.normal);
				float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
				float3 lightCol = _LightColor0.rgb * (dot(normal,lightDir)/2 + 0.5) * 0.9;
				return fixed4(col.xyz * _Color.xyz * lightCol.xyz,1);
			}
			ENDCG
		}

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			struct appdata{
				float4 vertex : POSITION;
			};
			struct v2f{
				float4 vertex : SV_POSITION;
			};

            float4 _Center, _Normal;
            fixed4 _ShadowColor;

			v2f vert (appdata v){
				v2f o;
                float4 wPos = mul(unity_ObjectToWorld, v.vertex);
                // directional light
                float3 direction = normalize(_WorldSpaceLightPos0.xyz);

                //https://en.wikipedia.org/wiki/Line%E2%80%93plane_intersection
                float dist = dot(_Center.xyz - wPos.xyz, _Normal.xyz) / dot(direction, _Normal.xyz);

                //convert the vertex to the plain
                wPos.xyz   = dist * direction + wPos.xyz;

				o.vertex   = mul(unity_MatrixVP, wPos);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target{
				return _ShadowColor;
			}
			ENDCG
		
		}

	
	}
	Fallback "Diffuse"
}
