using UnityEngine;

public class DoorToNextScene : MonoBehaviour
{
    public Transform player; // 角色的Transform

    void OnTriggerEnter2D(Collider2D other)
    {
        // 检查碰撞的对象是否是player
        if (other.transform == player)
        {
            // 调用SceneControl中的方法来加载下一个场景
            Debug.Log("Player entered the door.");
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            string nextSceneName = currentSceneName == "chapter 1" ? "chapter 2" : "chapter 3";
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }
}