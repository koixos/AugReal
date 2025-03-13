Shader "Custom/PointCloud"
{
    Properties
    {
        _PointSize("Point Size", Float) = 0.05
        _PointColor("Point Color", Color) = (0,1,0,1)
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 5.0
            #include "UnityCG.cginc"
            
            struct appdata
            {
                uint vertexID : SV_VertexID;
            };
            
            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
                float size : PSIZE;
            };
            
            float _PointSize;
            float4 _PointColor;
            
            StructuredBuffer<float3> _PointsBuffer;
            
            v2f vert(appdata v)
            {
                v2f o;
                float3 worldPos = _PointsBuffer[v.vertexID];
                o.pos = UnityObjectToClipPos(float4(worldPos, 1.0));
                o.color = _PointColor;
                o.size = _PointSize * 10;
                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
}