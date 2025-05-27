using UnityEngine;

public class ShurikenRotator : MonoBehaviour
{
    public float rotationSpeed = 360f; // Tốc độ xoay (độ/giây)

    void Update()
    {
        // Xoay shuriken quanh trục Z
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}