using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public Voxel[,,] voxelMap;

    public void Init(int size)
    {
        voxelMap = new Voxel[size, size, size];
    }

    public Voxel GetVoxel(Vector3Int position)
    {
        return voxelMap[position.x, position.y, position.z];
    }

    public void SetVoxel(Vector3Int position, Voxel voxel)
    {
        voxelMap[position.x, position.y, position.z] = voxel;
    }

    public void Generate(int chunkSize, float terrainHeight, float terrainFrequency, float tunnelFrequency, float tunnelThreshold, Vector3Int chunkPosition)
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                // Calculate global X and Z positions
                int globalX = x + chunkPosition.x * chunkSize;
                int globalZ = z + chunkPosition.z * chunkSize;

                // 2D Perlin noise for terrain height at (x, z)
                float terrainHeightAtXZ = Mathf.PerlinNoise(globalX * terrainFrequency, globalZ * terrainFrequency) * terrainHeight;

                for (int y = 0; y < chunkSize; y++)
                {
                    // Calculate global Y position
                    int globalY = y + chunkPosition.y * chunkSize;

                    // Determine initial density based on terrain height
                    float baseDensity = (globalY <= terrainHeightAtXZ) ? 1.0f : 0.0f;

                    // Apply 3D Perlin noise for varied, winding tunnels
                    if (baseDensity != 0.0f) // Only apply tunnels below the terrain surface
                    {
                        // 3D noise-based tunnel generation
                        float tunnelNoiseX = Mathf.PerlinNoise(globalY * tunnelFrequency, globalZ * tunnelFrequency);
                        float tunnelNoiseY = Mathf.PerlinNoise(globalX * tunnelFrequency, globalZ * tunnelFrequency);
                        float tunnelNoiseZ = Mathf.PerlinNoise(globalX * tunnelFrequency, globalY * tunnelFrequency);

                        // Average out the 3D noises to create winding, organic shapes
                        float tunnelNoise = (tunnelNoiseX + tunnelNoiseY + tunnelNoiseZ) / 3.0f;

                        if (tunnelNoise > tunnelThreshold)
                        {
                            baseDensity = 0.0f; // Set as air if tunnel threshold is exceeded
                        }
                    }

                    // Round density to the nearest discrete value: 0, 0.33, 0.66, or 1
                    float density = 0.0f;
                    if (baseDensity > 0.0f)
                    {
                        if (baseDensity <= 0.33f) density = 0.33f;
                        else if (baseDensity <= 0.66f) density = 0.66f;
                        else density = 1.0f;
                    }

                    // Determine voxel type based on density
                    int type = (density >= 0.33f) ? 1 : 0;

                    // Create voxel with the calculated type and density
                    Voxel voxel = new Voxel { type = type, isOpaque = (type == 1), density = density };
                    SetVoxel(new Vector3Int(x, y, z), voxel);
                }
            }
        }
    }
}

public struct Voxel
{
    public int type;
    public bool isOpaque;
    public float density;
}
