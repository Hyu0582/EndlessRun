using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(5.5f, 2f, -10f);
    [SerializeField] private float smooth = 10f;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;
    [SerializeField]private Camera mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake() {
        mainCamera = GetComponent<Camera>();
    }
    void Start()
    {
        
    }

    private void LateUpdate()
    {
        Vector3 desiredPos = target.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smooth * Time.deltaTime);

        // Giới hạn tọa độ x và y trong phạm vi bản đồ
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;
        smoothedPos.x = Mathf.Clamp(smoothedPos.x, minBounds.x + camWidth / 2, maxBounds.x - camWidth / 2);
        smoothedPos.y = Mathf.Clamp(smoothedPos.y, minBounds.y + camHeight / 2, maxBounds.y - camHeight / 2);

        transform.position = smoothedPos;
    }
}
