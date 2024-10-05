Shader "Custom/ScanlineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScanlineIntensity ("Scanline Intensity", Range(0, 1)) = 0.5
        _ScanlineColor ("Scanline Color", Color) = (0, 0, 0, 0.5)
        _ScanlineDensity ("Scanline Density", Range(1, 200)) = 30 // 新增扫描线密度属性
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            // Properties
            float _ScanlineIntensity;
            float4 _ScanlineColor;
            float _ScanlineDensity; // 新增扫描线密度

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // 计算扫描线效果
                float scanline = step(0.5, frac(i.uv.y * _ScanlineDensity)); // 使用密度属性
                col.rgb = lerp(col.rgb, _ScanlineColor.rgb, scanline * _ScanlineIntensity);

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}