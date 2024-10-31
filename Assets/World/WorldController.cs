using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    int seed;

    public GameObject playerPrefab;
    private GameObject player;
    private Vector3Int playerChunkPosition;
    
    static int chunkSize = 7;
    public int loadRadius = 35;
    int renderDistance = 10;
    public GameObject chunkPrefab;
    private Dictionary<Vector3Int, GameObject> chunkObjectCache = new Dictionary<Vector3Int, GameObject>();
    private Dictionary<Vector3Int, Chunk> chunkDataCache = new Dictionary<Vector3Int, Chunk>();
    private string savePath;

    void Start()
    {
        Debug.Log("Persistent Data Path: " + Application.persistentDataPath);
        
        player = Instantiate(playerPrefab, new Vector3(0f, 100f, 0f), transform.rotation);
        savePath = Application.persistentDataPath + "/WorldData";

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        //UpdateChunkDataCache();
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
        int x = Mathf.FloorToInt(position.x / chunkSize);
        int y = Mathf.FloorToInt(position.y / chunkSize);
        int z = Mathf.FloorToInt(position.z / chunkSize);

        return new Vector3Int(x, y, z);
    }

    void UpdateChunkDataCache()
    {
        HashSet<Vector3Int> requiredChunks = new HashSet<Vector3Int>();

        // Define the positions for chunks to be loaded within the load radius
        for (int x = -loadRadius; x <= loadRadius; x++)
        {
            for (int y = -loadRadius; y <= loadRadius; y++)
            {
                for (int z = -loadRadius; z <= loadRadius; z++)
                {
                    // Define absolute chunk position based on player chunk position
                    Vector3Int chunkPosition = playerChunkPosition + new Vector3Int(x, y, z);
                    requiredChunks.Add(chunkPosition); // Add chunk to required chunks

                    // Check if the chunk is already cached
                    if (!chunkDataCache.ContainsKey(chunkPosition))
                    {
                        // Create the file path using chunkPosition
                        string filePath = Path.Combine(savePath, $"chunk_{chunkPosition.x}_{chunkPosition.y}_{chunkPosition.z}.dat");

                        // Load the chunk from file if it exists, or create a new chunk if it doesn't
                        chunkDataCache[chunkPosition] = LoadChunk(filePath);
                    }
                }
            }
        }

        // create a list to store the positions of chunks to be removed
        List<Vector3Int> chunksToUnload = new List<Vector3Int>();

        // loop through chunks in cache
        foreach (var chunkPosition in chunkDataCache.Keys)
        {
            // if chunk isnt required in cache
            if (!requiredChunks.Contains(chunkPosition))
            {
                // save chunk data
                SaveChunk(chunkDataCache[chunkPosition], savePath);
                // add to unload list
                chunksToUnload.Add(chunkPosition);
            }
        }

        // remove chunks from cache in seperate loop so you dont remove during iteration
        foreach (var chunkPosition in chunksToUnload)
        {
            // remove chunk from cache
            chunkDataCache.Remove(chunkPosition);
        }

    }

    void UpdateChunkObjectCache()
    {
        HashSet<Vector3Int> requiredChunks = new HashSet<Vector3Int>();

        // Define the positions for chunks to be loaded within the load radius
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
                        GameObject chunkObject = Instantiate(chunkPrefab);
                        chunkObject.transform.position = chunkPosition * chunkSize;
                        chunkObject.name = $"Chunk_{chunkPosition}";
                        chunkObjectCache[chunkPosition] = chunkObject;
                    }
                }
            }
        }

        // create a list to store the positions of chunks to be removed
        List<Vector3Int> chunksToUnload = new List<Vector3Int>();

        // loop through chunks in cache
        foreach (var chunkPosition in chunkObjectCache.Keys)
        {
            // if chunk isnt required in cache
            if (!requiredChunks.Contains(chunkPosition))
            {
                // add to unload list
                chunksToUnload.Add(chunkPosition);
            }
        }

        // Remove chunks from cache in a separate loop so you don't remove during iteration
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

    // save chunk to file
    private void SaveChunk(Chunk chunk, string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        {
            BinaryWriter writer = new BinaryWriter(fs);

            // Loop through all voxels in the voxel map and save their data
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
    }

    // save all loaded chunks
    public void SaveWorld()
    {
        foreach (var chunkEntry in chunkDataCache)
        {
            Vector3Int chunkPosition = chunkEntry.Key;
            Chunk chunk = chunkEntry.Value;

            // Generate a unique file path for each chunk based on its position
            string chunkFilePath = Path.Combine(savePath, $"chunk_{chunkPosition.x}_{chunkPosition.y}_{chunkPosition.z}.dat");

            SaveChunk(chunk, chunkFilePath);
        }
    }

    // create a chunk or load from file if it already exists
    private Chunk LoadChunk(string filePath)
    {
        Chunk chunk = new Chunk();
        chunk.Init(chunkSize);

        // Check if the file exists before attempting to load
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("Chunk file not found: " + filePath);
            chunk.Generate(seed);
            return chunk;
        }
        else
        {
            Debug.LogWarning("Chunk file magically found NIGGER!!!!");
        }

        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        {
            BinaryReader reader = new BinaryReader(fs);

            // Loop through all voxel positions in the chunk and load their data
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        int type = reader.ReadInt32();
                        bool isOpaque = reader.ReadBoolean();

                        // Create the voxel and set its properties
                        Voxel voxel = new Voxel { type = type, isOpaque = isOpaque };

                        // Set the voxel in the chunk's voxel map
                        chunk.SetVoxel(new Vector3Int(x, y, z), voxel);
                    }
                }
            }
        }

        return chunk;
    }

    public Chunk GetChunkFromCacheAtPosition(Vector3Int chunkPosition)
    {
        return chunkDataCache[chunkPosition];
    }
}
