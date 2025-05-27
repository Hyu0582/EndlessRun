using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    public void LoadLevel01()
    {
        SceneManager.LoadScene("Level01");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void StartGame()
    {
        Time.timeScale = 1;
        LoadLevel01();
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        LoadGameOver();
    }
    public void LoadGameOver()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
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
