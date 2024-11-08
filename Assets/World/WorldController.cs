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
    public int chunkSize = 10;
    [HideInInspector]
    public float voxelSize = 1f;
    int loadRadius = 5;
    int renderDistance = 4;

    public GameObject chunkPrefab;
    private Dictionary<Vector3Int, GameObject> chunkObjectCache = new Dictionary<Vector3Int, GameObject>();
    public Dictionary<Vector3Int, Chunk> chunkDataCache = new Dictionary<Vector3Int, Chunk>();
    private string savePath;
    ChunkRenderer chunkRenderer;
    private List<Vector3Int> requiredChunkPositions = new List<Vector3Int>();

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

        // update stored player chunk pos
        playerChunkPosition = GetChunkPosition(player.transform.position);
        // update chunks to be loaded as runtime variables
        UpdateChunkDataCache();
        UpdateRequiredChunkObjects();
        StartCoroutine(ManageChunkObjectsCoroutine());
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
            UpdateRequiredChunkObjects();
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
                SaveChunk(chunkDataCache[chunkPosition], filePath);
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

    void UpdateRequiredChunkObjects()
    {
        List<Vector3Int> requiredChunks = new List<Vector3Int>();

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
                }
            }
        }

        // Sort chunks by their distance to the player position (ascending)
        requiredChunks.Sort((a, b) =>
            Vector3Int.Distance(playerChunkPosition, a).CompareTo(Vector3Int.Distance(playerChunkPosition, b))
        );

        // Update the class-level requiredChunkPositions with the sorted list
        requiredChunkPositions = requiredChunks;
    }

    IEnumerator ManageChunkObjectsCoroutine()
    {
        while (true) // This loop will run indefinitely
        {
            List<Vector3Int> chunksToUnload = new List<Vector3Int>();

            // Unload chunks that are no longer needed
            foreach (var chunkPosition in chunkObjectCache.Keys)
            {
                if (!requiredChunkPositions.Contains(chunkPosition))
                {
                    chunksToUnload.Add(chunkPosition);
                }
            }

            // Unload one chunk per coroutine iteration
            foreach (var chunkPosition in chunksToUnload)
            {
                if (chunkObjectCache.TryGetValue(chunkPosition, out GameObject chunkObject))
                {
                    Destroy(chunkObject);
                    chunkObjectCache.Remove(chunkPosition);
                    yield return null; // Pause coroutine to avoid frame drops
                }
            }

            int chunksCreated = 0;

            // Create required chunks up to a limit of 7 at a time, in order of proximity
            foreach (var chunkPosition in requiredChunkPositions)
            {
                if (!chunkObjectCache.ContainsKey(chunkPosition) && chunksCreated < 7)
                {
                    float worldChunkSize = chunkSize * voxelSize;

                    // Instantiate the chunk GameObject and set its world position
                    GameObject chunkObject = Instantiate(chunkPrefab);
                    chunkObject.transform.position = new Vector3(
                        chunkPosition.x * worldChunkSize,
                        chunkPosition.y * worldChunkSize,
                        chunkPosition.z * worldChunkSize
                    );
                    chunkObject.name = $"Chunk_{chunkPosition}";

                    // Set mesh data for the new chunk
                    chunkRenderer.SetMeshData(this, chunkDataCache[chunkPosition], chunkPosition, chunkObject);
                    chunkObjectCache[chunkPosition] = chunkObject;

                    chunksCreated++;
                    yield return null; // Pause coroutine to avoid frame drops
                }
            }

            // Wait a short time before repeating the loop
            yield return new WaitForSeconds(0.5f); // Adjust as needed
        }
    }

    private void SaveChunk(Chunk chunk, string filePath)
    {
        string tempFilePath = filePath + ".tmp"; // Define tempFilePath here to use in all parts of the function

        try
        {
            string directory = Path.GetDirectoryName(filePath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Save to a temporary file
            using (FileStream fs = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
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
                            writer.Write(voxel.density);
                        }
                    }
                }
            }

            // Use File.Replace to overwrite the existing file safely
            if (File.Exists(filePath))
            {
                File.Replace(tempFilePath, filePath, null); // Replace existing file with temp file
            }
            else
            {
                File.Move(tempFilePath, filePath); // Move if no existing file
            }

            Debug.Log($"Chunk saved successfully at {filePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save chunk at {filePath}. Error: {ex.Message}");

            // Attempt to delete the .tmp file if the save failed
            try
            {
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                    Debug.Log($"Temporary file {tempFilePath} deleted after failed save.");
                }
            }
            catch (Exception deleteEx)
            {
                Debug.LogError($"Failed to delete temporary file {tempFilePath}. Error: {deleteEx.Message}");
            }
        }
    }
    
    public void SaveWorld()
    {
        foreach (var chunkEntry in chunkDataCache)
        {
            Vector3Int chunkPosition = chunkEntry.Key;
            Chunk chunk = chunkEntry.Value;

            string chunkFilePath = Path.Combine(savePath, $"chunk_{chunkPosition.x}_{chunkPosition.y}_{chunkPosition.z}.dat");
            SaveChunk(chunk, chunkFilePath);
        }
    }
    
    private Chunk LoadChunk(string filePath, Vector3Int chunkPosition)
    {
        Chunk chunk = new Chunk();
        Voxel[,,] voxelMap = new Voxel[chunkSize, chunkSize, chunkSize];

        if (!File.Exists(filePath))
        {
            // If the file doesn't exist, generate a new voxel map
            voxelMap = GenerateChunkVoxelMap(100f, 0.05f, 0.1f, 0.5f, chunkPosition);
        }
        else
        {
            // Load voxel data from file
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
                            float density = reader.ReadSingle();

                            Voxel voxel = new Voxel { type = type, density = density };
                            voxelMap[x, y, z] = voxel;
                        }
                    }
                }
            }

            Debug.Log($"Chunk loaded successfully from {filePath}");
        }

        // Initialize the chunk with the generated or loaded voxel map
        chunk.Init(voxelMap);
        return chunk;
    }

    Voxel[,,] GenerateChunkVoxelMap(float terrainHeight, float terrainFrequency, float tunnelFrequency, float tunnelThreshold, Vector3Int chunkPosition)
    {
        // Initialize the Voxel map array
        Voxel[,,] voxelMap = new Voxel[chunkSize, chunkSize, chunkSize];

        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                // Calculate global positions in world space using voxelSize
                float globalX = (x + chunkPosition.x * chunkSize) * voxelSize;
                float globalZ = (z + chunkPosition.z * chunkSize) * voxelSize;

                // Apply 2D Perlin noise for terrain height, yielding a natural continuous density
                float terrainHeightAtXZ = Mathf.PerlinNoise(globalX * terrainFrequency, globalZ * terrainFrequency) * terrainHeight;

                for (int y = 0; y < chunkSize; y++)
                {
                    // Calculate global Y position in world space using voxelSize
                    float globalY = (y + chunkPosition.y * chunkSize) * voxelSize;

                    // Base density based on height map
                    float heightDensity = Mathf.Clamp01((terrainHeightAtXZ - globalY) / terrainHeight);

                    // 3D Perlin noise for tunnels based on world-space coordinates
                    float tunnelNoise = Mathf.PerlinNoise(globalX * tunnelFrequency, globalY * tunnelFrequency)
                                      * Mathf.PerlinNoise(globalY * tunnelFrequency, globalZ * tunnelFrequency)
                                      * Mathf.PerlinNoise(globalX * tunnelFrequency, globalZ * tunnelFrequency);

                    // Adjust density smoothly based on tunnel noise and threshold
                    float tunnelDensity = Mathf.Clamp01((tunnelThreshold - tunnelNoise) / tunnelThreshold);

                    // Final density combines height density with tunnel modification
                    float noise = heightDensity * tunnelDensity;

                    float density = noise;

                    // Determine voxel type based on density threshold
                    int type = (noise > 0.5f) ? 1 : 0;

                    // Create voxel with the calculated type and density and set it in the voxel map
                    Voxel voxel = new Voxel { type = type, density = density };
                    voxelMap[x, y, z] = voxel;
                }
            }
        }

        // Return the populated voxel map
        return voxelMap;
    }

    public Chunk GetChunkFromCacheAtPosition(Vector3Int chunkPosition)
    {
        return chunkDataCache[chunkPosition];
    }

    private void OnApplicationQuit()
    {
        SaveWorld();
    }

}
