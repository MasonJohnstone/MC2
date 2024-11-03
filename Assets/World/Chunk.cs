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

    public void Generate(WorldController worldController, int chunkSize, float terrainHeight, float terrainFrequency, float tunnelFrequency, float tunnelThreshold, Vector3Int chunkPosition)
    {
        float voxelSize = worldController.voxelSize;

        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                // Calculate global positions in world space using voxelSize
                float globalX = (x + chunkPosition.x * chunkSize) * voxelSize;
                float globalZ = (z + chunkPosition.z * chunkSize) * voxelSize;

                // Apply 2D Perlin noise for terrain height, yielding a natural continuous density
                float terrainHeightAtXZ = Mathf.PerlinNoise(globalX * terrainFrequency, globalZ * terrainFrequency) * terrainHeight;

                for (int y = 0; y < chunkSize; y++)
                {
                    // Calculate global Y position in world space using voxelSize
                    float globalY = (y + chunkPosition.y * chunkSize) * voxelSize;

                    // Base density based on height map
                    float heightDensity = Mathf.Clamp01((terrainHeightAtXZ - globalY) / terrainHeight);

                    // 3D Perlin noise for tunnels based on world-space coordinates
                    float tunnelNoise = Mathf.PerlinNoise(globalX * tunnelFrequency, globalY * tunnelFrequency)
                                      * Mathf.PerlinNoise(globalY * tunnelFrequency, globalZ * tunnelFrequency)
                                      * Mathf.PerlinNoise(globalX * tunnelFrequency, globalZ * tunnelFrequency);

                    // Adjust density smoothly based on tunnel noise and threshold
                    float tunnelDensity = Mathf.Clamp01((tunnelThreshold - tunnelNoise) / tunnelThreshold);

                    // Final density combines height density with tunnel modification
                    float density = heightDensity * tunnelDensity;

                    // Determine voxel type based on density threshold
                    int type = (density > 0.5f) ? 1 : 0;

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
