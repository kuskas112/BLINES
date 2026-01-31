Shader "Custom/SimplePolygon"
{
    Properties
    {
        [HDR] _MainColor("Main Color", Color) = (1, 1, 1, 1)
        [HDR] _NeonColor("Neon Color", Color) = (1, 0.2, 1, 1)
        _EdgeWidth("Edge Width", Range(0, 2)) = 0.05
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
                half4 _MainColor;
                half4 _NeonColor;
                half _EdgeWidth;
                half _GlowIntensity;
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
                // ====================
                // ОСНОВНАЯ ЛОГИКА БЕЗ ТЕКСТУРЫ
                // ====================
                
                // 1. Центрируем UV
                // UV идут от (0,0) в углу до (1,1) в противоположном углу
                // Мы хотим от (-0.5,-0.5) до (0.5,0.5)
                float2 centeredUV = IN.uv - 0.5;
                
                // 2. Расстояние от центра (0 в центре, ~0.7 в углу квадрата)
                float distanceFromCenter = length(centeredUV) * 2.0;
                // Умножаем на 2, потому что от центра до угла ~0.7, хотим до 1.0
                
                // 3. Эффект грани полигона
                // smoothstep создает плавный переход:
                // - 0 если distance < 0.95 - _EdgeWidth
                // - 1 если distance > 0.95
                // - плавно между
                float edge = smoothstep(0.95 - _EdgeWidth, 0.95, distanceFromCenter);
                
                // 4. Внешнее свечение (мягкая обводка)
                float outerGlow = smoothstep(0.95, 1.0, distanceFromCenter);
                outerGlow = pow(outerGlow, 2.0) * 0.5; // Делаем мягче
                
                // 5. Комбинируем эффекты
                float totalGlow = max(edge, outerGlow);
                
                // 6. Создаем цвет
                half4 color;
                
                if (totalGlow > 0.01) // Если есть свечение
                {
                    // Неоновое свечение
                    color = _NeonColor * _GlowIntensity * totalGlow;
                }
                else
                {
                    // Основной цвет (может быть темным или прозрачным)
                    color = _MainColor;
                }
                
                // 7. Плавное исчезновение к краям
                // Чтобы не было резкого обрезания
                color.a *= 1.0 - smoothstep(0.8, 1.0, distanceFromCenter);
                
                return color;
            }
            ENDHLSL
        }
    }
}