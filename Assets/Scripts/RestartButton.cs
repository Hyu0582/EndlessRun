using UnityEngine;

public class RestartButton : BaseButton
{
    [System.Obsolete]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
    protected override void OnButtonClick()
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
    {
        base.OnButtonClick(); // Phát âm thanh
        gameManager.RestartGame();
    }
}