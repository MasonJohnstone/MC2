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

                    // Determine voxel type based on terrain height
                    int type = (globalY <= terrainHeightAtXZ) ? 1 : 0;
                    float density = (type != 0) ? 1.0f : 0.0f;

                    // Apply layered 3D Perlin noise for varied tunnels
                    if (type != 0) // Only apply tunnels below the terrain surface
                    {
                        // Adjust the 3D noise to account for chunkPosition.y for vertical variation
                        float tunnelNoise1 = Mathf.PerlinNoise(
                            globalX * tunnelFrequency,
                            (globalY + chunkPosition.y * chunkSize) * tunnelFrequency + globalZ * tunnelFrequency
                        );

                        float tunnelNoise2 = Mathf.PerlinNoise(
                            globalX * tunnelFrequency * 0.5f,
                            (globalY + chunkPosition.y * chunkSize) * tunnelFrequency * 0.5f + globalZ * tunnelFrequency * 0.5f
                        );

                        // Combine noises to add variation
                        float tunnelNoise = tunnelNoise1 * 0.6f + tunnelNoise2 * 0.4f;

                        if (tunnelNoise > tunnelThreshold)
                        {
                            type = 0; // Make it an air voxel for a tunnel
                            density = 0.0f;
                        }
                    }

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
