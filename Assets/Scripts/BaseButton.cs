using UnityEngine;

public class BaseButton : MonoBehaviour
{
    public Sprite btnNormal;
    public Sprite btnEnter;
    private SpriteRenderer sr;
    public GameManager gameManager;
    private AudioManager audioManager;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        gameManager = FindAnyObjectByType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    void OnMouseEnter()
    {
        if (btnEnter != null)
        {
            sr.sprite = btnEnter;
            audioManager.PlaySfxSelect();
        }
    }
    void OnMouseExit()
    {
        if (btnNormal != null)
        {
            sr.sprite = btnNormal;
        }
    }
}
