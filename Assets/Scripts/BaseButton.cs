using UnityEngine;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour
{
    [SerializeField] protected Button button; // Kéo Button component vào đây
    [SerializeField] protected GameManager gameManager;
    [SerializeField] protected AudioManager audioManager;

    [System.Obsolete]
    protected virtual void Awake()
    {
        button = GetComponent<Button>();
        gameManager = FindObjectOfType<GameManager>(); // Hoặc assign trong Inspector
        audioManager = FindObjectOfType<AudioManager>(); // Hoặc assign trong Inspector
        button.onClick.AddListener(OnButtonClick); // Gọi hàm khi nhấn
    }

    protected virtual void OnButtonClick()
    {
        if(audioManager != null) audioManager.PlaySfxSelect(); // Phát âm thanh khi nhấn
    }
}