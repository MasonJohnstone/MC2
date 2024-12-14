using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkData
{
    public Voxel[,,] voxelMap;
    public Vector3Int chunkPosition;

    public void Init(Voxel[,,] _voxelMap, Vector3Int _chunkPosition)
    {
        voxelMap = _voxelMap;
        chunkPosition = _chunkPosition;
    }

    //public Voxel GetVoxel(Vector3Int position)
    //{
    //    return voxelMap[position.x, position.y, position.z];
    //}

    //public void SetVoxel(Vector3Int position, Voxel voxel)
    //{
    //    voxelMap[position.x, position.y, position.z] = voxel;
    //}

    public void Update(Voxel[,,] _voxelMap)
    {
        voxelMap = _voxelMap;
    }
}

public struct Voxel
{
    public int id;
    public Type type;
    public float density;
}

public enum Type
{
    none, block, terrain
}
