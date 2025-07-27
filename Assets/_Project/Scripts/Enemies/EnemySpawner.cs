using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")] [SerializeField]
    private GameObject bugPrefab;

    [SerializeField] private GameObject birdPrefab;
    [SerializeField] private GameObject corruptedBugPrefab;
    [SerializeField] private GameObject corruptedBirdPrefab;
    [SerializeField] private GameObject squirrelPrefab;

    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnRadius = 8f;

    private Transform player;
    private float timer;
    private float gameTimer;

    private List<GameObject> activeEnemies = new();

    private void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
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

        var index = Random.Range(0, activeEnemies.Count);
        var spawnPosition = (Vector2)player.position + Random.insideUnitCircle.normalized * spawnRadius;
        Instantiate(activeEnemies[index], spawnPosition, Quaternion.identity);
    }

    private void UpdateActiveEnemies(float time)
    {
        activeEnemies.Clear();

        if (time < 30f)
        {
            activeEnemies.Add(bugPrefab);
        }
        else if (time < 90f)
        {
            activeEnemies.Add(bugPrefab);
            activeEnemies.Add(birdPrefab);
        }
        else if (time < 180f)
        {
            activeEnemies.Add(corruptedBugPrefab);
            activeEnemies.Add(birdPrefab);
        }
        else if (time < 240f)
        {
            activeEnemies.Add(corruptedBugPrefab);
            activeEnemies.Add(corruptedBirdPrefab);
        }
        else
        {
            activeEnemies.Add(corruptedBugPrefab);
            activeEnemies.Add(corruptedBirdPrefab);
            activeEnemies.Add(squirrelPrefab);
        }
    }
}