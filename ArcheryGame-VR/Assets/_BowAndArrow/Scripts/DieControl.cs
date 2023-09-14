using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DieControl : MonoBehaviour
{
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI lastScoreText;
    public TextMeshProUGUI currentScoreText;
    private int _currentScoreAmount;
    private void Start()
    {
        _currentScoreAmount = 0;
        currentScoreText.text = "Current Score: " + _currentScoreAmount.ToString();
        lastScoreText.text = "Last Score: " + PlayerPrefs.GetInt("LastScore").ToString();
        bestScoreText.text = "Best Score: " + PlayerPrefs.GetInt("BestScore").ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy1"))
        {
            DieFunc();
        }
    }

    private void DieFunc()
    {
        PlayerPrefs.SetInt("LastScore", _currentScoreAmount);
        if(_currentScoreAmount >= PlayerPrefs.GetInt("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", _currentScoreAmount);
        }
        SceneManager.LoadScene(0);
    }
    public void IncreaseCurrentScore(int amount)
    {
        _currentScoreAmount += amount;
        currentScoreText.text = "Current Score: " + _currentScoreAmount.ToString();
    }
}
