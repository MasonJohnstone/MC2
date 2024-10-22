Shader "Custom/TriplanarWithFlatNormalsGeometry"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Scale("Texture Scale", Float) = 1.0
        _LightDir("Light Direction", Vector) = (0.6, 1.0, 0.4, 0) // Angled light
        _LightIntensity("Light Intensity", Float) = 1.0 // Adjustable light intensity
        _AmbientIntensity("Ambient Intensity", Float) = 0.3 // Adjustable ambient light
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma geometry geom
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                float _Scale;
                float3 _LightDir;
                float _LightIntensity;
                float _AmbientIntensity;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float3 worldPos : TEXCOORD0;
                    float3 faceNormal : TEXCOORD1;
                };

                // Vertex shader: Pass world position to geometry shader
                v2f vert(appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    return o;
                }

                // Geometry shader: Calculate flat face normal
                [maxvertexcount(3)]
                void geom(triangle v2f input[3], inout TriangleStream<v2f> triStream)
                {
                    // Calculate the face normal using two edges of the triangle
                    float3 faceNormal = normalize(cross(input[1].worldPos - input[0].worldPos, input[2].worldPos - input[0].worldPos));

                    // Emit each vertex with the same face normal for flat shading
                    for (int i = 0; i < 3; i++)
                    {
                        v2f output = input[i];
                        output.faceNormal = faceNormal;  // Assign the face normal to each vertex
                        triStream.Append(output);
                    }
                }

                // Triplanar mapping function
                float4 sampleTriplanar(float3 worldPos, float3 faceNormal)
                {
                    float3 absWorldNormal = abs(faceNormal);

                    // Blend factors
                    float3 blendWeights = absWorldNormal / (absWorldNormal.x + absWorldNormal.y + absWorldNormal.z);

                    // Get the individual projections in world space
                    float2 xProjection = worldPos.yz * _Scale;
                    float2 yProjection = worldPos.zx * _Scale;
                    float2 zProjection = worldPos.xy * _Scale;

                    // Sample the texture based on the dominant axis
                    float4 xTex = tex2D(_MainTex, xProjection);
                    float4 yTex = tex2D(_MainTex, yProjection);
                    float4 zTex = tex2D(_MainTex, zProjection);

                    // Blend the textures based on the normal direction
                    return xTex * blendWeights.x + yTex * blendWeights.y + zTex * blendWeights.z;
                }

                // Fragment shader: Use the face normal for lighting and texture mapping
                float4 frag(v2f i) : SV_Target
                {
                    // Normalize the light direction (coming from an angle)
                    float3 lightDir = normalize(_LightDir);

                    // Sample triplanar texture based on the face normal
                    float4 color = sampleTriplanar(i.worldPos, i.faceNormal);

                    // Calculate diffuse lighting using the angled light direction
                    float NdotL = max(dot(i.faceNormal, lightDir), 0.0);

                    // Apply light intensity
                    float lightContribution = NdotL * _LightIntensity;

                    // Add ambient light so the shadowed faces aren’t completely dark
                    float ambientContribution = _AmbientIntensity;

                    // Combine light and ambient contribution
                    float finalLighting = saturate(lightContribution + ambientContribution);

                    // Return the final color with lighting applied
                    return float4(color.rgb * finalLighting, color.a);
                }

                ENDCG
            }
        }
            FallBack "Diffuse"
}
