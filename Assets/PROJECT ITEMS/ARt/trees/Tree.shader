Shader "Unlit/Tree"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _color("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

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
                float4 normal: NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 normal: TEXCOORD1;

               
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _color;


            float map(float s, float a1, float a2, float b1, float b2)
            {
                return b1 + (s-a1)*(b2-b1)/(a2-a1);
            }


            v2f vert (appdata v)
            {
                v2f o;
                float3 newPos;
                newPos.x = map(v.uv.x,0,1,-1,1) * UNITY_MATRIX_MV;
                newPos.z = map(v.uv.y,0,1,-1,1) * UNITY_MATRIX_MV;
                newPos.y = 0 ;

                newPos.x *= unity_ObjectToWorld;
                newPos.z *= unity_ObjectToWorld;

                v.vertex.xyz += normalize(newPos) * 0.009;
                //v.vertex.z = v.uv.y;
                o.normal = v.normal;
                o.uv = v.uv;
                o.vertex = UnityObjectToClipPos(v.vertex);
               
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 light = _WorldSpaceLightPos0;
                float normal = i.normal;

                float lambert = max(.3,dot(light, normal));
                float4 col = float4(_color.xyz * lambert,.1);
                // apply fog
                
                return col;
            }
            ENDCG
        }
    }
}
