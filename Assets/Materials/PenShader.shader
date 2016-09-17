// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/PenShader" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Orientation ("Orientation",Float) = 0
	_PosCassette("Cassette Position", Vector) = (0,0,0,0)
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
				float4 vertexWorld : TEXCOORD1;
				half2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float4 _PosCassette;
			float _Orientation;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.vertexWorld = mul(_Object2World,v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.texcoord);

				if(_Orientation>0.0){
					if(i.vertexWorld.y<=_PosCassette.y){
						col.a = 0.0;
					}
				}else if(_Orientation<0.0){
					if(i.vertexWorld.y>=_PosCassette.y){
						col.a = 0.0;
					}
				}
				
				return col;
			}
		ENDCG
	}
}

}
