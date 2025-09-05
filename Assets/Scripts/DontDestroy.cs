using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    private static readonly Dictionary<string, DontDestroy> instances = new Dictionary<string, DontDestroy>();

    private void Awake()
    {
        string objectKey = gameObject.name + "_" + GetType().Name;
        if (instances.ContainsKey(objectKey))
        {
            //đã có instance này -> huỷ cái mới
            Destroy(gameObject);
            return;
        }
        // Lưu instance và áp dụng DontDestroyOnLoad
        instances[objectKey] = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded; // Đăng ký sự kiện trong Awake
    }

    private void OnDestroy()
    {
        // Hủy đăng ký sự kiện và xóa instance khỏi danh sách
        string objectKey = gameObject.name + "_" + GetType().Name;
        if (instances.ContainsKey(objectKey) && instances[objectKey] == this)
        {
            instances.Remove(objectKey);
        }
        SceneManager.sceneLoaded -= OnSceneLoaded; // Hủy đăng ký sự kiện để tránh rò rỉ bộ nhớ
    }
    public virtual void ResetState()
    {
        // Mặc định không làm gì, các lớp con có thể ghi đè
        // Ví dụ: Player có thể reset vị trí, ItemSpawner có thể reset timer
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            Destroy(gameObject); // Hủy đối tượng khi vào MainMenu
        }
        else if (scene.name.StartsWith("Level"))
        {
            Debug.Log("scene name:" + scene.name);
            ResetState();
            Debug.Log("RESET");
        }
    }
}