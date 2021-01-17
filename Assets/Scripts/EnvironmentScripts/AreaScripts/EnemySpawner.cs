using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class EnemySpawner : MonoBehaviour
    {
        public List<GameObject> prefabsList;
        public List<Vector3> positionsList;

        List<KeyValuePair<GameObject, Vector3>> spawnList;
        List<GameObject> enemiesAlive;

        private void Start()
        {
            enemiesAlive = new List<GameObject>();

            InitializeSpawnerList();    
        }

        public void SpawnEnemies()
        {
            StartCoroutine(RefreshSpawner());
        }

        private void ClearAliveEnemies()
        {
            foreach(var enemy_ in enemiesAlive)
            {
                Destroy(enemy_.gameObject);
            }

            enemiesAlive.Clear();
        }

        private void InitializeSpawnerList()
        {
            spawnList = new List<KeyValuePair<GameObject, Vector3>>();

            for(int i = 0; i < prefabsList.Count; i++)
            {
                spawnList.Add(new KeyValuePair<GameObject, Vector3>(prefabsList[i], positionsList[i]));
            }

            foreach(var enemyClone in spawnList)
            {
                enemiesAlive.Add(Instantiate(enemyClone.Key, enemyClone.Value, Quaternion.identity));
            }
        }

        private IEnumerator RefreshSpawner()
        {
            ClearAliveEnemies();

            yield return CoroutineYielder.spawnRefreshWaiter;

            foreach (var enemyClone in spawnList)
            {
                enemiesAlive.Add(Instantiate(enemyClone.Key, enemyClone.Value, Quaternion.identity));
            }
        }

        private void FixedUpdate()
        {
            enemiesAlive.RemoveAll(enemy => enemy == null);
        }
    }
}