using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject enemyPrefab;
        //public Transform[] spawnPoints;
        //public float spawnRate = 1f;
        public float SpawnTimerSec = 5f;
        public float SpawnTimerCurrent = 2f;
        public bool NeedSpawn = false;

        public int Count = 0;
        //public float spawnDelay = 0f;
        void Start()
        {
            //StartCoroutine(SpawnCoroutine());
        }

        void Update()
        {
            if (Count > 1)
            {
                return;
            }
            if (!NeedSpawn)
            {
                SpawnTimerCurrent -= Time.deltaTime;
                if (SpawnTimerCurrent < 0)
                {
                    NeedSpawn = true;
                    SpawnEnemy();
                }
            }
        }

        public static int IdGen = 0;

        //IEnumerator SpawnCoroutine()
        //{
        //    yield return new WaitForSeconds(SpawnTimerSec);
        //    SpawnEnemy();
        //}

        private void SpawnEnemy()
        {
            Count++;
            NeedSpawn = false;
            SpawnTimerCurrent = SpawnTimerSec;
            var gameobject = Instantiate(enemyPrefab, gameObject.transform.position, Quaternion.identity);
            var controller = gameobject.GetComponent<EnemyController>();
            IdGen++;
            controller.Id = IdGen;
        }
    }
}
