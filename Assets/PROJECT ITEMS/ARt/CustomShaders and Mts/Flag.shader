Shader "Unlit/Flag"
{
    Properties
    {
        _color ("color" , Color)= (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include_with_pragmas "Assets/PROJECT ITEMS/ARt/EXPORT/Simplex.compute"
            // make fog work
            #pragma multi_compile_fog
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 normal : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

       
            float4 _color;
            v2f vert (appdata v)
            {
                v2f o;
                float yPos = v.vertex.y;
                v.vertex.y =  v.uv.y * sin( 8 *_Time.y  + v.vertex.z + (v.vertex.x* .01)) ;
               
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal  = v.normal;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 LightN = _WorldSpaceLightPos0;
                float3 N = i.normal;
                float lambert = saturate(dot(LightN,N));
                float4 final = float4(_color.xyz * lambert,1);
                return final;
            }
            ENDCG
        }
    }
}
