using UnityEngine;

public class PlayButton : BaseButton
{
    void OnMouseDown()
    {
        gameManager.StartGame();
    }
}
