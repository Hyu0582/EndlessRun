using UnityEngine;

public class HomeButton : BaseButton
{
    protected override void OnButtonClick()
    {
        base.OnButtonClick(); // Phát âm thanh từ BaseButton
        Time.timeScale = 1;
        gameManager.LoadMainMenu();
    }
}