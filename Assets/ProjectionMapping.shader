Shader "Custom/ProjectionMapping"
{
    Properties
    {
        _PersonTex ("PersonCam Texture", 2D) = "white" {}
    }
    
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            
            struct v2f
            {
                float4 uv_PersonCam : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _PersonTex;
            float4x4 _PersonCamVP; // View * Projection matrix for the personCam.
            float4x4 _MyToWorld;

            v2f vert (appdata v)
            {
                v2f o;
                float4 worldPos = mul(_MyToWorld, v.vertex);
                o.uv_PersonCam = mul(_PersonCamVP, worldPos);
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float2 uv = (i.uv_PersonCam.xy / i.uv_PersonCam.w) * 0.5 + 0.5; // Convert to [0, 1] range.
                half4 col = tex2D(_PersonTex, uv);
                return col;
            }
            ENDCG
        }
    }
}
