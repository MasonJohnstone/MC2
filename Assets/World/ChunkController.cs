//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;
//using UnityEngine;

//public class ChunkController : MonoBehaviour
//{
//    //WorldController worldController;

//    //static int size = 7;

//    //bool[,,] voxelMap = new bool[size, size, size];

//    public MeshRenderer meshRenderer;
//    public MeshFilter meshFilter;
//    public MeshCollider meshCollider;

//    List<Vector3> vertices = new List<Vector3>();
//    List<int> triangles = new List<int>();
//    List<Vector2> uvs = new List<Vector2>();

//    public void Init(WorldController _worldController)
//    {
//        worldController = _worldController;
//    }

//    void Start()
//    {
//        InitializeVoxelMap();
//        AddVoxelData();
//        CreateMesh();
//    }

//    void InitializeVoxelMap()
//    {
//        for (int x = 0; x < size; x++)
//        {
//            for (int y = 0; y < size; y++)
//            {
//                for (int z = 0; z < size; z++)
//                {
//                    voxelMap[x, y, z] = Random.value < 1f / y / y;
//                }
//            }
//        }
//    }

//    void AddVoxelData()
//    {
//        for (int x = 0; x < size; x++)
//        {
//            for (int y = 0; y < size; y++)
//            {
//                for (int z = 0; z < size; z++)
//                {
//                    if (voxelMap[x, y, z])
//                    {
//                        bool[,,] surroundingVoxelIsOccupied = new bool[3, 3, 3];
//                        bool[,,] surroundingVoxelIsTransparent = new bool[3, 3, 3];
//                        for (int i = 0; i < 3; i++)
//                        {
//                            for (int j = 0; j < 3; j++)
//                            {
//                                for (int k = 0; k < 3; k++)
//                                {
//                                    surroundingVoxelIsTransparent[i, j, k] = true; // Set default to true
//                                }
//                            }
//                        }

//                        // Check all surrounding voxels in 3x3x3 block
//                        for (int dx = -1; dx <= 1; dx++)
//                        {
//                            for (int dy = -1; dy <= 1; dy++)
//                            {
//                                for (int dz = -1; dz <= 1; dz++)
//                                {
//                                    // Calculate new index in surroundingVoxels array
//                                    int sx = dx + 1;
//                                    int sy = dy + 1;
//                                    int sz = dz + 1;

//                                    // Calculate neighboring voxel coordinates in the voxelMap
//                                    int nx = x + dx, ny = y + dy, nz = z + dz;

//                                    // Check if the neighboring voxel is within the bounds
//                                    if (nx >= 0 && nx < size && ny >= 0 && ny < size && nz >= 0 && nz < size)
//                                    {
//                                        surroundingVoxelIsOccupied[sx, sy, sz] = voxelMap[nx, ny, nz];
//                                        surroundingVoxelIsTransparent[sx, sy, sz] = !voxelMap[nx, ny, nz];
//                                    }
//                                    else
//                                    {
//                                        GameObject chunk = worldController.GetChunk(worldController.GetChunkPosition(transform.position));
//                                        surroundingVoxelIsOccupied[sx, sy, sz] = true;
//                                    }
//                                }
//                            }
//                        }

//                        var verts = VoxelMeshData.GenerateVertices(surroundingVoxelIsOccupied, new Vector3(x, y, z));
//                        var tris = VoxelMeshData.GenerateTriangles(surroundingVoxelIsTransparent);
//                        int vertexCount = vertices.Count;
//                        // Adjust the triangles by the current number of vertices
//                        for (int i = 0; i < tris.Count; i++)
//                        {
//                            tris[i] += vertexCount;
//                        }
//                        vertices.AddRange(verts);
//                        triangles.AddRange(tris);
//                    }
//                }
//            }
//        }
//    }

//    void CreateMesh()
//    {
//        Mesh mesh = new Mesh();
//        mesh.vertices = vertices.ToArray();
//        mesh.triangles = triangles.ToArray();
//        //mesh.uv = uvs.ToArray();

//        mesh.RecalculateNormals();
//        meshFilter.mesh = mesh;

//        // Ensure there's a MeshCollider component
//        meshCollider = gameObject.GetComponent<MeshCollider>();
//        if (meshCollider == null)
//        {
//            meshCollider = gameObject.AddComponent<MeshCollider>();
//        }
//        meshCollider.sharedMesh = meshFilter.mesh;
//    }
//}
