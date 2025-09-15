using UnityEngine;
using UnityEngine.UI;

public class TouchInputManager : MonoBehaviour
{
    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;
    private PlayerController playerController;

    [System.Obsolete]
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        if(upButton != null) upButton.onClick.AddListener(() => playerController.HandleButtonUp());
        if(downButton != null) downButton.onClick.AddListener(() => playerController.HandleButtonDown());
    }
}