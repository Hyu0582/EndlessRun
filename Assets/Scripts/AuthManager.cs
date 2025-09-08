using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using System;
using System.Text;
using UnityEngine.UI;
using Unity.Services.Leaderboards.Models;
using Unity.Services.Leaderboards;

public class AuthManager : MonoBehaviour
{
    [SerializeField] private GameObject signInBtn;
    [SerializeField] private GameObject signOutBtn;
    [SerializeField] private GameObject leaderboardBtn;
    [SerializeField] private Text userNameTxt;
    public bool isAnonymous = true;
    private const string anonymous = "IsAnonymous";
    private const string LeaderboardId = "HighScoreLB";
    private ScoreManager scoreManager;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(anonymous)) PlayerPrefs.SetInt(anonymous, 1);
        signInBtn.SetActive(PlayerPrefs.GetInt(anonymous) == 1);
        signOutBtn.SetActive(PlayerPrefs.GetInt(anonymous) == 0);
        leaderboardBtn.SetActive(PlayerPrefs.GetInt(anonymous) == 0);
        scoreManager = FindAnyObjectByType<ScoreManager>();
    }
    private async Task UpdateInfoAsync()
    {
        string playerName;

        int highScore;
        if (PlayerPrefs.GetInt(anonymous) == 1)
        {
            // Người chơi ẩn danh
            playerName = "Guest";
            highScore = PlayerPrefs.GetInt("HighScore_Guest", 0);
            scoreManager.SetPlayerId("Guest");
            scoreManager.SetHighScore(highScore);
        }
        else
        {
            try
            {
                playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
            }
            catch (RequestFailedException)
            {
                playerName = AuthenticationService.Instance.PlayerId;
            }

            highScore = PlayerPrefs.GetInt("HighScore_" + AuthenticationService.Instance.PlayerId, 0);
            scoreManager.SetPlayerId(AuthenticationService.Instance.PlayerId);
            scoreManager.SetHighScore(highScore);
        }
        
        userNameTxt.text = "username: " + playerName.ToString();
        
        Debug.Log("HighScore: " + highScore);
    }
    // private async Task SignInWithUnityAsync()
    // {
    //     try
    //     {
    //         await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);
    //         PlayerPrefs.SetInt(anonymous, 0);
    //         scoreManager?.SetPlayerId(AuthenticationService.Instance.PlayerId);
    //         UpdateUI();
    //     }
    //     catch (RequestFailedException ex)
    //     {
    //         Debug.LogException(ex);
    //         SetException(ex);
    //     }
    // }
    private async Task SignInWithUnityAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);
            PlayerPrefs.SetInt(anonymous, 0);
            scoreManager.SetPlayerId(AuthenticationService.Instance.PlayerId);
            
            // Lấy HighScore từ leaderboard
            int localHighScore = PlayerPrefs.GetInt($"HighScore_{AuthenticationService.Instance.PlayerId}", 0);
            LeaderboardEntry playerEntry = await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);
            int onlineHighScore = (int)(playerEntry?.Score ?? 0);
            
            // Chọn điểm cao nhất
            int finalHighScore = Mathf.Max(localHighScore, onlineHighScore);
            scoreManager.SetPlayerId(AuthenticationService.Instance.PlayerId);
            scoreManager.SetHighScore(finalHighScore);
            await scoreManager.UpdateLeaderboardScore(finalHighScore);
            
            UpdateUI();
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
            SetException(ex);
        }
    }
    private async void SignInWithUnity()
    {
        await SignInWithUnityAsync();
    }

    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            PlayerAccountService.Instance.SignedIn += SignInWithUnity;

            Debug.Log("authen: " + AuthenticationService.Instance.IsSignedIn);
            Debug.Log("playeraccount: " + PlayerAccountService.Instance.IsSignedIn);

            if (AuthenticationService.Instance.IsSignedIn)
            {
                PlayerPrefs.SetInt(anonymous, 0);
                UpdateUI();
                Debug.Log("Đã đăng nhập tự động vào Unity Authentication.");
            }
            else
            {
                // Attempt to restore session if previously signed in
                if (PlayerPrefs.GetInt(anonymous) == 0)
                {
                    try
                    {
                        // Check if a session token exists and try to sign in anonymously
                        if (AuthenticationService.Instance.SessionTokenExists)
                        {
                            await AuthenticationService.Instance.SignInAnonymouslyAsync();
                            PlayerPrefs.SetInt(anonymous, 0);
                            UpdateUI();
                            Debug.Log("Restored previous session.");
                        }
                        else if (PlayerAccountService.Instance.IsSignedIn)
                        {
                            await SignInWithUnityAsync();
                        }
                        else
                        {
                            Debug.Log("No previous session found. Please sign in again.");
                            PlayerPrefs.SetInt(anonymous, 1);
                            //await UpdateInfoAsync();
                            UpdateUI();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Failed to restore session: {ex.Message}");
                        PlayerPrefs.SetInt(anonymous, 1);
                        //await UpdateInfoAsync();
                        UpdateUI();
                    }
                }
                else
                {
                    Debug.Log("Anonymous");
                    PlayerPrefs.SetInt(anonymous, 1);
                    //await UpdateInfoAsync();
                    UpdateUI();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Lỗi khởi tạo Unity Services: {ex.Message}");
            PlayerPrefs.SetInt(anonymous, 1);
            await UpdateInfoAsync();
        }
        
    }
    async void StartSignInAsync()
    {
        if (PlayerAccountService.Instance.IsSignedIn)
        {
            await SignInWithUnityAsync();
            return;
        }

        try
        {
            await PlayerAccountService.Instance.StartSignInAsync();
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
            SetException(ex);
        }
    }
    public void OnSignInButtonClicked()
    {
        StartSignInAsync();
    }
    public void SignOut()
    {
        AuthenticationService.Instance.SignOut(); // Giữ session token

        PlayerAccountService.Instance.SignOut();
        PlayerPrefs.SetInt(anonymous, 1);
        scoreManager.SetPlayerId("Guest");
        UpdateUI();
    }

    private void OnDestroy()
    {
        if (PlayerAccountService.Instance != null)
        {
            PlayerAccountService.Instance.SignedIn -= SignInWithUnity;
            Debug.Log("PlayerAccountService event handler detached.");
        }
    }
    private void UpdateUI()
    {
        Debug.Log("isSignIn: " + AuthenticationService.Instance.IsSignedIn);
        Debug.Log("anonymous: " + PlayerPrefs.GetInt(anonymous));
        if (AuthenticationService.Instance.IsSignedIn)
        {
            signOutBtn.SetActive(true);
            signInBtn.SetActive(false);
            leaderboardBtn.SetActive(true);
            scoreManager.SetPlayerId(AuthenticationService.Instance.PlayerId);
        }
        else
        {
            signInBtn.SetActive(true);
            signOutBtn.SetActive(false);
            leaderboardBtn.SetActive(false);
            scoreManager.SetPlayerId("Guest");
        }
        _ = UpdateInfoAsync();
        SetException(null);
    }
    private void SetException(RequestFailedException ex)
    {
        if (ex != null) Debug.Log($"Lỗi: {ex.Message}");
    }
}