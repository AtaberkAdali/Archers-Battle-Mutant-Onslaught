using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnTime;
    public float spawnAmount;
    private int currentTotalCreatedEnemyAmount = 1;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        StartCoroutine(SpawnEnemyE());
    }

    [System.Obsolete]
    IEnumerator SpawnEnemyE()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            yield return new WaitForSeconds(spawnTime - (float)Mathf.Log(currentTotalCreatedEnemyAmount, 6));
            Vector3 newSpawnPoint = gameObject.transform.position;
            newSpawnPoint.x += Random.RandomRange(-30, 30);
            Instantiate(enemyPrefab, newSpawnPoint, Quaternion.identity);
            Debug.Log((spawnTime - (float)Mathf.Log(currentTotalCreatedEnemyAmount, 5)));
            currentTotalCreatedEnemyAmount++;
        }

    }
}
