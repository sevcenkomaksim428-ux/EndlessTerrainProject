Shader "Custom/Terrain"
{
    Properties {}
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings {
                float4 positionHCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
            };

            float minHeight;
            float maxHeight;

            float layerCount; 
            float4 baseColours[8]; 
            float baseStartHeights[8];
            float baseBlends[8]; 
            float baseColourStrength[8]; 
            float baseTextureScales[8];  

            TEXTURE2D_ARRAY(baseTextures);
            SAMPLER(sampler_baseTextures);

            float inverseLerp(float a, float b, float value) {
                if (a == b) return 0.0; 
                return saturate((value - a) / (b - a));
            }

            float3 triplanar(float3 worldPos, float scale, float3 blendAxes, int textureIndex) {
                float3 scaledWorldPos = worldPos / scale;
                float3 xProjection = SAMPLE_TEXTURE2D_ARRAY(baseTextures, sampler_baseTextures, scaledWorldPos.yz, textureIndex).rgb * blendAxes.x;
                float3 yProjection = SAMPLE_TEXTURE2D_ARRAY(baseTextures, sampler_baseTextures, scaledWorldPos.xz, textureIndex).rgb * blendAxes.y;
                float3 zProjection = SAMPLE_TEXTURE2D_ARRAY(baseTextures, sampler_baseTextures, scaledWorldPos.xy, textureIndex).rgb * blendAxes.z;
                return xProjection + yProjection + zProjection;
            }

            Varyings vert(Attributes IN) {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.worldNormal = TransformObjectToWorldNormal(IN.normalOS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target {
                float heightPercent = inverseLerp(minHeight, maxHeight, IN.worldPos.y);
                float3 blendAxes = abs(IN.worldNormal);
                blendAxes /= (blendAxes.x + blendAxes.y + blendAxes.z);

                float3 finalColor = float3(0, 0, 0);

                for (int i = 0; i < 8; i++) {
                    if (i >= layerCount) break; 
                    
                    float blendDst = baseBlends[i] / 2.0f;
                    float drawStrength = inverseLerp(-blendDst - 0.0001f, blendDst, heightPercent - baseStartHeights[i]);
                    
                    float3 textureColor = triplanar(IN.worldPos, baseTextureScales[i], blendAxes, i);
                    float3 tintColor = baseColours[i].rgb;
                    
                    float3 layerColor = textureColor * (1.0 - baseColourStrength[i]) + (textureColor * tintColor) * baseColourStrength[i];
                    
                    finalColor = finalColor * (1.0 - drawStrength) + layerColor * drawStrength;
                }

                return half4(finalColor, 1.0);  
            }
            ENDHLSL
        }
    }
}