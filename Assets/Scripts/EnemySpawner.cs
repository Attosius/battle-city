using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
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
        public static int IdGen = 0;

        IEnumerator SpawnCoroutine()
        {
            yield return new WaitForSeconds(SpawnTimerSec);
            SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            var gameobject = Instantiate(enemyPrefab, gameObject.transform.position, Quaternion.identity);
            var controller = gameobject.GetComponent<EnemyController>();
            IdGen++;
            controller.Id = IdGen;
        }
    }
}
