using UnityEngine;

public class RestartButton : BaseButton
{
    void OnMouseDown()
    {
        gameManager.StartGame();
    }
}
