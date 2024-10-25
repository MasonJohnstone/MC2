using System.Collections;
using System.Collections.Generic;
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

    private Queue<Vector3Int> chunksToLoadQueue = new Queue<Vector3Int>();
    private bool isLoadingChunks = false;
    private string savePath;

    void Start()
    {
        player = Instantiate(playerPrefab, new Vector3(0f, 100f, 0f), transform.rotation);
        savePath = Application.persistentDataPath + "/WorldData";

        if (!System.IO.Directory.Exists(savePath))
        {
            System.IO.Directory.CreateDirectory(savePath);
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

                    if (!loadedChunks.ContainsKey(chunkPosition) && !chunksToLoadQueue.Contains(chunkPosition))
                    {
                        chunksToLoadQueue.Enqueue(chunkPosition);
                    }
                }
            }
        }

        if (!isLoadingChunks)
        {
            StartCoroutine(LoadChunksInBackground());  // Call the coroutine correctly
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
            loadedChunks[chunkPosition].GetComponent<ChunkController>().SaveChunk(savePath);
            Destroy(loadedChunks[chunkPosition]);
            loadedChunks.Remove(chunkPosition);
        }
    }

    private IEnumerator LoadChunksInBackground()
    {
        isLoadingChunks = true;

        while (chunksToLoadQueue.Count > 0)
        {
            Vector3Int chunkPosition = chunksToLoadQueue.Dequeue();
            if (!loadedChunks.ContainsKey(chunkPosition))
            {
                GameObject newChunk = Instantiate(chunkPrefab, chunkPosition * chunkSize, Quaternion.identity);
                ChunkController chunkController = newChunk.GetComponent<ChunkController>();
                chunkController.LoadChunk(savePath);
                loadedChunks[chunkPosition] = newChunk;
            }
            yield return null;
        }

        isLoadingChunks = false;
    }
}
