using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text highScoreTxt;
    [SerializeField] private Text scoreTxt;

    private int currentScore;
    private int highScore;

    [SerializeField] private GameManager gameManager;
    private float timeElapsed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeElapsed = 0;
        currentScore = 0;
        highScore = PlayerPrefs.HasKey("HighScore") ? PlayerPrefs.GetInt("HighScore") : 0;
        DisplayScore();
    }

    void Update()
    {
        
            IncreaseScore();
            UpdateHighScore();
            DisplayScore();
        
    }

    public void IncreaseScore()
    {
        timeElapsed += Time.deltaTime;
        currentScore = (int)timeElapsed;
    }

    public void UpdateHighScore()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
            SaveHighScore();
        }
    }

    public void DisplayScore()
    {
        if(scoreTxt != null) scoreTxt.text = "Score: " + currentScore.ToString();
        if(highScoreTxt != null) highScoreTxt.text = "High Score: " + highScore.ToString();
    }

    public void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
    }
}
