using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject bugPrefab;
    [SerializeField] private GameObject birdPrefab;
    [SerializeField] private GameObject corruptedBugPrefab;
    [SerializeField] private GameObject corruptedBirdPrefab;
    [SerializeField] private GameObject squirrelPrefab;

    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnRadius = 8f;

    private Transform player;
    private float timer;
    private float gameTimer;

    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        UpdateActiveEnemies(0); // Start with bugs only
    }

    private void Update()
    {
        if (player == null) return;

        gameTimer += Time.deltaTime;
        timer += Time.deltaTime;

        UpdateActiveEnemies(gameTimer);

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        if (activeEnemies.Count == 0) return;

        int index = Random.Range(0, activeEnemies.Count);
        Vector2 spawnPosition = (Vector2)player.position + Random.insideUnitCircle.normalized * spawnRadius;
        Instantiate(activeEnemies[index], spawnPosition, Quaternion.identity);
    }

    private void UpdateActiveEnemies(float time)
    {
        activeEnemies.Clear();

        if (time < 90f)
        {
            activeEnemies.Add(bugPrefab);
        }
        else if (time < 180f)
        {
            activeEnemies.Add(bugPrefab);
            activeEnemies.Add(birdPrefab);
        }
        else if (time < 300f)
        {
            activeEnemies.Add(corruptedBugPrefab);
            activeEnemies.Add(birdPrefab);
        }
        else if (time < 420f)
        {
            activeEnemies.Add(corruptedBugPrefab);
            activeEnemies.Add(birdPrefab);
            activeEnemies.Add(corruptedBirdPrefab);
        }
        else if (time < 540f)
        {
            activeEnemies.Add(corruptedBugPrefab);
            activeEnemies.Add(corruptedBirdPrefab);
            activeEnemies.Add(squirrelPrefab);
        }
    }
}
