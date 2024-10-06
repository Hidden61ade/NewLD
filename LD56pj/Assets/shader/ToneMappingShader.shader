Shader "Custom/ToneMappingShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Exposure ("Exposure", Range(-5, 5)) = 0 // 曝光参数
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
            float _Exposure;

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

                // 色调映射效果
                col.rgb = col.rgb * pow(2.0, _Exposure); // 根据曝光进行调整

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}