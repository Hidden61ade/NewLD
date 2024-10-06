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
            UnityEngine.SceneManagement.SceneManager.LoadScene("chapter 2");
        }
    }
}