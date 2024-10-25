using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ChunkController : MonoBehaviour
{
    static int size = 7;

    bool[,,] voxelMap = new bool[size, size, size];

    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    public MeshCollider meshCollider;
    
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uvs = new List<Vector2>();

    void Start()
    {
        InitializeVoxelMap();
        AddVoxelData();
        CreateMesh();
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;  // Set the color for the Gizmos

    //    // Loop through all vertices and draw them as small spheres
    //    foreach (var vertex in vertices)
    //    {
    //        Gizmos.DrawSphere(vertex, 0.01f);  // Adjust the size of the spheres as needed
    //    }
    //}

    void InitializeVoxelMap()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    voxelMap[x, y, z] = Random.value < 1f / y / y;
                }
            }
        }
    }

    //void InitializeVoxelMap()
    //{
    //    for (int x = 0; x < size; x++)
    //    {
    //        for (int y = 0; y < size; y++)
    //        {
    //            for (int z = 0; z < size; z++)
    //            {
    //                // Generate the height for this (x, z) using a sine wave
    //                float height = Mathf.Sin(x * 0.2f) * Mathf.Sin(z * 0.2f) * 3f + 3f;

    //                // Set the voxel to true if it's below the generated height, false otherwise
    //                voxelMap[x, y, z] = y <= height;
    //            }
    //        }
    //    }
    //}


    //void InitializeVoxelMap()
    //{
    //    bool[,,] voxelMap = new bool[size, size, size];
    //    System.Text.StringBuilder sb = new System.Text.StringBuilder();

    //    for (int x = 0; x < size; x++)
    //    {
    //        for (int y = 0; y < size; y++)
    //        {
    //            for (int z = 0; z < size; z++)
    //            {
    //                voxelMap[x, y, z] = Random.value < 1f / y / y;  // Adjusting to avoid division by zero
    //            }
    //        }
    //    }

    //    // Print the voxel map in a format suitable for direct copying into code
    //    sb.Append("{\n");
    //    for (int x = 0; x < size; x++)
    //    {
    //        sb.Append("    {");
    //        for (int y = 0; y < size; y++)
    //        {
    //            sb.Append("{");
    //            for (int z = 0; z < size; z++)
    //            {
    //                sb.Append(voxelMap[x, y, z] ? "true, " : "false, ");
    //            }
    //            sb.Length -= 2;  // Remove the last comma and space
    //            sb.Append("}, ");
    //        }
    //        sb.Length -= 2;  // Remove the last comma and space
    //        sb.Append("},\n");
    //    }
    //    sb.Length -= 2;  // Remove the last comma and new line
    //    sb.Append("\n}");

    //    Debug.Log(sb.ToString());  // Output the voxel map
    //}

    //void InitializeVoxelMap()
    //{
    //    voxelMap = new bool[6, 6, 6] {
    //    { { true, true, true, true, true, true }, { true, true, true, true, true, true }, { false, false, true, false, false, false }, { true, false, false, false, false, false }, { true, false, false, true, false, false }, { true, false, false, false, false, false } },
    //    { { true, true, true, true, true, true }, { true, true, true, true, true, true }, { true, false, true, true, true, false }, { false, true, false, false, false, false }, { false, true, false, false, false, false }, { false, false, true, false, false, false } },
    //    { { true, true, true, true, true, true }, { true, true, true, true, true, true }, { true, false, false, true, false, false }, { true, false, false, false, false, false }, { false, false, false, false, false, false }, { false, false, false, false, false, false } },
    //    { { true, true, true, true, true, true }, { true, true, true, true, true, true }, { false, false, false, true, false, false }, { false, false, false, false, false, false }, { false, false, false, false, false, false }, { true, false, false, false, false, false } },
    //    { { true, true, true, true, true, true }, { true, true, true, true, true, true }, { false, true, false, true, false, false }, { true, false, false, true, false, false }, { false, false, false, false, false, true }, { false, false, false, false, false, false } },
    //    { { true, true, true, true, true, true }, { true, true, true, true, true, true }, { false, false, false, false, false, false }, { true, false, false, true, false, false }, { false, false, false, false, false, false }, { false, false, false, false, false, false } }
    //    };

    //    voxelMap[3, 5, 3] = true;
    //    voxelMap[2, 5, 4] = true;
    //}

    //void InitializeVoxelMap()
    //{
    //    voxelMap = new bool[3, 3, 3] {
    //    { { false, false, false }, { false, false, false }, { false, false, false } },
    //    { { false, false, false }, { false, false, false }, { false, false, false } },
    //    { { false, false, false }, { false, false, false }, { false, false, false } }
    //    };

    //    voxelMap[1, 1, 1] = true;
    //    voxelMap[0, 2, 1] = true;
    //}



    void AddVoxelData()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    if (voxelMap[x, y, z])
                    {
                        bool[,,] surroundingVoxelIsOccupied = new bool[3,3,3];
                        bool[,,] surroundingVoxelIsTransparent = new bool[3,3,3];
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                for (int k = 0; k < 3; k++)
                                {
                                    surroundingVoxelIsTransparent[i, j, k] = true; // Set default to true
                                }
                            }
                        }

                        // Check all surrounding voxels in 3x3x3 block
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            for (int dy = -1; dy <= 1; dy++)
                            {
                                for (int dz = -1; dz <= 1; dz++)
                                {
                                    // Calculate new index in surroundingVoxels array
                                    int sx = dx + 1;
                                    int sy = dy + 1;
                                    int sz = dz + 1;

                                    // Calculate neighboring voxel coordinates in the voxelMap
                                    int nx = x + dx, ny = y + dy, nz = z + dz;

                                    // Check if the neighboring voxel is within the bounds
                                    if (nx >= 0 && nx < size && ny >= 0 && ny < size && nz >= 0 && nz < size)
                                    {
                                        surroundingVoxelIsOccupied[sx, sy, sz] = voxelMap[nx, ny, nz];
                                        surroundingVoxelIsTransparent[sx, sy, sz] = !voxelMap[nx, ny, nz];

                                    }
                                }
                            }
                        }

                        var verts = VoxelMeshData.GenerateVertices(surroundingVoxelIsOccupied, new Vector3(x, y, z));
                        var tris = VoxelMeshData.GenerateTriangles(surroundingVoxelIsTransparent);
                        int vertexCount = vertices.Count;
                        // Adjust the triangles by the current number of vertices
                        for (int i = 0; i < tris.Count; i++)
                        {
                            tris[i] += vertexCount;
                        }
                        vertices.AddRange(verts);
                        triangles.AddRange(tris);
                    }
                }
            }
        }
    }

    public void GetVoxelAtPos(Vector3 position)
    {

    }

    void CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        //mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

        // Ensure there's a MeshCollider component
        meshCollider = gameObject.GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }
        meshCollider.sharedMesh = meshFilter.mesh;
    }














    public void SaveChunk(string savePath)
    {
        // Create a file path for this chunk based on its position
        string filePath = Path.Combine(savePath, $"Chunk_{transform.position.x}_{transform.position.y}_{transform.position.z}.dat");

        // Open a file stream to save the chunk data
        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, voxelMap);
        }
    }

    public void LoadChunk(string loadPath)
    {
        // Check if a saved file exists for this chunk
        string filePath = Path.Combine(loadPath, $"Chunk_{transform.position.x}_{transform.position.y}_{transform.position.z}.dat");

        if (File.Exists(filePath))
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                voxelMap = (bool[,,])formatter.Deserialize(fs);
            }
            // Call methods to regenerate the mesh if needed
            AddVoxelData();
            CreateMesh();
        }
        else
        {
            InitializeVoxelMap(); // Create a new voxel map if no saved file is found
        }
    }
}
