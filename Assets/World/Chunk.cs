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

    public void Generate(int chunkSize, float waveHeight, float waveFrequency, Vector3Int chunkPosition)
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                // Calculate the global x, y, and z positions by adding the chunk position offset
                int globalX = x + chunkPosition.x * chunkSize;
                int globalZ = z + chunkPosition.z * chunkSize;

                // Calculate the height of the sine wave at this global (x, z) position
                float wave = Mathf.Sin(globalX * waveFrequency) * Mathf.Sin(globalZ * waveFrequency) * waveHeight;

                for (int y = 0; y < chunkSize; y++)
                {
                    // Calculate the global y position with chunk position offset
                    int globalY = y + chunkPosition.y * chunkSize;

                    // Set voxel type based on whether it's below or above the wave height, adjusted by globalY
                    int type = (globalY <= wave) ? 1 : 0;

                    // Create the voxel, setting it as opaque if type is 1
                    Voxel voxel = new Voxel { type = type, isOpaque = (type == 1) };
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
}
