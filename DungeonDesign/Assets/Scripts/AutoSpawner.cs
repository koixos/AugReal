using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject spawnedAI;
    public Transform player;

    [Header("ProximityDetection")]
    public float detectionDistance = 3f;
    public LayerMask obstacleLayerMask = -1;

    [Header("Stuck Detection")]
    public float stuckTh = 0.1f;
    public float stuckTimeLimit = 3f;

    [Header("General")]
    public float spawnCooldown = 5f;

    private Vector3 lastPlayerPos;
    private float timeSinceLastMove;
    private float lastSpawnTime;
    private bool hasSpawnedForCurrObstacle;

    void Start()
    {
        lastPlayerPos = player.position;
    }

    void Update()
    {
        bool shouldSpawn = CheckProximityCond() || CheckStuckCond();

        if (shouldSpawn && !hasSpawnedForCurrObstacle && Time.time - lastSpawnTime > spawnCooldown)
        {
            ActivateAI();
            hasSpawnedForCurrObstacle = true;
            lastSpawnTime = Time.time;
        }
        else if (!shouldSpawn)
        {
            hasSpawnedForCurrObstacle = false;
        }
    }

    void ActivateAI()
    {
        spawnedAI.SetActive(true);
    }

    bool CheckProximityCond()
    {
        Collider[] nearbyObjs = Physics.OverlapSphere(player.position, detectionDistance, obstacleLayerMask);
        return nearbyObjs.Length > 0;
    }

    bool CheckStuckCond()
    {
        float distMoved = Vector3.Distance(player.position, lastPlayerPos);

        if (distMoved < stuckTh)
        {
            timeSinceLastMove += Time.deltaTime;
            return timeSinceLastMove >= stuckTimeLimit;
        }
        else
        {
            timeSinceLastMove = 0f;
            lastPlayerPos = player.position;
            return false;
        }
    }
}
