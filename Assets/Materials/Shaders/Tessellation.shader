Shader "Custom/Tessellation"
{
    Properties
    {
        _Tess ("Tessellation", Range(1,32)) = 4
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _DispTex ("Disp Texture", 2D) = "gray" {}
        _NormalMap ("Normalmap", 2D) = "bump" {}
        _Displacement ("Displacement", Range(0, 1.0)) = 0.3
        _Waves ("Waves", Range(0, 5.0)) = 1.0
        _WaveSpeed ("WaveSpeed", Range(0, 50)) = 5
        _VertexCount ("VertexCount", int) = 0
        _Color ("Color", color) = (1,1,1,0)
        _SpecColor ("Spec color", color) = (0.5,0.5,0.5,0.5)
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 300

        CGPROGRAM
        #pragma surface surf BlinnPhong addshadow fullforwardshadows vertex:disp tessellate:tessFixed nolightmap
        #pragma target 4.6

        struct appdata
        {
            float4 vertex : POSITION;
            float4 tangent : TANGENT;
            float3 normal : NORMAL;
            float2 texcoord : TEXCOORD0;
            //float1 vertexCount : FLOAT;
        };

        float _Tess;

        float4 tessFixed()
        {
            return _Tess;
        }
        

        sampler2D _DispTex;
        float _Displacement;
        float _Waves;
        int _WaveSpeed;
        int _VertexCount;

        void disp(inout appdata v)
        {
            //float d = tex2Dlod(_DispTex, float4(v.texcoord.xy,0,0)).r * _Displacement;
            float3 worldpos = mul(unity_ObjectToWorld, v.vertex);
            float d = sin(worldpos.x * UNITY_PI * _Waves + _Time * _WaveSpeed) * _Displacement + cos(
                worldpos.z * UNITY_PI * _Waves) * _Displacement;
            
            v.vertex.y += d + 1;

        }

        

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        sampler2D _MainTex;
        sampler2D _NormalMap;
        
        fixed4 _Color;

        void surf(Input IN, inout SurfaceOutput o)
        {
            float d = sin(IN.worldPos.x * UNITY_PI * _Waves + _Time * _WaveSpeed) * _Displacement + cos(
                IN.worldPos.z * UNITY_PI * _Waves) * _Displacement;
            half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color * (d+1);
            o.Albedo = c.rgb;
            o.Specular = 0.2;
            o.Gloss = 1.0;
            o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
        }
        ENDCG
    }
    FallBack "Diffuse"
}