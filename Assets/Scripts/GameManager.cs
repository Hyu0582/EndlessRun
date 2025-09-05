using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    private ScoreManager scoreManager;
    public int currentLevel;
    private float gameTime;
    private float speedIncreaseRate = 0.05f;
    //private float speedIncreaseRate = 0.2f;
    private float baseSpeed = 10f;
    private float maxSpeed = 15f;
    private float currentSpeed;
    void Awake()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        // Tăng tốc độ theo thời gian
        gameTime += Time.deltaTime;
        currentSpeed = Mathf.Min(baseSpeed + (speedIncreaseRate * gameTime), maxSpeed);
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
    public void LoadLevel(int level)
    {
        SceneManager.LoadScene("Level" + "0" + level.ToString());
    }
    public void LoadRandomLevel()
    {
        int nextLevel;
        do
        {
            nextLevel = Random.Range(1, 6);
        } while (currentLevel == nextLevel);
        currentLevel = nextLevel;
        PlayerPrefs.SetInt("CurrentLevel", nextLevel);
        LoadLevel(nextLevel);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void StartGame()
    {
        Time.timeScale = 1;
        currentLevel = 1;
        PlayerPrefs.SetInt("CurrentLevel", 1);
        currentSpeed = baseSpeed;
        LoadLevel(1);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        LoadGameOver(true);
    }

    [System.Obsolete]
    public void RestartGame()
    {
        LoadGameOver(false);
        
        DontDestroy[] dontDestroys = FindObjectsOfType<DontDestroy>();
        foreach (var dontDestroy in dontDestroys)
        {
            dontDestroy.ResetState();
        }
        StartGame();
        scoreManager.ResetScore();
        
    }
    public void LoadGameOver(bool value)
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(value);
        }
        else
        {
            Debug.Log("Không có gameOverScreen");
        }
    }


    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
