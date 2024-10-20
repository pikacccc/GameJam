Shader "Custom/PixelPostprocessing"
{
   Properties {
        // 显式声明出来_MainTex
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Dither ("Dither", 2D) = "white" {}
        _DownScale ("Downscale", Float) = 2
        _ColorRes ("ColorRes", Float) = 256
        _UserLut ("User Lut", 2D) =  "white" {}

    }
    SubShader {

        Tags {
            "RenderType"="Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass {
            Name "ColorBlitPass"

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityInput.hlsl"
            #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
            #pragma vertex Vert
            #pragma fragment frag


            #define DITHER
            #define AUTO_MODE
            #define DOWN_SCALE 2.0

            #define MAX_STEPS 196
            #define MIN_DIST 0.002
            #define NORMAL_SMOOTHNESS 0.1

            #define PALETTE_SIZE 16
            #define SUB_PALETTE_SIZE 8

            #define RGB(r,g,b) (vec3(r,g,b) / 255.0)
            static half3 palette[PALETTE_SIZE] = {
                    half3(  0,  0,  0)/255,
                    half3(255,255,255)/255 / 6,
                    half3(152, 75, 67)/255,
                    half3(121,193,200)/255,	
                    half3(155, 81,165)/255,
                    half3(104,174, 92)/255,
                    half3( 62, 49,162)/255,
                    half3(201,214,132)/255,	
                    half3(155,103, 57)/255,
                    half3(106, 84,  0)/255,
                    half3(195,123,117)/255,
                    half3( 85, 85, 85)/255,	
                    half3(138,138,138)/255,
                    half3(163,229,153)/255,
                    half3(138,123,206)/255,
                    half3(173,173,173)/255
                };

            // TEXTURE2D_X(_CameraOpaqueTexture);
            // SAMPLER(sampler_CameraOpaqueTexture);
            TEXTURE2D_X(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_Dither);
            SAMPLER(sampler_Dither);
            TEXTURE2D(_UserLut);

            float4 _Dither_TexelSize;
            float _DownScale;
            float _ColorRes;
            
            float _Intensity;

            half3 lut(half3 color)
            {
                float lutHeight = 32;
                float lutWidth = 1024;
                float3 userLutParams = float3(1/lutWidth, 1 / lutHeight,lutHeight - 1);
                half3 outLut = ApplyLut2D(TEXTURE2D_ARGS(_UserLut, sampler_PointClamp), color, userLutParams);
                return outLut;
            }

            //Blends the nearest two palette colors with dithering.
            half3 GetDitheredPalette(float3 color,float2 pixel)
            {
                float x = Luminance(color);
                
                float idx = ( clamp(x,0.0,1.0) * float(_ColorRes-1));
                float3 factor = color / saturate(color);
                float3 c1 = floor(saturate(color) * _ColorRes) / _ColorRes;
                float3 c2 = floor(saturate(color) * _ColorRes+(0.5).xxx) / _ColorRes;

                c1 = lut(floor(saturate(color) * _ColorRes) / _ColorRes);
                c2 = lut(floor(saturate(color) * _ColorRes+(0.5).xxx) / _ColorRes);

                float dith = SAMPLE_TEXTURE2D(_Dither, sampler_Dither, pixel * _ScreenParams.xy * _Dither_TexelSize.xy).r;
                float mixAmt = float(frac(idx) > dith);
                return lerp(c1,c2,mixAmt) * factor;
            }

            half4 frag (Varyings input) : SV_Target
            {

                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.texcoord);
                // float2 fragCoord = input.positionCS.xy / _ScreenParam.xy;
                // return half4(input.texcoord,0,1);
                //Palette preview
                // if(input.texcoord.x  < 0.05) 
                // {
                //     return half4(GetDitheredPalette(input.texcoord.yyy , input.texcoord / _DownScale),1);
                // }
                {

    
                    // color = saturate(color);
                    // color.rgb = lut(color.rgb);
                    color.rgb = GetDitheredPalette(color, input.texcoord / _DownScale);
                    //color.rgb = GetLinearToSRGB(color.rgb); // In LDR do the lookup in sRGB for the user LUT
                    // input.rgb = GetSRGBToLinear(input.rgb);

                }
                // return half4(GetDitheredPalette(color, input.texcoord / _DownScale),1);
                return color;
            }
            ENDHLSL
        }
    }
}