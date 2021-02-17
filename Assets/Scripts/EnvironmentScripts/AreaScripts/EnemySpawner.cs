using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SzymonPeszek.Misc;


namespace SzymonPeszek.Environment.Areas
{
    public class EnemySpawner : MonoBehaviour
    {
        public Transform parentTransform;
        public List<GameObject> prefabsList;
        public List<Vector3> positionsList;

        private List<GameObject> _enemiesAlive;
        private readonly int FrameCheckRate = 3;
        private const int RefreshCheckVal = 0;

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
            for (var i = 0; i < _enemiesAlive.Count; i++)
            {
                Destroy(_enemiesAlive[i].gameObject);
            }

            _enemiesAlive.Clear();
        }

        private void InitializeSpawnerList()
        {
            for (int i = 0; i < prefabsList.Count; i++)
            {
                _enemiesAlive.Add(Instantiate(prefabsList[i], positionsList[i], Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f), parentTransform));
            }
        }

        private IEnumerator RefreshSpawner()
        {
            ClearAliveEnemies();

            yield return CoroutineYielder.spawnRefreshWaiter;

            for (int i = 0; i < prefabsList.Count; i++)
            {
                _enemiesAlive.Add(Instantiate(prefabsList[i], positionsList[i], Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f), parentTransform));
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