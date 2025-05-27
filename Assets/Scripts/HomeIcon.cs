using UnityEngine;

public class HomeIcon : BaseButton
{
    void OnMouseDown()
    {
        gameManager.LoadMainMenu();
    }
}
