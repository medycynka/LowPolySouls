using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SzymonPeszek.Misc;


namespace SzymonPeszek.Environment.Areas
{
    public class EnemySpawner : MonoBehaviour
    {
        public List<GameObject> prefabsList;
        public List<Vector3> positionsList;

        private List<KeyValuePair<GameObject, Vector3>> _spawnList;
        private List<GameObject> _enemiesAlive;
        private int FrameCheckRate = 3;
        private int RefreshCheckVal = 0;

        private void Start()
        {
            _enemiesAlive = new List<GameObject>();

            InitializeSpawnerList();    
        }

        public void SpawnEnemies()
        {
            StartCoroutine(RefreshSpawner());
        }

        private void ClearAliveEnemies()
        {
            foreach(var enemy_ in _enemiesAlive)
            {
                Destroy(enemy_.gameObject);
            }

            _enemiesAlive.Clear();
        }

        private void InitializeSpawnerList()
        {
            _spawnList = new List<KeyValuePair<GameObject, Vector3>>();

            for(int i = 0; i < prefabsList.Count; i++)
            {
                _spawnList.Add(new KeyValuePair<GameObject, Vector3>(prefabsList[i], positionsList[i]));
            }

            foreach(var enemyClone in _spawnList)
            {
                _enemiesAlive.Add(Instantiate(enemyClone.Key, enemyClone.Value, Quaternion.identity));
            }
        }

        private IEnumerator RefreshSpawner()
        {
            ClearAliveEnemies();

            yield return CoroutineYielder.spawnRefreshWaiter;

            foreach (var enemyClone in _spawnList)
            {
                _enemiesAlive.Add(Instantiate(enemyClone.Key, enemyClone.Value, Quaternion.identity));
            }
        }

        private void FixedUpdate()
        {
            if (Time.frameCount % FrameCheckRate == RefreshCheckVal)
            {
                _enemiesAlive.RemoveAll(enemy => enemy == null);
            }
        }
    }
}