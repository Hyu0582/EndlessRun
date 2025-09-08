using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Exceptions;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text highScoreTxt;
    [SerializeField] private Text scoreTxt;

    private int currentScore;
    private int highScore;
    private float timeElapsed;

    [SerializeField] private GameManager gameManager;
    private LeaderBoardManager leaderBoardManager;
    private string playerId;
    private const string LeaderboardId = "HighScoreLB";
    private const string anonymous = "IsAnonymous";
    private PlayerController player;

    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        leaderBoardManager = FindAnyObjectByType<LeaderBoardManager>();
        player = FindFirstObjectByType<PlayerController>();
        Debug.Log("LeaderBoardManager found: " + (leaderBoardManager != null));
    }
    // Gửi HighScore lên leaderboard
    public async Task UpdateLeaderboardScore(int highScore)
    {
        try
        {
            if (PlayerPrefs.GetInt($"HighScore_{playerId}") == 0) return;
            var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, highScore);
            Debug.Log($"HighScore {highScore} submitted to leaderboard {LeaderboardId}. Entry: {scoreResponse.Score}");
        }
        catch (LeaderboardsException ex)
        {
            Debug.LogError($"Failed to submit score to leaderboard: {ex.Message}");
        }
    }

    public void SetPlayerId(string id)
    {
        playerId = id ?? "Guest"; // Cập nhật PlayerId khi đăng nhập
    }

    void Start()
    {
        playerId = (AuthenticationService.Instance.IsSignedIn && PlayerPrefs.GetInt(anonymous) == 0) ? AuthenticationService.Instance.PlayerId : "Guest";
        timeElapsed = 0;
        currentScore = 0;
        highScore = PlayerPrefs.HasKey($"HighScore_{playerId}") ? PlayerPrefs.GetInt($"HighScore_{playerId}") : 0;
        DisplayScore();
    }

    void Update()
    {
        if (player != null)
        {
            IncreaseScore();
            UpdateHighScore();
            DisplayScore();
        }
    }

    public void IncreaseScore()
    {
        timeElapsed += Time.deltaTime;
        currentScore = (int)timeElapsed;
    }

    public async void UpdateHighScore()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
            SaveHighScore();
            if(AuthenticationService.Instance.IsSignedIn
            && PlayerPrefs.GetInt(anonymous) == 0) await UpdateLeaderboardScore(highScore);
             
        }
    }

    public void DisplayScore()
    {
        if (scoreTxt != null) scoreTxt.text = "Score: " + currentScore.ToString();
        if (highScoreTxt != null) highScoreTxt.text = "High Score: " + highScore.ToString();
    }

    public void SaveHighScore()
    {
        PlayerPrefs.SetInt($"HighScore_{playerId}", highScore);
    }

    public void ResetScore()
    {
        currentScore = 0;
        timeElapsed = 0;
        DisplayScore();
    }

    public void SetHighScore(int newHighScore)
    {
        PlayerPrefs.SetInt($"HighScore_{playerId}", newHighScore);
        highScore = newHighScore;
        Debug.Log($"new HighScore_{playerId}: {newHighScore}");
        DisplayScore();
        
    }
}