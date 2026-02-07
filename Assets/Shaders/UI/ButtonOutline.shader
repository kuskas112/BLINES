Shader "Custom/UI/ButtonOutline"
{
    Properties
    {
        [HDR] _NeonColor("Neon Color", Color) = (1, 0.2, 1, 1)
        _OutlineWidth("Outline Width", Range(0, 0.5)) = 0.1
        _SmoothWidth("Smooth Width", Range(0, 0.5)) = 0.1
        _GlowIntensity("Glow Intensity", Range(1, 10)) = 3.0
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline" 
        }
        
        Blend SrcAlpha OneMinusSrcAlpha  // Обычная прозрачность
        // Или для свечения:
        // Blend One One  // Аддитивное смешивание
        ZWrite Off  // Важно для 2D/прозрачных объектов

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;  // UV все еще нужны для эффектов!
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _NeonColor;
                half _GlowIntensity;
                half _OutlineWidth;
                half _SmoothWidth;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;  // Просто передаем UV как есть
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 centeredUV = abs(IN.uv - 0.5);
                float innerBoundary = 0.5 - _OutlineWidth;
                
                // Вычисляем, насколько мы вышли за внутреннюю границу
                float2 outside = max(centeredUV - innerBoundary, 0);
                
                // Определяем область обводки (1 = обводка, 0 = не обводка)
                float outlineMask = saturate(outside.x + outside.y);
                
                // Плавный переход для антиалиасинга (опционально)
                outlineMask = smoothstep(0, _SmoothWidth, outlineMask);
                
                return outlineMask * _GlowIntensity * _NeonColor;
            }
            ENDHLSL
        }
    }
}