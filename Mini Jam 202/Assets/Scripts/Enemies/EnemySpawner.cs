using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Random = UnityEngine.Random;


public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance
    { get; private set; }

    [SerializeField]
    private BoxCollider spawnCollider;

    [SerializeField]
    public GameObject enemyPrefab;

    float x, z;
    Vector3 spawnPos;

    IEnumerator spawnCor;

    public float spawnDelay = 1.0f;
    public int maxEnemies = 25;

    private int curEnemies = 0;
    private void Awake()
    {
        Instance = this;
        Enemy.Event_PlayerTouched += OnPlayerTouched;
    }

    private void OnDestroy()
    {
        Enemy.Event_PlayerTouched -= OnPlayerTouched;
    }
    private void Start()
    {
        int currentSeed = (int)DateTime.Now.Ticks;
        Random.InitState(currentSeed);
        if (spawnCollider != null) 
        {
            x = spawnCollider.size.x / 2;
            z = spawnCollider.size.z / 2;
            Vector3 pos = spawnCollider.center;
            spawnPos = this.transform.position + pos;
            spawnCor = ISpawnCoroutine();
            StartCoroutine(spawnCor);
        }
    }

    void OnPlayerTouched(object obj, EventArgs e)
    {
        StartCoroutine(spawnCor);
    }

    public void RemoveEnemy()
    {
        curEnemies--;
        if (curEnemies < 0)
            curEnemies = 0;
    }

    IEnumerator ISpawnCoroutine()
    {
        while (true) 
        {
            if (curEnemies < maxEnemies)
            {
                curEnemies++;
                Vector3 spawnOffset = new Vector3(Random.Range(-x, x), 0, Random.Range(-z, z));
                GameObject enemy = Instantiate(enemyPrefab, spawnPos + spawnOffset, Quaternion.identity);
                enemy.transform.parent = this.transform;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
