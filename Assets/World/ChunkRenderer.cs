using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRenderer
{
    public void SetMeshData(WorldController worldController, Chunk chunkData, Vector3Int chunkPosition, GameObject chunkObject, int chunkSize)
    {
        // mesh data variables
        List<Vector3> chunkVertices = new List<Vector3>();
        List<int> chunkTriangles = new List<int>();
        //List<Vector2> uvs = new List<Vector2>();

        // loop through chunk voxels
        for (int cvx = 0; cvx < chunkSize; cvx++)
        {
            for (int cvy = 0; cvy < chunkSize; cvy++)
            {
                for (int cvz = 0; cvz < chunkSize; cvz++)
                {
                    // if voxel isnt empty
                    if (chunkData.voxelMap[cvx, cvy, cvz].type != 0)
                    {
                        // define adjacency grid to store occupation and transparency of adjacent voxels
                        AdjacentVoxels adjacentVoxelIsOpaque = new AdjacentVoxels();
                        AdjacentVoxels adjacentVoxelIsOccupied = new AdjacentVoxels();

                        // loop through offset for each adjacent voxel
                        for (int ox = -1; ox <= 1; ox++)
                        {
                            for (int oy = -1; oy <= 1; oy++)
                            {
                                for (int oz = -1; oz <= 1; oz++)
                                {
                                    // default set all surrounding voxels to transparent just so it renders everyhting instead of nothing until i add conditional faces
                                    //adjacentVoxelIsOpaque.SetData(ox, oy, oz, false);

                                    // calculate adjacent voxel position in chunk voxel map
                                    int avx = cvx + ox, avy = cvy + oy, avz = cvz + oz;

                                    // if adjacent voxel is within chunk voxel map
                                    if (avx >= 0 && avx < chunkSize && avy >= 0 && avy < chunkSize && avz >= 0 && avz < chunkSize)
                                    {
                                        adjacentVoxelIsOccupied.SetData(ox, oy, oz, chunkData.voxelMap[avx, avy, avz].type != 0);
                                        adjacentVoxelIsOpaque.SetData(ox, oy, oz, chunkData.voxelMap[avx, avy, avz].isOpaque);
                                    }
                                    // if adjacent voxel is outside chunk voxel map
                                    else
                                    {
                                        // Calculate which chunk we need to access
                                        Vector3Int adjacentChunkPosition = chunkPosition;

                                        // Adjust the chunk position based on which axis is out of bounds
                                        if (avx < 0) adjacentChunkPosition.x -= 1;
                                        else if (avx >= chunkSize) adjacentChunkPosition.x += 1;

                                        if (avy < 0) adjacentChunkPosition.y -= 1;
                                        else if (avy >= chunkSize) adjacentChunkPosition.y += 1;

                                        if (avz < 0) adjacentChunkPosition.z -= 1;
                                        else if (avz >= chunkSize) adjacentChunkPosition.z += 1;

                                        // Get the adjacent chunk from the world controller
                                        Chunk adjacentChunk = worldController.GetChunkFromCacheAtPosition(adjacentChunkPosition);

                                        if (adjacentChunk != null)
                                        {
                                            // Calculate local voxel coordinates within the adjacent chunk
                                            int adjacentVoxelX = (avx + chunkSize) % chunkSize;
                                            int adjacentVoxelY = (avy + chunkSize) % chunkSize;
                                            int adjacentVoxelZ = (avz + chunkSize) % chunkSize;

                                            // Set data based on the voxel in the adjacent chunk
                                            adjacentVoxelIsOccupied.SetData(ox, oy, oz, adjacentChunk.voxelMap[adjacentVoxelX, adjacentVoxelY, adjacentVoxelZ].type != 0);
                                            adjacentVoxelIsOpaque.SetData(ox, oy, oz, adjacentChunk.voxelMap[adjacentVoxelX, adjacentVoxelY, adjacentVoxelZ].isOpaque);
                                        }
                                        else
                                        {
                                            // Handle missing chunk (optional) – you may want to assume occupancy or transparency defaults
                                            //adjacentVoxelIsOccupied.SetData(ox, oy, oz, false);
                                            //adjacentVoxelIsOpaque.SetData(ox, oy, oz, false);
                                            Debug.Log("You are somehow tryna access a chunk that isnt in the cache nigger!");
                                        }
                                    }
                                }
                            }
                        }

                        var voxelVertices = VoxelMeshData.GenerateVertices(adjacentVoxelIsOccupied, new Vector3(cvx, cvy, cvz));
                        var voxelTriangles = VoxelMeshData.GenerateTriangles(adjacentVoxelIsOpaque);
                        int vertexCount = chunkVertices.Count;
                        // Adjust the triangles by the current number of vertices
                        for (int i = 0; i < voxelTriangles.Count; i++)
                        {
                            voxelTriangles[i] += vertexCount;
                        }
                        chunkVertices.AddRange(voxelVertices);
                        chunkTriangles.AddRange(voxelTriangles);
                    }
                }
            }
        }

        // Ensure the GameObject has a MeshFilter
        MeshFilter meshFilter = chunkObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = chunkObject.AddComponent<MeshFilter>();
        }

        // Ensure the GameObject has a MeshRenderer
        MeshRenderer meshRenderer = chunkObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = chunkObject.AddComponent<MeshRenderer>();
        }

        // Create a new mesh and assign vertices and triangles
        Mesh mesh = new Mesh();
        mesh.vertices = chunkVertices.ToArray();
        mesh.triangles = chunkTriangles.ToArray();

        mesh.RecalculateNormals(); // Optionally recalculate normals

        // Assign the mesh to the MeshFilter
        meshFilter.mesh = mesh;

        // Ensure there's a MeshCollider component
        MeshCollider meshCollider = chunkObject.GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = chunkObject.AddComponent<MeshCollider>();
        }

        // Set the MeshCollider to use the same mesh
        meshCollider.sharedMesh = mesh;
    }
}

public class AdjacentVoxels
{
    private bool[,,] data;
    private int offset = 1; // Allows indexing with -1, 0, 1 for neighbors

    public AdjacentVoxels()
    {
        // Initialize a 3x3x3 array
        data = new bool[3, 3, 3];
    }

    // Set data for a specific position
    public void SetData(int dx, int dy, int dz, bool value)
    {
        //Debug.Log(dx + ", " + dy + ", " + dz);
        data[dx + offset, dy + offset, dz + offset] = value;
    }

    // Get data for a specific position
    public bool GetData(int dx, int dy, int dz)
    {
        return data[dx + offset, dy + offset, dz + offset];
    }
}

