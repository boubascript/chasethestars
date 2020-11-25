using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLayoutGenerator : MonoBehaviour
{
    public LevelChunkData[] levelChunkData;
    public LevelChunkData firstChunk;

    private LevelChunkData previousChunk;
    public GameObject objectFromChunk;

    public Transform star;
    public Transform obstacle;
    public Transform pointBoost;
    public Transform shield;

    public Vector3 spawnOrigin;

    private Vector3 spawnPosition;
    public int chunksToSpawn = 10;

    void OnEnable()
    {
        TriggerExit.OnChunkExited += PickAndSpawnChunk;
    }

    private void OnDisable()
    {
        TriggerExit.OnChunkExited -= PickAndSpawnChunk;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PickAndSpawnChunk();
        }
    }

    void Start()
    {
        obstacle.tag = "obstacle";
        pointBoost.tag = "pointBoost";
        shield.tag = "shield";
        obstacle.gameObject.GetComponent<MeshCollider>().convex = true;
        shield.gameObject.GetComponent<MeshCollider>().convex = true;
        shield.gameObject.GetComponent<MeshCollider>().isTrigger = true;
        pointBoost.gameObject.GetComponent<MeshCollider>().convex = true;
        pointBoost.gameObject.GetComponent<MeshCollider>().isTrigger = true;

        previousChunk = firstChunk;

        for (int i = 0; i < chunksToSpawn; i++)
        {
            PickAndSpawnChunk();
        }
    }
    
    LevelChunkData PickNextChunk()
    {
        List<LevelChunkData> allowedChunkList = new List<LevelChunkData>();
        LevelChunkData nextChunk = null;

        LevelChunkData.Direction nextRequiredDirection = LevelChunkData.Direction.North;

        switch (previousChunk.exitDirection)
        {
            case LevelChunkData.Direction.North:
                nextRequiredDirection = LevelChunkData.Direction.South;
                spawnPosition = spawnPosition + new Vector3(0f, 0, previousChunk.chunkSize.y);

                break;
            case LevelChunkData.Direction.East:
                nextRequiredDirection = LevelChunkData.Direction.West;
                spawnPosition = spawnPosition + new Vector3(previousChunk.chunkSize.x, 0, 0);
                break;
            case LevelChunkData.Direction.South:
                nextRequiredDirection = LevelChunkData.Direction.North;
                spawnPosition = spawnPosition + new Vector3(0, 0, -previousChunk.chunkSize.y);
                break;
            case LevelChunkData.Direction.West:
                nextRequiredDirection = LevelChunkData.Direction.East;
                spawnPosition = spawnPosition + new Vector3(-previousChunk.chunkSize.x, 0, 0);

                break;
            default:
                break;
        }

        for (int i = 0; i < levelChunkData.Length; i++)
        {
            if(levelChunkData[i].entryDirection == nextRequiredDirection)
            {
                allowedChunkList.Add(levelChunkData[i]);
            }
        }
        
        nextChunk = allowedChunkList[Random.Range(0, allowedChunkList.Count)];

        return nextChunk;

    }

    void PickAndSpawnChunk()
    {
        LevelChunkData chunkToSpawn = PickNextChunk();

        objectFromChunk = chunkToSpawn.levelChunks[Random.Range(0, chunkToSpawn.levelChunks.Length)];
        previousChunk = chunkToSpawn;
        Instantiate(objectFromChunk, spawnPosition + spawnOrigin, Quaternion.identity);
        Instantiate(star, spawnPosition + spawnOrigin + new Vector3(0, 250.0f, 0), Quaternion.identity);

        spawnObstacles();
        spawnPointBoosts();
        spawnShields();
    }

    void spawnObstacles(){
        for (int i = 0; i < 200; i++) {
            // Vector3 trackpos = objectFromChunk.transform.GetChild(20).transform.position;
            Instantiate(obstacle.gameObject, spawnOrigin + spawnPosition + new Vector3(Random.Range(-previousChunk.chunkSize.x/3,previousChunk.chunkSize.x/3), 42.0f, Random.Range(-previousChunk.chunkSize.y/3, previousChunk.chunkSize.y/3)), Quaternion.identity);
        }
    }

    void spawnPointBoosts(){
        for (int i = 0; i < 75; i++) {
            Instantiate(pointBoost, spawnOrigin + spawnPosition + new Vector3(Random.Range(-previousChunk.chunkSize.x/3,previousChunk.chunkSize.x/3), 42.0f, Random.Range(-previousChunk.chunkSize.y/3, previousChunk.chunkSize.y/3)), Quaternion.identity);
        }
    }

    void spawnShields(){
        for (int i = 0; i < 100; i++) {
            Instantiate(shield, spawnOrigin + spawnPosition + new Vector3(Random.Range(-previousChunk.chunkSize.x,previousChunk.chunkSize.x), 42.0f, Random.Range(-previousChunk.chunkSize.x, previousChunk.chunkSize.x)), Quaternion.identity);
        }
    }

    public void UpdateSpawnOrigin(Vector3 originDelta)
    {
        spawnOrigin = spawnOrigin + originDelta;
    }

}
