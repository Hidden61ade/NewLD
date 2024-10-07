using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyCameraOnSceneLoad : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 确保只在加载新场景时摧毁相机
        if (scene.name != SceneManager.GetActiveScene().name)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}