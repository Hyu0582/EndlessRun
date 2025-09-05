using UnityEngine;

public class RestartButton : BaseButton
{
    [System.Obsolete]
    void OnMouseDown()
    {
        gameManager.RestartGame();
    }
}
