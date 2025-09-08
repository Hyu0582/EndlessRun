using UnityEngine;

public class HomeIcon : BaseButton
{
    void OnMouseDown()
    {
        Time.timeScale = 1;
        gameManager.LoadMainMenu();
    }
}
