using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public Voxel[,,] voxelMap;

    public void Init(Voxel[,,] _voxelMap)
    {
        voxelMap = _voxelMap;
    }

    public Voxel GetVoxel(Vector3Int position)
    {
        return voxelMap[position.x, position.y, position.z];
    }

    public void SetVoxel(Vector3Int position, Voxel voxel)
    {
        voxelMap[position.x, position.y, position.z] = voxel;
    }

    public void Update(Voxel[,,] _voxelMap)
    {
        voxelMap = _voxelMap;
    }
}

public struct Voxel
{
    public int type;
    public float density;
}
