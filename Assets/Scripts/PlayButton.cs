using UnityEngine;

public class PlayButton : BaseButton
{
    //đã có 2 cái hàm thay đổi hình ảnh khi rê chuột
    void OnMouseDown()
    {
        gameManager.StartGame();
    }
}
