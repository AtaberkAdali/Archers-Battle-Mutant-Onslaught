using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class ChangeArrow : MonoBehaviour
{//Mesela 10 zehir 10 ateþ ve 10 dondurma hakkýn olsun. sonrasýnda bunlarý ufak bir þekilde sollarýna yaz. burada da makewhite yaparken ona göre çalýþmasýn.
    public InputActionReference changeWeponInput;
    public InputActionReference changeDefaultWeponInput;
    public GameObject[] images = new GameObject[4];
    public GameObject ChangeArrowImageParent = null;
    public GameObject[] arrowParticalEffect = new GameObject[4];
    public GameObject[] bowParticals = new GameObject[10];//PullMeasurer için.

    public Material[] tipMaterials;
    public Material[] bodyMaterials;

    private Color defaultImageColor;

    private MeshRenderer[] arrowMeshComponentsCurrent;//Þu an elimizde olaný deðiþtirmek için
    private MeshRenderer[] arrowMeshComponentsPrefab;//Bir sonraki çekeceðimiz oku ayný yapmak için

    [HideInInspector]
    public int weponNumber = 0;
    private GameObject tip;
    [HideInInspector]
    public bool haveAnyChange = false;
    private bool canIClosePanel = true;

    public Material posionMat;// enemy target'de yük olup defalarca çaðýrýlmasýn diye.


    private void Awake()
    {
        MakeWhite(4);
    }

    // Start is called before the first frame update
    void Start()
    {
        changeWeponInput.action.started += ChangeObject;
        changeWeponInput.action.canceled -= ChangeObject;
        changeWeponInput.action.canceled += SelectObject;
        changeDefaultWeponInput.action.started += ChangeArrowToDefault;
        changeDefaultWeponInput.action.canceled -= ChangeArrowToDefault;
    }

    private void SelectObject(InputAction.CallbackContext obj)
    {
        Debug.Log("Select obje çalýþtý");
        if(canIClosePanel)
            StartCoroutine(ClosePanelE());
    }

    private void ChangeArrowToDefault(InputAction.CallbackContext ctx)
    {
        Debug.Log("Wepon Num = " + weponNumber);
        weponNumber = 4;
        MakeWhiteDefault(4);
        
    }

    private void ChangeObject(InputAction.CallbackContext ctx)
    {
        //AllObjectBlack(images);
        if(ctx.ReadValue<Vector2>().x > 0)
        {
            if(ctx.ReadValue<Vector2>().y > 0)
            {
                //sag üst
                MakeWhite(1);
            }
            else
            {
                //sað alt
                MakeWhite(3);
            }
        }
        else
        {
            if (ctx.ReadValue<Vector2>().y > 0)
            {
                //sol üst
                MakeWhite(0);
            }
            else
            {
                //sol alt
                MakeWhite(2);
            }
        }
        Debug.Log("change");

    }

    private void MakeWhite(int GOIndex)
    {
        OpenPanel();
        ChangeImageColor(GOIndex);
        ChangeMatColor(tipMaterials[GOIndex], bodyMaterials[GOIndex]);
        MakeParticleSystem();
        haveAnyChange = true;
    }
    private void MakeWhiteDefault(int GOIndex)
    {
        ChangeImageColor(GOIndex);
        ChangeMatColor(tipMaterials[GOIndex], bodyMaterials[GOIndex]);
        for (int i = 0; i < 4; i++)
        {
            arrowParticalEffect[i].SetActive(false);
        }
        haveAnyChange = true;
    }

    private void ChangeImageColor(int index)
    {
        for (int i = 0; i < images.Length; i++)
        {
            if (index == i)
            {
                defaultImageColor = images[i].GetComponent<Image>().color;
                images[i].GetComponent<Image>().color = new Color(defaultImageColor.r, defaultImageColor.g, defaultImageColor.b, 1);
                images[i].GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                weponNumber = i;
            }
            else
            {
                defaultImageColor = images[i].GetComponent<Image>().color;
                images[i].GetComponent<Image>().color = new Color(defaultImageColor.r, defaultImageColor.g, defaultImageColor.b, 0.5f);
                images[i].GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.5f);
            }
        }
    }
    private void OpenPanel()
    {
        ChangeArrowImageParent.SetActive(true);
    }
    IEnumerator ClosePanelE()
    {
        canIClosePanel = false;
        yield return new WaitForSeconds(0.4f);
        ChangeArrowImageParent.SetActive(false);
        canIClosePanel = true;
    }

    public void MakeParticleSystem()
    {
        if (FindObjectOfType<Quiver>().currentArrow != null)
        {
            arrowMeshComponentsCurrent = FindObjectOfType<Quiver>().currentArrow.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer joint in arrowMeshComponentsCurrent)
            {
                if (joint.gameObject.name == "Tip")
                {
                    tip = joint.gameObject;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (i == weponNumber)
                {
                    //Debug.Log("Select");
                    arrowParticalEffect[i].SetActive(true);
                    arrowParticalEffect[i].transform.position = tip.transform.position;
                    arrowParticalEffect[i].transform.parent = tip.transform;
                }
                else
                {
                    arrowParticalEffect[i].SetActive(false);
                }
            }
        }
    }

    private void ChangeMatColor(Material tipMat, Material bodyMat)
    {
        if(FindObjectOfType<Quiver>().currentArrow != null)
        {
            arrowMeshComponentsCurrent = FindObjectOfType<Quiver>().currentArrow.GetComponentsInChildren<MeshRenderer>();
            ChangeMatColorVoidFunction(arrowMeshComponentsCurrent, tipMat, bodyMat);
        }

        arrowMeshComponentsPrefab = FindObjectOfType<Quiver>().arrowPrefab.GetComponentsInChildren<MeshRenderer>();// Yeterince okun yoksa default oka döndür.
        ChangeMatColorVoidFunction(arrowMeshComponentsPrefab, tipMat, bodyMat);
    }

    #region ChangeHalperFunctions
    private void ChangeMatColorVoidFunction(MeshRenderer[] meshArray, Material tipMat, Material bodyMat)
    {
        foreach (MeshRenderer joint in meshArray)
        {
            if (joint.gameObject.name == "Shaft")
            {
                joint.material = bodyMat;//body
            }
            if (joint.gameObject.name == "Tip")
            {
                joint.material = tipMat;//Tip
                tip = joint.gameObject;
            }
        }
    }
    #endregion


}
