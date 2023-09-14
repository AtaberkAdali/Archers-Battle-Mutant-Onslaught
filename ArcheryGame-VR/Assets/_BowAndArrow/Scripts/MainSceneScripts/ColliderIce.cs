using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderIce : MonoBehaviour
{
    public Material IceMaterial;
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
        processContinues = 1;
        Material defaultMaterial = enemyGO.GetComponentInChildren<SkinnedMeshRenderer>().material;
        enemyGO.GetComponentInChildren<SkinnedMeshRenderer>().material = IceMaterial;
        Vector3 defaultEnemyVelocity = enemyGO.GetComponent<Rigidbody>().velocity;
        
        enemyGO.GetComponent<Rigidbody>().velocity = Vector3.zero;
        enemyGO.GetComponent<EnemyTarget>().animator.SetBool("Stop", true);
        yield return new WaitForSeconds(3f);
        processContinues = 2;
        enemyGO.GetComponentInChildren<SkinnedMeshRenderer>().material = defaultMaterial;
        enemyGO.GetComponent<EnemyTarget>().animator.SetBool("Stop", false);
        enemyGO.GetComponent<Rigidbody>().velocity = new Vector3(0,0,1);
        Debug.Log("defaultVelocity" + defaultEnemyVelocity);
    }
}
