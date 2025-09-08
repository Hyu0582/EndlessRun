using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models; 
using System.Threading.Tasks;
using Unity.Services.Core;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerNameManager : MonoBehaviour {
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private Button saveButton; 
    [SerializeField] private Text errorMessageText;
    private const string anonymous = "IsAnonymous";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _ = SetupNameFieldAsync();
        saveButton.onClick.AddListener(OnSaveButtonClicked);
    }

    private async Task SetupNameFieldAsync()
    {
        string playerName = "Guest";
        try
        {
            playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
        }
        catch (RequestFailedException)
        {
            playerName = AuthenticationService.Instance.PlayerId;
        }
        
        if (playerNameInputField != null)
        {
            playerNameInputField.text = playerName;
        }
    }

    private async void OnSaveButtonClicked()
    {
        string newName = playerNameInputField.text.Trim();
        if (string.IsNullOrEmpty(newName))
        {
            ShowError("Username is not allowed to leave blank!");
            return;
        }
        if (newName.Length < 3)
        {
            ShowError("Username is not less than 3 characters!");
            return;
        }
        if (newName.Length > 30)
        {
            ShowError("Username must not exceed 30 characters!");
            return;
        }

        await UpdatePlayerName(newName);
        ShowError("");
        SceneManager.LoadScene("LeaderBoard");
    }

    private async Task UpdatePlayerName(string newName)
    {
        try
        {
            if (AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.UpdatePlayerNameAsync(newName);
            }
            PlayerPrefs.SetString("PlayerName_" + AuthenticationService.Instance.PlayerId, newName);
            playerNameInputField.text = newName; 
        }
        catch
        {
            ShowError("Error when updating username!");
        }
    }

    private void ShowError(string message)
    {
        if (errorMessageText != null)
        {
            errorMessageText.text = message;
        }
        else
        {
            Debug.LogWarning("Không tìm thấy Text để hiển thị thông báo lỗi!");
        }
    }
}
