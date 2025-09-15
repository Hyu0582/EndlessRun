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
    private float minY = -4f;
    private float maxY = 4f;
    private float spawnX = 20f; //vị trí tạo x
    private SpawnMode[] spawnModes;
    public SpawnModeConfig[] spawnConfigs; //Mảng cấu hình cho các chế độ
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
                mode = SpawnMode.Double,
                spawnCount = 2,
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
            },
            new() {
                mode = SpawnMode.WaveExtra,
                spawnCount = 6,
                minY = minY,
                maxY = maxY,
                spawnX = spawnX + 15,
            },
        };
        spawnModes = new SpawnMode[]
        {
            SpawnMode.Normal,
            SpawnMode.Normal,
            SpawnMode.Wave,
            SpawnMode.Double,
            SpawnMode.Normal,
            SpawnMode.Normal,
            SpawnMode.Normal,
            SpawnMode.Double,
            SpawnMode.Normal,
            SpawnMode.Double,
            SpawnMode.Wave,
            SpawnMode.WaveExtra,
            SpawnMode.Double,
            SpawnMode.Normal,
            SpawnMode.Wave,
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
            //
            currentMode = spawnModes[Random.Range(0, spawnModes.Length)];
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
        else if (config.mode == SpawnMode.Wave || config.mode == SpawnMode.WaveExtra)
        {
            float spacing = (config.maxY - config.minY) / (config.spawnCount * 2f);
            // Spawn flying theo hình /
            //if (config.mode == SpawnMode.WaveExtra) spacing = (config.maxY - config.minY) / (config.spawnCount * 0.5f); // Khoảng cách Y giữa các flying
            float xOffset = 5f; // Khoảng cách X để tạo góc nghiêng
            int rand = Random.Range(0, 2) == 0 ? -1 : 1;
            for (int i = 0; i < config.spawnCount; i++)
            {
                // Tính vị trí Y: từ maxY xuống minY
                float yPos = randomY + i * spacing * rand * 2f;
                // Tính vị trí X: dịch dần sang trái để tạo hình /
                float xPos = config.spawnX - i * xOffset;

                Vector2 spawnPosition = new(xPos, yPos);
                Instantiate(flyingObstacle, spawnPosition, Quaternion.identity);
            }
        }
        else if (config.mode == SpawnMode.Double)
        {
            Vector2 spawnPosition1 = new(config.spawnX, randomY);
            Vector2 spawnPosition2 = new(config.spawnX + 2f, randomY + 2f);
            Instantiate(flyingObstacle, spawnPosition1, Quaternion.identity);
            Instantiate(flyingObstacle, spawnPosition2, Quaternion.identity);
        }
        
    }
}
