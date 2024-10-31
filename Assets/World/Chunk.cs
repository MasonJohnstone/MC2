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

    public void Generate(int seed, int chunkSize)
    {
        System.Random random = new System.Random(seed);

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    // 1 in 10 chance to set voxel type to 1; otherwise, it's 0
                    int type = (random.Next(10) == 0) ? 1 : 0;

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
