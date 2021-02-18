using System.Collections.Generic;
using UnityEngine;


namespace SzymonPeszek.Misc
{
    public class RandomPositionTester : MonoBehaviour
    {
        public Bounds bounds;
        public GameObject testPrefab;
        public bool useBounds;
        [Range(1, 25000)] public int objectsAmount = 1;
        public Terrain worldTerrain;
        public LayerMask terrainLayer;
        private Vector3 _centerPoint;
        private List<GameObject> _createdObjects = new List<GameObject>();

        public void CreateObjectAtRandomPosition()
        {
            Vector3[] points = useBounds
                ? ExtensionMethods.GetRandomPointsOnSurfaceInBoundsNonRefFast(bounds, terrainLayer,
                    objectsAmount)
                : ExtensionMethods.GetRandomPointsOnSurfaceNonRef(worldTerrain.transform.position,
                    worldTerrain.terrainData.size, terrainLayer, objectsAmount);

            for (int i = 0; i < objectsAmount; i++)
            {
                _createdObjects.Add(Instantiate(testPrefab, points[i], Quaternion.identity));
            }
        }

        public void DeleteCreatedObjects()
        {
            if (_createdObjects.Count > 0)
            {
                for (int i = 0; i < _createdObjects.Count; i++)
                {
                    DestroyImmediate(_createdObjects[i]);
                }

                _createdObjects.Clear();
            }
        }
    }
}