using UnityEditor.PackageManager;
using UnityEngine;

public class ParallaxBg : MonoBehaviour
{
    private float baseSpeed = 10f;
    [SerializeField] private Transform[] backgrounds;
    public float parallaxEffect;
    private float length;
    private void Start() {
        length = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
    }
    private void Update() {
        // Tính toán tốc độ di chuyển của layer dựa trên baseSpeed và parallaxFactor
        float moveSpeed = baseSpeed * parallaxEffect * Time.deltaTime;
        // Di chuyển layer
        transform.position += new Vector3(-moveSpeed, 0, 0);
        // Kiểm tra và di chuyển sprite để tạo hiệu ứng vô hạn
        foreach (Transform bg in backgrounds)
        {
            // Nếu sprite di chuyển quá xa về bên trái
            if (bg.position.x < -length)
            {
                // Di chuyển sprite sang bên phải của bản sao khác
                bg.position += new Vector3(length * backgrounds.Length, 0, 0);
            }
        }
    }
}
