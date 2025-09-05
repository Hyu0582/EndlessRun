using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] prefabObstacles;

    [SerializeField] private float[] spawnRates = {0.3f, 1.25f, 1.75f};
    private float spawnRate = 1.75f;

    private float timer = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnObstacle();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            SpawnObstacle();
            timer = 0;
        }
    }

    void SpawnObstacle()
    {
        spawnRate = Random.Range(0, 10) == 0 ? spawnRates[0] : spawnRates[Random.Range(1, spawnRates.Length)];
        GameObject obstacle = prefabObstacles[Random.Range(0, prefabObstacles.Length)];
        Vector3 obsLocalScale = obstacle.transform.localScale;
        obstacle.transform.localScale = new Vector3(obsLocalScale.x, Random.Range(0, 4) == 0 ? -1 * obsLocalScale.x : obsLocalScale.x, obsLocalScale.z);
        Instantiate(obstacle, transform.position, transform.rotation);
    }
}
