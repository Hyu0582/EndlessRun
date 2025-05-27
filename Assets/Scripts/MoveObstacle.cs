using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 10f;
    [SerializeField] private float deadZOne = -15f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * baseSpeed * Time.deltaTime;
    }
    void FixedUpdate()
    {
        if (transform.position.x < deadZOne)
        {
            Destroy(gameObject);
        }
    }
}
