Shader "Unlit/Distortion"
{
    Properties
    {
        _MainTexture("Noise texture", 2D) = "white" {}
        _OtherTexture("Additional Text", 2D) = "white" {}
        _Color("Water Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 ScreenPos : TEXCOORD1;
                float2 depth : TEXCOORD2;
            };

            sampler2D _MainTexture;
            sampler2D _OtherTexture;
            float4 _Color;

            v2f vert (appdata v)
            {
                float2 uv = v.uv;
                
                uv.x += frac(float2(_Time.y,0)* 0.01) ;
                uv.y += frac(float2(_Time.y,0)* 0.01) ;
                v2f o;

                o.ScreenPos = mul(UNITY_MATRIX_P, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);  
                o.uv = uv;
                UNITY_TRANSFER_DEPTH(o.depth);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                
                //return float4(uv,0,1);
                //UNITY_OUTPUT_DEPTH(i.depth);
                float2 screenUVs = i.ScreenPos.xy / i.ScreenPos.w;


                float4 textureColor = tex2D(_MainTexture,i.uv) ;
                float4 distortion = i.ScreenPos + (textureColor * .61);


                //float zEye = LinearEyeDepth(SampleSceneDepth(screenUVs), _ZBufferParams);
                float distorted = tex2D(_OtherTexture,distortion);
                return distorted * _Color;


                //float4 col = float4(i.uv,0,1);
                ///return col;
            }
            ENDCG
        }
    }
}
