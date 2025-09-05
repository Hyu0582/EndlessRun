using UnityEngine;

public class ItemSpawner : DontDestroy
{
    [SerializeField] private GameObject[] buffItems;
    [SerializeField] private GameObject portal;
    private float timeSpawnBuff = 5f;
    private float timeSpawnPortal = 8f;
    private float timerBuff;
    private float timerPortal;
    [SerializeField] float spawnMaxY;
    [SerializeField] float spawnMinY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        timerBuff = 0f;
        timerPortal = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerPortal >= timeSpawnPortal)
        {
            timerPortal = 0;
            SpawnPortal();
        }
        if (timerBuff >= timeSpawnBuff)
        {
            timerBuff = 0;
            SpawnBuff();
        }
        timerBuff += Time.deltaTime;
        timerPortal += Time.deltaTime;
    }

    private void SpawnPortal()
    {
        // Tạo vị trí ngẫu nhiên trong khu vực spawn
        float randomY = Random.Range(spawnMinY, spawnMaxY);
        Vector2 spawnPosition = new(transform.position.x, randomY);

        // Spawn vật phẩm tại vị trí ngẫu nhiên
        Instantiate(portal, spawnPosition, Quaternion.identity);
    }

    private void SpawnBuff()
    {
        // Chọn ngẫu nhiên một prefab từ mảng
        int randomIndex = Random.Range(0, buffItems.Length);
        GameObject itemToSpawn = buffItems[randomIndex];

        // Tạo vị trí ngẫu nhiên trong khu vực spawn
        float randomY = Random.Range(spawnMinY, spawnMaxY);
        Vector2 spawnPosition = new(transform.position.x, randomY);

        // Spawn vật phẩm tại vị trí ngẫu nhiên
        Instantiate(itemToSpawn, spawnPosition, Quaternion.identity);
    }
    public override void ResetState()
    {
        timerBuff = 0f;
        timerPortal = 0f;
        Debug.Log("Reset state - item spawner");
    }

}