using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRenderer
{
    private MarchingCubes marchingCubes;
    private NormalCubes normalCubes;
    WorldController worldController;

    public ChunkRenderer(WorldController _worldController)
    {
        // Initialize MarchingCubes once in the constructor with a surface threshold (e.g., 0.5f)
        marchingCubes = new MarchingCubes(0.5f);
        normalCubes = new NormalCubes();
        worldController = _worldController;
    }

    public void SetMeshData(ChunkData chunkData, GameObject chunkObject)
    {
        List<Vector3> chunkVertices = new List<Vector3>();
        List<int> chunkTriangles = new List<int>();
        int offset = 0;

        int chunkSize = worldController.chunkSize;
        float voxelSize = worldController.voxelSize;

        // Helper function to get density, considering chunk boundaries
        float GetDensity(int x, int y, int z)
        {
            if (x >= 0 && x < chunkSize && y >= 0 && y < chunkSize && z >= 0 && z < chunkSize)
            {
                // Within bounds of this chunk
                Voxel voxel = chunkData.voxelMap[x, y, z];
                return voxel.type == Type.terrain ? voxel.density : 0.5f;
            }
            else
            {
                // Calculate offset for neighboring chunk
                Vector3Int neighborOffset = new Vector3Int(
                    x < 0 ? -1 : x >= chunkSize ? 1 : 0,
                    y < 0 ? -1 : y >= chunkSize ? 1 : 0,
                    z < 0 ? -1 : z >= chunkSize ? 1 : 0
                );

                Vector3Int neighborPosition = chunkData.chunkPosition + neighborOffset;

                // Try to get the neighboring chunk
                if (worldController.chunkDataCache.TryGetValue(neighborPosition, out ChunkData neighborChunk))
                {
                    // Calculate local position within the neighboring chunk
                    int localX = (x + chunkSize) % chunkSize;
                    int localY = (y + chunkSize) % chunkSize;
                    int localZ = (z + chunkSize) % chunkSize;

                    Voxel voxel = neighborChunk.voxelMap[localX, localY, localZ];
                    return voxel.type == Type.terrain ? voxel.density : 0f;
                }
                else
                {
                    return 0f; // Default density for out-of-bounds if no neighbor
                }
            }
        }

        bool GetOccupation(int x, int y, int z)
        {
            if (x >= 0 && x < chunkSize && y >= 0 && y < chunkSize && z >= 0 && z < chunkSize)
            {
                // Within bounds of this chunk
                Voxel voxel = chunkData.voxelMap[x, y, z];
                return voxel.type == Type.block;
            }
            else
            {
                // Calculate offset for neighboring chunk
                Vector3Int neighborOffset = new Vector3Int(
                    x < 0 ? -1 : x >= chunkSize ? 1 : 0,
                    y < 0 ? -1 : y >= chunkSize ? 1 : 0,
                    z < 0 ? -1 : z >= chunkSize ? 1 : 0
                );

                Vector3Int neighborPosition = chunkData.chunkPosition + neighborOffset;

                // Try to get the neighboring chunk
                if (worldController.chunkDataCache.TryGetValue(neighborPosition, out ChunkData neighborChunk))
                {
                    // Calculate local position within the neighboring chunk
                    int localX = (x + chunkSize) % chunkSize;
                    int localY = (y + chunkSize) % chunkSize;
                    int localZ = (z + chunkSize) % chunkSize;

                    Voxel voxel = neighborChunk.voxelMap[localX, localY, localZ];
                    return voxel.type == Type.block;
                }
                else
                {
                    return false; // Default to false for out-of-bounds if no neighbor
                }
            }
        }


        // Loop through all voxels in the voxel map
        for (int vmx = 0; vmx < chunkSize; vmx++)
        {
            for (int vmy = 0; vmy < chunkSize; vmy++)
            {
                for (int vmz = 0; vmz < chunkSize; vmz++)
                {
                    float[] cubeDensities = new float[8];
                    cubeDensities[0] = GetDensity(vmx, vmy, vmz);
                    cubeDensities[1] = GetDensity(vmx + 1, vmy, vmz);
                    cubeDensities[2] = GetDensity(vmx + 1, vmy + 1, vmz);
                    cubeDensities[3] = GetDensity(vmx, vmy + 1, vmz);
                    cubeDensities[4] = GetDensity(vmx, vmy, vmz + 1);
                    cubeDensities[5] = GetDensity(vmx + 1, vmy, vmz + 1);
                    cubeDensities[6] = GetDensity(vmx + 1, vmy + 1, vmz + 1);
                    cubeDensities[7] = GetDensity(vmx, vmy + 1, vmz + 1);

                    bool[] cubeOccupations = new bool[8];
                    cubeOccupations[0] = GetOccupation(vmx, vmy, vmz);
                    cubeOccupations[1] = GetOccupation(vmx, vmy, vmz-1);
                    cubeOccupations[2] = GetOccupation(vmx, vmy, vmz+1);
                    cubeOccupations[3] = GetOccupation(vmx, vmy-1, vmz);
                    cubeOccupations[4] = GetOccupation(vmx, vmy+1, vmz);
                    cubeOccupations[5] = GetOccupation(vmx-1, vmy, vmz);
                    cubeOccupations[6] = GetOccupation(vmx+1, vmy, vmz);

                    // Prepare lists to hold the generated vertices and triangles for this cube
                    List<Vector3> cubeVertices = new List<Vector3>();
                    List<int> cubeTriangles = new List<int>();

                    // Call PerformMarch for this cube
                    marchingCubes.PerformMarch(vmx, vmy, vmz, cubeDensities, cubeVertices, cubeTriangles);
                    normalCubes.Generate(vmx, vmy, vmz, cubeOccupations, cubeVertices, cubeTriangles);

                    // Scale each vertex by voxelSize and add it to the chunk’s lists
                    for (int i = 0; i < cubeVertices.Count; i++)
                    {
                        cubeVertices[i] *= voxelSize;
                    }

                    // Add cube vertices and triangles to the chunk’s lists, adjusting indices
                    chunkVertices.AddRange(cubeVertices);
                    foreach (int cubeTriangle in cubeTriangles)
                    {
                        chunkTriangles.Add(cubeTriangle + offset);
                    }
                    offset += cubeVertices.Count;  // Update offset based on the number of vertices added
                }
            }
        }

        // Mesh setup (as you had it)
        MeshFilter meshFilter = chunkObject.GetComponent<MeshFilter>() ?? chunkObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = chunkObject.GetComponent<MeshRenderer>() ?? chunkObject.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();
        mesh.vertices = chunkVertices.ToArray();
        mesh.triangles = chunkTriangles.ToArray();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        // Optional: MeshCollider setup
        MeshCollider meshCollider = chunkObject.GetComponent<MeshCollider>();

        // Add MeshCollider only if it doesn't already exist
        if (meshCollider == null)
        {
            meshCollider = chunkObject.AddComponent<MeshCollider>();
        }

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

