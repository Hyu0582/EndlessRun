using UnityEngine;

public class ExitButton : BaseButton
{
    void OnMouseDown()
    {
        gameManager.ExitGame();
    }
}
