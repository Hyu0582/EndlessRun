using UnityEngine;

public class PlayButton : BaseButton
{
    protected override void OnButtonClick()
    {
        base.OnButtonClick(); // Phát âm thanh từ BaseButton
        gameManager.StartGame();
    }
}