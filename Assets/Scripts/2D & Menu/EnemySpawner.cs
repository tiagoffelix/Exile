using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject bossPrefab;

    [SerializeField] private PlayerStats playerStats;

    void Start()
    {

        if(playerStats.BossFight == false){ 
            SpawnEnemies(); 
        }
        else {
            Vector2 spawnPosition = new Vector2(10f, 0f);
            GameObject boss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        }
        
    }

    public void SpawnEnemies()
    {
        float minX = -14f;
        float maxX = 14f;
        float minY = -6f;
        float maxY = 6f;
        float minDistanceFromCenter = 2.5f;

        for (int i = 0; i < playerStats.EnemiesAlive; i++)
        {
            Vector2 spawnPosition = Vector2.zero;
            float distanceFromCenter = 0f;

            do
            {
                spawnPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                distanceFromCenter = Vector2.Distance(spawnPosition, Vector2.zero);
            } while (distanceFromCenter < minDistanceFromCenter);

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}