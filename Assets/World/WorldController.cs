using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;

using System;

public class WorldController : MonoBehaviour
{
    int seed;

    public GameObject playerPrefab;
    private GameObject player;
    private Vector3Int playerChunkPosition;

    [HideInInspector]
    public int chunkSize = 16;
    [HideInInspector]
    public float voxelSize = 0.33f;
    int loadRadius = 5;
    int renderDistance = 4;

    public GameObject chunkPrefab;
    private Dictionary<Vector3Int, GameObject> chunkObjectCache = new Dictionary<Vector3Int, GameObject>();
    public Dictionary<Vector3Int, Chunk> chunkDataCache = new Dictionary<Vector3Int, Chunk>();
    private string savePath;
    ChunkRenderer chunkRenderer;

    void Start()
    {
        Debug.Log("Persistent Data Path: " + Application.persistentDataPath);
        
        player = Instantiate(playerPrefab, new Vector3(0f, 0f, 0f), transform.rotation);
        savePath = Path.Combine(Application.persistentDataPath, "WorldData");
        chunkRenderer = new ChunkRenderer();

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        //Chunk chunk = new Chunk();
        //chunk.Init(chunkSize);
        //SaveChunk(chunk, savePath);

        // update stored player chunk pos
        playerChunkPosition = GetChunkPosition(player.transform.position);
        // update chunks to be loaded as runtime variables
        UpdateChunkDataCache();
        UpdateChunkObjectCache();
    }

    void Update()
    {
        // if player stored chunk pos different to actual chunk pos
        if (playerChunkPosition != GetChunkPosition(player.transform.position))
        {
            // update stored player chunk pos
            playerChunkPosition = GetChunkPosition(player.transform.position);
            // update chunks to be loaded as runtime variables
            UpdateChunkDataCache();
            UpdateChunkObjectCache();
        }
    }

    public Vector3Int GetChunkPosition(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / (chunkSize * voxelSize));
        int y = Mathf.FloorToInt(position.y / (chunkSize * voxelSize));
        int z = Mathf.FloorToInt(position.z / (chunkSize * voxelSize));

        return new Vector3Int(x, y, z);
    }

    void UpdateChunkDataCache()
    {
        HashSet<Vector3Int> requiredChunks = new HashSet<Vector3Int>();

        // Calculate actual chunk size in world space
        float worldChunkSize = chunkSize * voxelSize;

        // Define the positions for chunks to be loaded within the load radius
        for (int x = -loadRadius; x <= loadRadius; x++)
        {
            for (int y = -loadRadius; y <= loadRadius; y++)
            {
                for (int z = -loadRadius; z <= loadRadius; z++)
                {
                    // Define absolute chunk position based on player chunk position
                    Vector3Int chunkPosition = playerChunkPosition + new Vector3Int(x, y, z);

                    // Convert chunkPosition to world space using voxel size
                    Vector3Int worldChunkPosition = new Vector3Int(
                        Mathf.FloorToInt(chunkPosition.x * worldChunkSize),
                        Mathf.FloorToInt(chunkPosition.y * worldChunkSize),
                        Mathf.FloorToInt(chunkPosition.z * worldChunkSize)
                    );

                    requiredChunks.Add(chunkPosition); // Add chunk to required chunks

                    // Check if the chunk is already cached
                    if (!chunkDataCache.ContainsKey(chunkPosition))
                    {
                        string filePath = Path.Combine(savePath, $"chunk_{chunkPosition.x}_{chunkPosition.y}_{chunkPosition.z}.dat");
                        chunkDataCache[chunkPosition] = LoadChunk(filePath, chunkPosition);
                    }
                }
            }
        }

        // Create a list to store the positions of chunks to be removed
        List<Vector3Int> chunksToUnload = new List<Vector3Int>();

        // Loop through chunks in cache
        foreach (var chunkPosition in chunkDataCache.Keys)
        {
            // If chunk isn't required in cache
            if (!requiredChunks.Contains(chunkPosition))
            {
                string filePath = Path.Combine(savePath, $"chunk_{chunkPosition.x}_{chunkPosition.y}_{chunkPosition.z}.dat");
                //SaveChunk(chunkDataCache[chunkPosition], filePath);
                chunksToUnload.Add(chunkPosition);
            }
        }

        // Remove chunks from cache in a separate loop to avoid modifying the collection during iteration
        foreach (var chunkPosition in chunksToUnload)
        {
            // Remove chunk from cache
            chunkDataCache.Remove(chunkPosition);
        }
    }

    void UpdateChunkObjectCache()
    {
        HashSet<Vector3Int> requiredChunks = new HashSet<Vector3Int>();

        // Calculate the actual world space chunk size based on voxel size
        float worldChunkSize = chunkSize * voxelSize;

        // Define the positions for chunks to be loaded within the render distance
        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int y = -renderDistance; y <= renderDistance; y++)
            {
                for (int z = -renderDistance; z <= renderDistance; z++)
                {
                    // Define absolute chunk position based on player chunk position
                    Vector3Int chunkPosition = playerChunkPosition + new Vector3Int(x, y, z);
                    requiredChunks.Add(chunkPosition); // Add chunk to required chunks

                    // Check if the chunk is already cached
                    if (!chunkObjectCache.ContainsKey(chunkPosition))
                    {
                        // Instantiate the chunk GameObject and set its world position based on chunk size and voxel size
                        GameObject chunkObject = Instantiate(chunkPrefab);
                        chunkObject.transform.position = new Vector3(
                            chunkPosition.x * worldChunkSize,
                            chunkPosition.y * worldChunkSize,
                            chunkPosition.z * worldChunkSize
                        );
                        chunkObject.name = $"Chunk_{chunkPosition}";

                        // Set mesh data based on the calculated chunk position and chunk data
                        chunkRenderer.SetMeshData(this, chunkDataCache[chunkPosition], chunkPosition, chunkObject);
                        chunkObjectCache[chunkPosition] = chunkObject;
                    }
                }
            }
        }

        // Create a list to store the positions of chunks to be removed
        List<Vector3Int> chunksToUnload = new List<Vector3Int>();

        // Loop through chunks in cache
        foreach (var chunkPosition in chunkObjectCache.Keys)
        {
            // If chunk isn't required in cache
            if (!requiredChunks.Contains(chunkPosition))
            {
                // Add to unload list
                chunksToUnload.Add(chunkPosition);
            }
        }

        // Remove chunks from cache in a separate loop to avoid modifying the collection during iteration
        foreach (var chunkPosition in chunksToUnload)
        {
            // Retrieve the GameObject associated with the chunk position
            if (chunkObjectCache.TryGetValue(chunkPosition, out GameObject chunkObject))
            {
                // Destroy the GameObject associated with this chunk
                Destroy(chunkObject);

                // Remove the chunk from the dictionary
                chunkObjectCache.Remove(chunkPosition);
            }
        }
    }

    private void SaveChunk(Chunk chunk, string filePath)
    {
        return;
        try
        {
            string directory = Path.GetDirectoryName(filePath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                for (int x = 0; x < chunkSize; x++)
                {
                    for (int y = 0; y < chunkSize; y++)
                    {
                        for (int z = 0; z < chunkSize; z++)
                        {
                            Voxel voxel = chunk.GetVoxel(new Vector3Int(x, y, z));
                            writer.Write(voxel.type);
                            writer.Write(voxel.isOpaque);
                        }
                    }
                }

            }

            Debug.Log($"Chunk saved successfully at {filePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save chunk at {filePath}. Error: {ex.Message}");
        }
    }

    // save all loaded chunks
    public void SaveWorld()
    {
        foreach (var chunkEntry in chunkDataCache)
        {
            Vector3Int chunkPosition = chunkEntry.Key;
            Chunk chunk = chunkEntry.Value;

            string chunkFilePath = Path.Combine(savePath, $"chunk_{chunkPosition.x}_{chunkPosition.y}_{chunkPosition.z}.dat");
            //SaveChunk(chunk, chunkFilePath);
        }
    }


    // create a chunk or load from file if it already exists
    private Chunk LoadChunk(string filePath, Vector3Int chunkPosition)
    {
        Chunk chunk = new Chunk();
        chunk.Init(chunkSize);

        if (!File.Exists(filePath))
        {
            chunk.Generate(this, chunkSize, 100f, 0.01f, 0.1f, 0.5f, chunkPosition);
            return chunk;
        }

        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        using (BinaryReader reader = new BinaryReader(fs))
        {
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        int type = reader.ReadInt32();
                        bool isOpaque = reader.ReadBoolean();

                        Voxel voxel = new Voxel { type = type, isOpaque = isOpaque };
                        chunk.SetVoxel(new Vector3Int(x, y, z), voxel);
                    }
                }
            }
        }

        Debug.Log($"Chunk loaded successfully from {filePath}");
        return chunk;
    }


    public Chunk GetChunkFromCacheAtPosition(Vector3Int chunkPosition)
    {
        return chunkDataCache[chunkPosition];
    }
}
