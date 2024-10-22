Shader "Custom/TriPlanarFixedLighting" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}  // The texture for tri-planar projection
        _BlendSharpness("Blend Sharpness", Range(0, 10)) = 2.0  // Controls blending between planes
        _LightColor("Light Color", Color) = (1, 1, 1, 1)  // Color for upward-facing surfaces
        _DarkColor("Dark Color", Color) = (0.2, 0.2, 0.2, 1)  // Color for downward-facing surfaces
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 200

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                sampler2D _MainTex;
                float _BlendSharpness;
                float4 _LightColor;
                float4 _DarkColor;

                struct appdata {
                    float4 vertex : POSITION;  // Vertex position
                    float3 normal : NORMAL;    // Vertex normal
                };

                struct v2f {
                    float4 pos : SV_POSITION;    // Final screen-space position
                    float3 worldPos : TEXCOORD0; // World space position
                    float3 normal : TEXCOORD1;   // World space normal
                };

                v2f vert(appdata v) {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);  // Transform vertex position to screen space
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; // World space position
                    o.normal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal)); // Transform normal to world space
                    return o;
                }

                // Helper function for texture projection and blending
                float4 SampleTriplanar(float3 position, float3 normal) {
                    // Calculate the blending factors based on the surface normal
                    float3 blending = pow(abs(normal), _BlendSharpness);
                    blending /= (blending.x + blending.y + blending.z); // Normalize the blending

                    // Sample the texture along the X, Y, and Z planes
                    float4 xProjection = tex2D(_MainTex, position.yz);  // X-axis projection
                    float4 yProjection = tex2D(_MainTex, position.xz);  // Y-axis projection
                    float4 zProjection = tex2D(_MainTex, position.xy);  // Z-axis projection

                    // Blend the three projections together based on the surface normal
                    return xProjection * blending.x + yProjection * blending.y + zProjection * blending.z;
                }

                // Fragment Shader
                fixed4 frag(v2f i) : SV_Target {
                    // Tri-planar texture sampling
                    float4 color = SampleTriplanar(i.worldPos, i.normal);

                    // Lighting based ONLY on the y-component of the normal
                    float lightFactor = saturate(i.normal.y);  // Using only the y-component of the normal

                    // Interpolate between dark and light based on the upward/downward direction
                    float4 finalLighting = lerp(_DarkColor, _LightColor, lightFactor);

                    // Return the texture color modulated by the lighting factor
                    return color * finalLighting;
                }

                ENDCG
            }
        }
}
