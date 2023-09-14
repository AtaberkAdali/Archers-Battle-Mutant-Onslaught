using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderFire : MonoBehaviour
{
    public Material fireMaterial;
    private int processContinues = 0;
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (processContinues == 0 && other.CompareTag("Enemy1"))
        {
            StartCoroutine(ChangeMaterial(other.gameObject));
        }

    }*/
    public IEnumerator ChangeMaterial(GameObject enemyGO)
    {
        Material defaultMaterial = enemyGO.GetComponentInChildren<SkinnedMeshRenderer>().material;
        enemyGO.GetComponentInChildren<SkinnedMeshRenderer>().material = fireMaterial;
        StartCoroutine(ChangeHealthSlowly(enemyGO));
        processContinues = 1;
        yield return new WaitForSeconds(3);
        processContinues = 2;
        enemyGO.GetComponentInChildren<SkinnedMeshRenderer>().material = defaultMaterial;
    }
    IEnumerator ChangeHealthSlowly(GameObject enemy)
    {
        for (int i = 0; i < 9; i++)
        {
            if (enemy != null && !enemy.GetComponent<EnemyTarget>().enemyDie)
            {
                enemy.GetComponent<EnemyTarget>().ChangeHealth(0.1f);
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                continue;
            }
        }
    }
}
