using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    //public Transform[] spawnPoints;
    //public float spawnRate = 1f;
    public float SpawnTimerSec = 2f;
    //public float spawnTime = 0f;
    
    //public float spawnDelay = 0f;
    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    void Update()
    {
        
    }

    IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(SpawnTimerSec);
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, gameObject.transform.position, Quaternion.identity);
    }
}
