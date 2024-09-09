Shader "Custom/SimplestInstancedShader"
{
    Properties
    {
        _LerpTopColor  ("Color", Color) = (1, 1, 1, 1)
        _LerpBottomColor ("Color", Color) = (1, 1, 1, 1)
        _AmbientOcclusionColor ("Color", Color) = (1, 1, 1, 1)
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
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID // use this to access instanced properties in the fragment shader.
            };



            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float4, _LerpTopColor)
                UNITY_DEFINE_INSTANCED_PROP(float4, _LerpBottomColor)
                UNITY_DEFINE_INSTANCED_PROP(float4, _AmbientOcclusionColor)
            UNITY_INSTANCING_BUFFER_END(Props)

            struct GrassData
{
                float3 Position;
                float3 Rotation;
            };

            StructuredBuffer<GrassData> GrassBuffer;
            float _Scale;
            v2f vert(appdata v,uint instanceID : SV_INSTANCEID)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);

                float3 pos = GrassBuffer[instanceID].Position;
                float sinTime = 10*sin(pos.x  * _Time.y);

                v.vertex.x += v.uv0.y * sinTime * 10;
                v.vertex.z += v.uv0.y * sinTime * 10;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv0 = v.uv0;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                float3 lightDir = _WorldSpaceLightPos0.xyz;
                float ndotl = saturate(dot(lightDir, normalize(float3(0, 1, 0))));

                float4 bottomColor = UNITY_ACCESS_INSTANCED_PROP(Props, _LerpBottomColor);
                float4 topColor = UNITY_ACCESS_INSTANCED_PROP(Props, _LerpTopColor);
                float4 AmbientCol = UNITY_ACCESS_INSTANCED_PROP(Props, _AmbientOcclusionColor);

                float4 tip = lerp( AmbientCol,0.0f, i.uv0.y * i.uv0.y * (1.0f + _Scale));
                float4 col = lerp(topColor, bottomColor, i.uv0.y);
                //float4 tipColor = lerp(topColor,AmbientCol,-i.uv0.y);
               // return col + tipColor;
               return (col + tip);
            }
            ENDCG
        }
    }
}