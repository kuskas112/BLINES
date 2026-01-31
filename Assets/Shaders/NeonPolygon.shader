Shader "Custom/NeonPolygon"
{
    Properties
    {
        [HDR] _NeonColor("Neon Color", Color) = (1, 0.2, 1, 1)
        _EdgeWidth("Edge Width", Range(0, 1)) = 0.05
        _GlowIntensity("Glow Intensity", Range(1, 10)) = 3.0
        _StripColor("Strip Color", Color) = (1, 1, 1, 1)
        _StripWidth("Strip Width", Range(0.001, 0.1)) = 0.02
        _StripIntensity("Strip Intensity", Range(0, 3)) = 5.0
        _StripPosition("Strip Position", Range(0.3, 0.7)) = 0.5
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
                half _EdgeWidth;
                half _GlowIntensity;
                half4 _StripColor;
                half _StripWidth;
                half _StripIntensity;
                half _StripPosition;
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
                float2 centeredUV = IN.uv - 0.5;
                float distToCenter = abs(centeredUV.y) / 0.5;
                
                // Существующий эффект краев
                float edge = smoothstep(_EdgeWidth, 1, 1 - distToCenter);
                half4 neonColor = _NeonColor * edge * _GlowIntensity;
                
                // Горизонтальная полоса с позиционированием
                // Преобразуем _StripPosition из [0.3, 0.7] в centeredUV координаты [-0.2, 0.2]
                float stripPosCentered = (_StripPosition - 0.5);
                float stripDist = abs(centeredUV.y - stripPosCentered);
                
                // Создаем полосу с плавными краями
                float strip = smoothstep(_StripWidth, 0, stripDist);
                half4 stripColor = _StripColor * strip * _StripIntensity;
                
                // Комбинируем эффекты
                half4 finalColor = neonColor + stripColor;
                finalColor.a = max(neonColor.a, stripColor.a);
                
                return finalColor;
            }
            ENDHLSL
        }
    }
}