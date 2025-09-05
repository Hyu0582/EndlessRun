using System;
using UnityEngine;


public class PlayerCollisionController : MonoBehaviour
{
    private GameManager gameManager;
    private AudioManager audioManager;
    private SpriteRenderer playerSprite;
    private SkillManager skillManager;
    private bool isShielded;
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
        skillManager = FindAnyObjectByType<SkillManager>();
        playerSprite = GetComponent<SpriteRenderer>();
        isShielded = false;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (isShielded)
            {
                Destroy(collision.gameObject);
                isShielded = false;
                playerSprite.color = Color.white;
            }
            else
            {
                gameManager.GameOver();
                audioManager.PlaySfxEnd();
            }
        }
        else if (collision.gameObject.CompareTag("Portal"))
        {
            Debug.Log("Chuyển level");
            gameManager.LoadRandomLevel();
        }
        else if (collision.gameObject.CompareTag("Shield"))
        {
            //
            isShielded = true;
            Destroy(collision.gameObject);
            playerSprite.color = Color.yellow;
        }
        else if (collision.gameObject.CompareTag("SapphireStaff"))
        {
            skillManager.SetBulletPrefab("Bolt");
            skillManager.ActivateSkill();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("EmeraldStaff"))
        {
            skillManager.SetBulletPrefab("Spark");
            skillManager.ActivateSkill();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("RubyStaff"))
        {
            skillManager.SetBulletPrefab("Fire");
            skillManager.ActivateSkill();
            Destroy(collision.gameObject);
        }

    }
}
