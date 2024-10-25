using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject chunkPrefab;

    private GameObject player;
    public int chunkSize = 7;
    public int loadRadius = 3;

    private Vector3Int currentPlayerChunkPosition;
    private Dictionary<Vector3Int, GameObject> loadedChunks = new Dictionary<Vector3Int, GameObject>();

    private string savePath;

    void Start()
    {
        player = Instantiate(playerPrefab);
        player.GetComponent<PlayerController>().Initialize(this);
        savePath = Path.Combine(Application.persistentDataPath, "WorldData");

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        UpdateLoadedChunks();
    }

    void Update()
    {
        Vector3Int newChunkPosition = GetChunkPosition(player.transform.position);

        if (newChunkPosition != currentPlayerChunkPosition)
        {
            currentPlayerChunkPosition = newChunkPosition;
            UpdateLoadedChunks();
        }

        // Save world data on 'P' press
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveWorld();
        }
    }

    Vector3Int GetChunkPosition(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / chunkSize);
        int y = Mathf.FloorToInt(position.y / chunkSize);
        int z = Mathf.FloorToInt(position.z / chunkSize);

        return new Vector3Int(x, y, z);
    }

    void UpdateLoadedChunks()
    {
        HashSet<Vector3Int> chunksToLoad = new HashSet<Vector3Int>();

        for (int x = -loadRadius; x <= loadRadius; x++)
        {
            for (int y = -loadRadius; y <= loadRadius; y++)
            {
                for (int z = -loadRadius; z <= loadRadius; z++)
                {
                    Vector3Int chunkPosition = currentPlayerChunkPosition + new Vector3Int(x, y, z);
                    chunksToLoad.Add(chunkPosition);

                    if (!loadedChunks.ContainsKey(chunkPosition))
                    {
                        GameObject newChunk = Instantiate(chunkPrefab, chunkPosition * chunkSize, Quaternion.identity);
                        ChunkController chunkController = newChunk.GetComponent<ChunkController>();
                        chunkController.LoadChunk(savePath); // Load saved chunk data if available
                        loadedChunks[chunkPosition] = newChunk;
                    }
                }
            }
        }

        List<Vector3Int> chunksToUnload = new List<Vector3Int>();

        foreach (var chunkPosition in loadedChunks.Keys)
        {
            if (!chunksToLoad.Contains(chunkPosition))
            {
                chunksToUnload.Add(chunkPosition);
            }
        }

        foreach (var chunkPosition in chunksToUnload)
        {
            // Save chunk before unloading
            loadedChunks[chunkPosition].GetComponent<ChunkController>().SaveChunk(savePath);
            Destroy(loadedChunks[chunkPosition]);
            loadedChunks.Remove(chunkPosition);
        }
    }

    void SaveWorld()
    {
        foreach (var chunk in loadedChunks.Values)
        {
            chunk.GetComponent<ChunkController>().SaveChunk(savePath);
        }
        Debug.Log("World saved.");
    }
}
