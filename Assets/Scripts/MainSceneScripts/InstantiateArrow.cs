using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateArrow : MonoBehaviour
{
    public GameObject arrowPrefab = null;
    public GameObject arrowParent = null;
    public GameObject releasedArrowParent = null;
    public GameObject[] myArrows = new GameObject[50];
    [HideInInspector] public int myArrowNum = 0;
    public void FinishedCreateArrow(Transform trans)
    {
        myArrows[myArrowNum] = Instantiate(arrowPrefab, trans.position, trans.rotation);
        myArrows[myArrowNum].transform.parent = arrowParent.transform;
    }
}
