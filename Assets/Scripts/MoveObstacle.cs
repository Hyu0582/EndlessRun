using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    [SerializeField] private float deadZOne = -15f;
    private GameManager gameManager;
    private float deltaSpeed = 4f;
    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = gameManager.GetCurrentSpeed();
        if (gameObject.GetComponent<SpriteRenderer>().sortingLayerName == "Flying")
        {
            speed += deltaSpeed;
        }
        transform.Translate(speed * Time.deltaTime * Vector2.left);
    }
    void FixedUpdate()
    {
        if (transform.position.x < deadZOne)
        {
            Destroy(gameObject);
        }
    }
}
