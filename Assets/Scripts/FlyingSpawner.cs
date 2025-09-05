using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class FlyingSpawner : MonoBehaviour
{
    public GameObject[] prefabObstacles; //Mảng các prefab
    [SerializeField] private float spawnRate;
    private float timer = 0;
    //khoảng vị trí y
    private float minY = -5f;
    private float maxY = 5f;
    private float spawnX = 20f; //vị trí tạo x

    public SpawnModeConfig[] spawnConfigs; // Mảng cấu hình cho các chế độ
    public SpawnMode currentMode = SpawnMode.Normal; // Chế độ hiện tại
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnRate = Random.Range(10f, 16f);
        spawnConfigs = new SpawnModeConfig[]
        {
            new() {
                mode = SpawnMode.Normal,
                spawnCount = 1,
                minY = minY,
                maxY = maxY,
                spawnX = spawnX,
            },
            new() {
                mode = SpawnMode.Wave,
                spawnCount = 4,
                minY = minY,
                maxY = maxY,
                spawnX = spawnX,
            }
        };
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
            //tỉ lệ 20%
            currentMode = Random.Range(0, 5) == 0 ? SpawnMode.Wave : SpawnMode.Normal;
            SpawnModeConfig config = GetCurrentConfig();
            SpawnFlying(config);
            spawnRate = Random.Range(3f, 5f);
            timer = 0;
        }
    }
    // Lấy cấu hình cho chế độ hiện tại
    SpawnModeConfig GetCurrentConfig()
    {
        foreach (var config in spawnConfigs)
        {
            if (config.mode == currentMode)
                return config;
        }
        return spawnConfigs[0];
    }

    void SpawnFlying(SpawnModeConfig config)
    {
        
        GameObject flyingObstacle = prefabObstacles[Random.Range(0, prefabObstacles.Length)];
        float randomY = Random.Range(config.minY, config.maxY);
        if (config.mode == SpawnMode.Normal)
        {
            // Spawn bình thường: 1 flying ngẫu nhiên
            Vector2 spawnPosition = new(config.spawnX, randomY);
            Instantiate(flyingObstacle, spawnPosition, Quaternion.identity);
        }
        else if (config.mode == SpawnMode.Wave)
        {
            // Spawn 3 flying theo hình /
            float spacing = (config.maxY - config.minY) / (config.spawnCount * 3f); // Khoảng cách Y giữa các flying
            float xOffset = 1f; // Khoảng cách X để tạo góc nghiêng

            for (int i = 0; i < config.spawnCount; i++)
            {
                // Tính vị trí Y: từ maxY xuống minY
                float yPos = randomY - i * spacing;
                // Tính vị trí X: dịch dần sang trái để tạo hình /
                float xPos = config.spawnX - i * xOffset;

                Vector2 spawnPosition = new(xPos, yPos);
                Instantiate(flyingObstacle, spawnPosition, Quaternion.identity);
            }
        }
    }
}
