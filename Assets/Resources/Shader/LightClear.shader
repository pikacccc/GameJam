
Shader "Hidden/LightClear" {
    // _ClearColor ("ClearColor", Color) = (0,0,0,0.5))
    SubShader {
        Pass {
            Name "Blit Copy"
            ColorMask RGBA
            ZTest Always
            Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            
            HLSLPROGRAM

            #pragma vertex vert2
            #pragma fragment frag
            #include "UnityCG.cginc"
            struct appdata {
                float2 vertex : POSITION;
            };
            
            float4 _ClearColor;
            
            struct v2fLite {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            // #include "Assets/NOAH/Modules/PostProcess/Runtime/AssetBundle/NOAH/Rendering/Shader/PostProcessV2.cginc"
            v2fLite vertLite(appdata v) {
                v2fLite o;
                o.vertex = float4(v.vertex.xy, 0, 1);
                float2 uv = v.vertex.xy * 0.5 + 0.5;
                if (_ProjectionParams.x < 0) uv.y = 1 - uv.y;
                o.uv = uv;
                return o;
            }
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            v2fLite vert2(appdata v) {
                v2fLite o = vertLite(v);
                return o;
            }

            half4 frag(v2fLite i) : SV_TARGET {
                return _ClearColor;
            }
            ENDHLSL

        }
    }
    Fallback Off
}
