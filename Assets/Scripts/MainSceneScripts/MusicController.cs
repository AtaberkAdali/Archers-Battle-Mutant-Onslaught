using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public GameObject[] clip = new GameObject[2];
    public GameObject arrowSoundEffect;
    private int currentGameObje = 0;


    void Start()
    {
        DontDestroyOnLoad(this);
        StartCoroutine(StartSound(clip[0], (float)clip[0].GetComponent<AudioSource>().clip.length));
        Debug.Log(clip[0].GetComponent<AudioSource>().clip.length);
    }
    IEnumerator StartSound(GameObject go,float time)
    {
        go.SetActive(true);
        go.GetComponent<AudioSource>().PlayOneShot(clip[currentGameObje].GetComponent<AudioSource>().clip);
        yield return new WaitForSeconds(time);
        go.SetActive(false);
        if(currentGameObje == 0) { currentGameObje = 1; }
        if(currentGameObje == 1) { currentGameObje = 0; }
        StartCoroutine(StartSound(clip[currentGameObje], (float)clip[currentGameObje].GetComponent<AudioSource>().clip.length));
    }

    public void ArrowSoundEffect()
    {
        arrowSoundEffect.GetComponent<AudioSource>().PlayOneShot(arrowSoundEffect.GetComponent<AudioSource>().clip);
    }
}
