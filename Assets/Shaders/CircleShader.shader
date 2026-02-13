Shader "Custom/CircleShader"
{
    Properties
    {
        [HDR] _NeonColor("Neon Color", Color) = (1, 0.2, 1, 1)
        _GlowIntensity("Glow Intensity", Range(1, 10)) = 3.0
        _Radius("Radius", Range(0,1)) = 0.2
        _Speed("Speed", Range(0,10000)) = 1
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
                float _Radius;
                float _Speed;
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
                float speed = 2;
                float newRadius = _Radius * sin(_Time * _Speed);
                newRadius *= newRadius;
                float distToCenter = centeredUV.x * centeredUV.x + centeredUV.y * centeredUV.y;

                // Замена условия на step(): возвращает 1, если distToCenter <= newRadius, иначе 0
                float mask = step(distToCenter, newRadius);
                half4 neonColor = _NeonColor * _GlowIntensity * mask;
                return neonColor;
            }
            ENDHLSL
        }
    }
}