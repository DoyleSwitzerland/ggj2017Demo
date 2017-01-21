// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'

// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'

/*
Shader "Custom/Echo Projector" {
	Properties {
		_Color("Main Color", Color) = (0.0001,0.0001,0.0001,0)
		_ShadowTex("Cookie", 2D) = "" { TexGen ObjectLinear }
		_FalloffTex("FallOff", 2D) = "" { TexGen ObjectLinear }
	}

	Subshader {
		Pass {
			ZTest Always // Turn off z test so things show through each other
			ZWrite Off
			Color[_Color]
			Blend One One // Use additive blending to make rendering order independent

			SetTexture[_ShadowTex] {
				combine texture * primary, ONE - texture
				Matrix[_Projector]
			}

			SetTexture[_FalloffTex] {
				constantColor(0,0,0,0)
				combine previous lerp(texture) constant
				Matrix[_ProjectorClip]
			}
		}
	}
}
*/
// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'

Shader "Custom/Echo Projector" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_ShadowTex ("Cookie", 2D) = "" { }
		_FalloffTex ("FallOff", 2D) = "" { }
	}
	
	Subshader {
		Tags {"Queue"="Transparent"}
		Pass {
			ZTest Always // Turn off z test so things show through each other
			ZWrite Off
			//ColorMask RGB
			Color[_Color]
		//Blend is SrcFactor, DstFactor  - generated color is multiplied by SrcFactor, colour already on screen is multiplied by DstFac
			Blend One One

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"

			struct v2f {
				float4 uvShadow : TEXCOORD0;
				float4 uvFalloff : TEXCOORD1;
				UNITY_FOG_COORDS(2)
				float4 pos : SV_POSITION;
			};
			
			float4x4 unity_Projector;
			float4x4 unity_ProjectorClip;
			
			v2f vert (float4 vertex : POSITION) {
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, vertex);
				o.uvShadow = mul (unity_Projector, vertex);
				o.uvFalloff = mul (unity_ProjectorClip, vertex);
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}
			
			fixed4 _Color;
			sampler2D _ShadowTex;
			sampler2D _FalloffTex;
			
			fixed4 frag (v2f i) : SV_Target {
				fixed4 texS = tex2Dproj (_ShadowTex, UNITY_PROJ_COORD(i.uvShadow));
				texS.rgb *= _Color.rgb;
				texS.a = 1.0-texS.a;
	
				fixed4 texF = tex2Dproj (_FalloffTex, UNITY_PROJ_COORD(i.uvFalloff));
				fixed4 res = texS * texF.a;

				UNITY_APPLY_FOG_COLOR(i.fogCoord, res, fixed4(0,0,0,0));
				return res;
			}
			ENDCG
		}
	}
}

