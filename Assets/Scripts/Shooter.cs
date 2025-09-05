using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private float deadZOne = 15f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    void FixedUpdate()
    {
        if (transform.position.x > deadZOne)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
