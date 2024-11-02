using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRenderer
{
    private MarchingCubes marchingCubes;

    public ChunkRenderer()
    {
        // Initialize MarchingCubes once in the constructor with a surface threshold (e.g., 0.5f)
        marchingCubes = new MarchingCubes(0.99f);
    }


    public void SetMeshData(WorldController worldController, Chunk chunkData, Vector3Int chunkPosition, GameObject chunkObject, int chunkSize)
    {
        List<Vector3> chunkVertices = new List<Vector3>();
        List<int> chunkTriangles = new List<int>();
        int offset = 0;

        // Helper function to get density, considering chunk boundaries
        float GetDensity(int x, int y, int z)
        {
            if (x >= 0 && x < chunkSize && y >= 0 && y < chunkSize && z >= 0 && z < chunkSize)
            {
                // Within bounds of this chunk
                return chunkData.voxelMap[x, y, z].density;
            }
            else
            {
                // Calculate offset for neighboring chunk
                Vector3Int neighborOffset = new Vector3Int(
                    x < 0 ? -1 : x >= chunkSize ? 1 : 0,
                    y < 0 ? -1 : y >= chunkSize ? 1 : 0,
                    z < 0 ? -1 : z >= chunkSize ? 1 : 0
                );

                Vector3Int neighborPosition = chunkPosition + neighborOffset;

                // Try to get the neighboring chunk
                if (worldController.chunkDataCache.TryGetValue(neighborPosition, out Chunk neighborChunk))
                {
                    // Calculate local position within the neighboring chunk
                    int localX = (x + chunkSize) % chunkSize;
                    int localY = (y + chunkSize) % chunkSize;
                    int localZ = (z + chunkSize) % chunkSize;

                    return neighborChunk.voxelMap[localX, localY, localZ].density;
                }
                else
                {
                    return 0f; // Default density for out-of-bounds if no neighbor
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
                    // Prepare a float array for corner densities
                    float[] cubeDensities = new float[8];

                    // Assign density values for each corner of the cube
                    cubeDensities[0] = GetDensity(vmx, vmy, vmz);
                    cubeDensities[1] = GetDensity(vmx + 1, vmy, vmz);
                    cubeDensities[2] = GetDensity(vmx + 1, vmy + 1, vmz);
                    cubeDensities[3] = GetDensity(vmx, vmy + 1, vmz);
                    cubeDensities[4] = GetDensity(vmx, vmy, vmz + 1);
                    cubeDensities[5] = GetDensity(vmx + 1, vmy, vmz + 1);
                    cubeDensities[6] = GetDensity(vmx + 1, vmy + 1, vmz + 1);
                    cubeDensities[7] = GetDensity(vmx, vmy + 1, vmz + 1);

                    // Prepare lists to hold the generated vertices and triangles for this cube
                    List<Vector3> cubeVertices = new List<Vector3>();
                    List<int> cubeTriangles = new List<int>();

                    // Call PerformMarch for this cube
                    marchingCubes.PerformMarch(vmx, vmy, vmz, cubeDensities, cubeVertices, cubeTriangles);

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

