using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ArrowBehaviour : MonoBehaviour, IArrowHittable
{
    public GameObject groundFireEffectGO, groundIceEffectGo, fireColliderr, iceColliderr;
    public GameObject xr_Rig;

    public void Hit(Arrow arrow)
    {
        if (FindObjectOfType<ChangeArrow>().weponNumber == 0)
        {
            //fire
            GroundFire(arrow.transform);
        }
        else if (FindObjectOfType<ChangeArrow>().weponNumber == 1)
        {
            //teleport
            GroundTeleport(arrow.transform);
        }
        else if (FindObjectOfType<ChangeArrow>().weponNumber == 2)
        {
            //poison
            ChangeArrowPos(arrow.transform,0.02f);
        }
        else if (FindObjectOfType<ChangeArrow>().weponNumber == 3)
        {
            //ice
            GroundIce(arrow.transform);
        }
    }
    private void GroundFire(Transform myTransform)
    {
        GameObject fireEffect = Instantiate(groundFireEffectGO, myTransform.position, Quaternion.identity);
        fireEffect.GetComponent<ParticleSystem>().Play();
        GameObject fireCollider = Instantiate(fireColliderr, myTransform.position, Quaternion.identity);
        Destroy(fireEffect, 15);
        Destroy(fireCollider, 13);
        ChangeArrowPos(myTransform,4);
    }
    private void GroundIce(Transform myTransform)
    {
        GameObject EffectGO = Instantiate(groundIceEffectGo, myTransform.position, Quaternion.identity);
        EffectGO.GetComponent<ParticleSystem>().Play();
        GameObject ColliderGO = Instantiate(iceColliderr, myTransform.position, Quaternion.identity);
        Destroy(EffectGO, 4);
        Destroy(ColliderGO, 4);
        ChangeArrowPos(myTransform,4);
    }
    private void GroundTeleport(Transform myTransform)
    {
        xr_Rig.transform.position = myTransform.position;
        ChangeArrowPos(myTransform,0.2f);
    }

    private void ChangeArrowPos(Transform myTransform, float time)
    {
        StartCoroutine(ChangePosE(myTransform, time));
    }
    IEnumerator ChangePosE(Transform myTransform,float time)
    {
        yield return new WaitForSeconds(time);
        myTransform.position = new Vector3(0, -500, 0);
    }

}
