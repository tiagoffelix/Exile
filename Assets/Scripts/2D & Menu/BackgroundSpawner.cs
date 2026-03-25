using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;

    void Start()
    {
        float minX = -20f;
        float maxX = 20f;
        float minY = -7f;
        float maxY = 7f;

        foreach (GameObject prefab in prefabs)
        {
            for (int i = 0; i < 7; i++)
            {
                Vector2 spawnPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                GameObject spawnedPrefab = Instantiate(prefab, spawnPosition, Quaternion.identity);
            }
        }
    } 
}
