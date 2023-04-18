Shader "Unlit/TargetingShader"
{
    Properties
    {
        [MainTexture] _MainTex("Texture", 2D) = "white" {}
        [MainColor] _BaseColor("GlowColor", Color) = (0.25, 0.5, 1, 1)
        _CoreColor("CoreColor", Color) = (1, 1, 1, 1)
        _InnerRadius("InnerRadius", Range(0.0, 1.0)) = 0.1
        _BlurRadius("BlurRadius", Range(0.0, 1.0)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline"}
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite On
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 obj : TEXCOORD0;
            };

            // sampler2D _MainTex;
            // float4 _MainTex_ST;
            fixed4 _BaseColor;
            fixed4 _CoreColor;
            float _InnerRadius;
            float _BlurRadius;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.obj = v.vertex;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed2 pos = i.obj.xy;
                fixed4 col = fixed4(0.0,0.0,0.0,1.0);

                float dist = distance(pos,fixed2(0.0,0.0));

                float opacity = dist / (_BlurRadius * 0.5);
                opacity = 1.0 - clamp(opacity, 0.0, 1.0);

                float centerOpacity = dist / (_InnerRadius * 0.5);
                centerOpacity = 1.0 - clamp(centerOpacity, 0.0, 1.0);

                col = _CoreColor * centerOpacity + fixed4(_BaseColor.rgb,opacity * opacity);

                return col;
            }
            ENDCG
        }
    }
}
