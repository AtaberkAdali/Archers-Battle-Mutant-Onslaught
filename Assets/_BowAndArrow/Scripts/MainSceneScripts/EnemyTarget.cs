using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class EnemyTarget : MonoBehaviour
{
    public Slider slider;
    public Animator animator;
    private float totalHealth = 1;
    [HideInInspector]public bool enemyDie = false;
    private GameObject arrowParent;
    private float moveSpeed = 0.5f;
    private Rigidbody rb;
    private Camera cam;
    [HideInInspector] public bool canTheEnemyMove = true;

    /*private Vector3 localDirection;
    private Quaternion localRotation;*/
    private void Start()
    {
        //slider = GetComponentInChildren<Slider>();
        //GetComponent<Rigidbody>().velocity = Vector3.forward;
        rb = GetComponent<Rigidbody>();
        cam = FindObjectOfType<Camera>();
        //GetComponent<Rigidbody>().velocity = new Vector3(0,0,0.5f);
        enemyDie = false;
        arrowParent = GameObject.Find("ArrowParent");
    }

    private void Update()
    {
        EnemyMove();
    }

    /*
    public void Hit(Arrow arrow)
    {
        Debug.Log("Arrow Pos = " + arrow.transform.position);
        Debug.Log("Arrow LocalPos2 = " + arrow.GetComponent<Transform>().localPosition);
        Debug.Log("Arrow pos2 = " + arrow.GetComponent<Transform>().position);
        
    }*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FireCollider"))
        {
            StartCoroutine(FindObjectOfType<ColliderFire>().ChangeMaterial(gameObject));

        }
        if (other.CompareTag("IceCollider"))
        {
            StartCoroutine(FindObjectOfType<ColliderIce>().ChangeMaterial(gameObject));
        }
        if (other.CompareTag("Arrow"))
        {
            if (FindObjectOfType<ChangeArrow>().weponNumber == 2)
            {
                StartCoroutine(DamageAnim());
                ChangeHealth(0.3f);
                StartCoroutine(PoisonEffectE());
                StartCoroutine(ChangeMaterial(gameObject));
            }
            if (FindObjectOfType<ChangeArrow>().weponNumber == 4)
            {
                StartCoroutine(DamageAnim());
                ChangeHealth(0.6f);
            }
        }
    }

    private void EnemyMove()
    {
        if (canTheEnemyMove)//&&pos deðiþti mi? didPlayerChangePosition ile kontrol ederek bellek kullanýmý azaltýlabilir.
        {
            // Düþmanýn kendi yönünde ilerlemesi
            rb.velocity = transform.forward * moveSpeed;

            // Düþmanýn oyuncuya doðru dönmesi
            Vector3 localDirection = (cam.transform.position - transform.position).normalized;
            Quaternion localRotation = Quaternion.LookRotation(localDirection);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, localRotation, Time.deltaTime * 3));
        }
    }

    public void ChangeHealth(float damageAmount)
    {
        totalHealth -= damageAmount;
        if (totalHealth <= 0)
        {
            Debug.Log("Diee");
            slider.value = 0;
            animator.SetBool("Die", true);
            FindObjectOfType<DieControl>().IncreaseCurrentScore(50);
            GetComponent<Rigidbody>().velocity = Vector3.zero;

            Arrow[] myArrows = GetComponentsInChildren<Arrow>();
            foreach (Arrow joint in myArrows)
            {
                joint.gameObject.transform.parent = arrowParent.transform;
                joint.transform.position = new Vector3(0, -500, 0);
            }

            Destroy(gameObject, 5);
            enemyDie = true;
        }
        else
        {
            slider.value -= damageAmount;
        }
        Debug.Log("Can Azaldý kalan: " + slider.value);
    }
    IEnumerator DamageAnim()
    {
        animator.SetBool("Damage", true);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("Damage", false);
    }
    IEnumerator PoisonEffectE()
    {
        for (int i = 0; i < 6; i++)
        {
            if (!enemyDie)
            {
                yield return new WaitForSeconds(1);
                ChangeHealth(0.1f);
            }
            else
            {
                continue;
            }
        }
    }
    IEnumerator ChangeMaterial(GameObject enemy)
    {
        Material defaultMat = enemy.GetComponentInChildren<SkinnedMeshRenderer>().material;
        enemy.GetComponentInChildren<SkinnedMeshRenderer>().material = FindObjectOfType<ChangeArrow>().posionMat;
        yield return new WaitForSeconds(6);
        enemy.GetComponentInChildren<SkinnedMeshRenderer>().material = defaultMat;
    }

}
