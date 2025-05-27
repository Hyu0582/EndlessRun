using UnityEngine;

public class RoadRoll : MonoBehaviour
{
    private float baseSpeed = 10f;
    [SerializeField]private Transform tilemap01;
    [SerializeField]private Transform tilemap02;
    public float length = 12f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Di chuyển cả hai Tilemap sang trái
        tilemap01.Translate(Vector3.left * baseSpeed * Time.deltaTime);
        tilemap02.Translate(Vector3.left * baseSpeed * Time.deltaTime);

        // Nếu Tilemap 1 ra khỏi màn hình, đặt lại vị trí
        if (tilemap01.position.x < -length)
        {
            tilemap01.position += new Vector3(length * 2, 0, 0);
        }
        // Nếu Tilemap 2 ra khỏi màn hình, đặt lại vị trí
        if (tilemap02.position.x < -length)
        {
            tilemap02.position += new Vector3(length * 2, 0, 0);
        }
    }
}
