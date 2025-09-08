using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Unity.Services.Leaderboards.Exceptions;

public class LeaderBoardManager : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollView; // Tham chiếu đến ScrollView
    [SerializeField] private GameObject leaderboardItemPrefab; // Prefab cho mỗi mục trong danh sách
    private const string LeaderboardId = "HighScoreLB";

    // // Gửi HighScore lên leaderboard
    // public async Task UpdateLeaderboardScore(int highScore)
    // {
    //     try
    //     {
    //         var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, highScore);
    //         Debug.Log($"HighScore {highScore} submitted to leaderboard {LeaderboardId}. Entry: {scoreResponse.Score}");
    //     }
    //     catch (LeaderboardsException ex)
    //     {
    //         Debug.LogError($"Failed to submit score to leaderboard: {ex.Message}");
    //     }
    // }

    // Lấy và hiển thị top 20 người chơi trong ScrollView
    public async Task DisplayTopScoresAsync()
    {
        try
        {
            if (scrollView == null)
            {
                Debug.Log("Không co scroll view");
                return;
            }
            // Xóa các mục cũ trong Content
            foreach (Transform child in scrollView.content)
            {
                Destroy(child.gameObject);
            }

            // Lấy top 20 người chơi từ leaderboard
            var leaderboardScores = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId, new GetScoresOptions
            {
                Limit = 20 // Giới hạn 20 người chơi
            });
            //header
            GameObject header = Instantiate(leaderboardItemPrefab, scrollView.content);
                header.transform.SetParent(scrollView.content, false); // false để giữ scale nguyên
                LayoutRebuilder.ForceRebuildLayoutImmediate(header.transform as RectTransform);

            Text headerRankText = header.transform.Find("RankText").GetComponent<Text>();
            Text headerPlayerNameText = header.transform.Find("PlayerNameText").GetComponent<Text>();
            Text headerScoreText = header.transform.Find("ScoreText").GetComponent<Text>();
            headerRankText.text = "Rank";
            headerPlayerNameText.text = "Player Name";
            headerScoreText.text = "Score";
            // Tạo các mục mới từ Prefab
            int rank = 1;
            foreach (LeaderboardEntry entry in leaderboardScores.Results)
            {
                string playerName = string.IsNullOrEmpty(entry.PlayerName) ? entry.PlayerId : entry.PlayerName;
                GameObject item = Instantiate(leaderboardItemPrefab, scrollView.content);

                //Đặt item dưới dạng child và làm mới layout
                item.transform.SetParent(scrollView.content, false); // false để giữ scale nguyên
                LayoutRebuilder.ForceRebuildLayoutImmediate(item.transform as RectTransform);

                Text rankText = item.transform.Find("RankText").GetComponent<Text>();
                Text playerNameText = item.transform.Find("PlayerNameText").GetComponent<Text>();
                Text scoreText = item.transform.Find("ScoreText").GetComponent<Text>();
                if (rankText != null && playerNameText != null && scoreText != null)
                {
                    rankText.text = rank.ToString();
                    playerNameText.text = playerName;
                    scoreText.text = entry.Score.ToString();
                    
                }
                else
                {
                    Debug.LogWarning("Text components not found in leaderboard item prefab!");
                }
                Debug.Log(rank);
                rank++;
            }
        }
        catch (LeaderboardsException ex)
        {
            Debug.LogError($"Failed to retrieve leaderboard scores: {ex.Message}");
            // Hiển thị thông báo lỗi nếu cần (tùy chỉnh UI)
        }
    }

    // Gọi khi cần làm mới leaderboard
    public void RefreshLeaderboard()
    {
        _ = DisplayTopScoresAsync();
    }

    // Tự động làm mới leaderboard khi khởi tạo
    private void Start()
    {
        _ = DisplayTopScoresAsync();
    }
}