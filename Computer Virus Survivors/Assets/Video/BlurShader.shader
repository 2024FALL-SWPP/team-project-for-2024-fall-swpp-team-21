Shader "UI/GlassMorphismWhite"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0, 20)) = 5
        _Opacity ("Opacity", Range(0, 1)) = 0.5
        _WhiteOverlay ("White Overlay", Range(0, 1)) = 0.3
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        
        GrabPass { "_GrabTexture" }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 grabPos : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            sampler2D _GrabTexture;
            sampler2D _MainTex;
            float _BlurSize;
            float _Opacity;
            float _WhiteOverlay;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.grabPos = ComputeGrabScreenPos(o.pos);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float2 grabUV = i.grabPos.xy / i.grabPos.w;
                
                half4 blur = 0;
                float total = 0;
                
                #define ITERATIONS 16
                
                for(int x = -ITERATIONS; x <= ITERATIONS; x++)
                {
                    for(int y = -ITERATIONS; y <= ITERATIONS; y++)
                    {
                        float2 offset = float2(x, y) * _BlurSize * 0.0001;
                        float weight = exp(-(x*x + y*y) / (2 * ITERATIONS));
                        blur += tex2D(_GrabTexture, grabUV + offset) * weight;
                        total += weight;
                    }
                }
                blur /= total;
                
                half4 mainTex = tex2D(_MainTex, i.uv);
                half4 result = lerp(blur, mainTex, mainTex.a * _Opacity);
                return lerp(result, half4(1,1,1,1), _WhiteOverlay);
            }
            ENDCG
        }
    }
}