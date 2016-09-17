// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/PenShader" {
Properties{
	_Width ("Width",Float) = 0
}
SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 100
	
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha 
	
	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
			};

			float _Width;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = v.texcoord;
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				float2 uv = i.texcoord.xy - float2(0.5,0.5);
				float d = length(uv);
				float f = min(1.0-smoothstep(0.2+_Width,0.2+_Width+0.01,d),smoothstep(0.2,0.2001,d));
				d = d+0.01*sin(5.0*atan2(uv.y,uv.x));
				float c = 0.7+0.7*sin(100.0*floor(d*70.0)/40.0);
				return f*float4(c*44.0/255.0,c*22.0/255.0,c*8.0/255.0,1.0);

			}
		ENDCG
	}
}

}
