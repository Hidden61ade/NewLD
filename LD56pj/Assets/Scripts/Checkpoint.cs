using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查进入触发器的是否为玩家
        if (other.CompareTag("Player"))
        {
            // 更新当前存档点的位置
            GameManager.Instance.SetRespawnPoint(transform.position);
            Debug.Log("Checkpoint reached! Respawn point updated.");
            
        }
    }
}