using UnityEngine;
using System;

public enum SpawnMode
{
    Normal,    // Spawn bình thường
    Wave,      // Spawn theo hình sóng
}

[SerializeField] public struct SpawnModeConfig
{
    public SpawnMode mode;           // Chế độ spawn
    public float spawnRate;          // Tần suất spawn (giây/lần)
    public int spawnCount;           // Số lượng shuriken mỗi lần spawn
    public float minY;               // Vị trí Y tối thiểu
    public float maxY;               // Vị trí Y tối đa
    public float spawnX;              // Vị trí X spawn
}
