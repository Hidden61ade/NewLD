using System.Runtime.InteropServices;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 检查进入触发器的是否为玩家
        if (collision.collider.CompareTag("Player"))
        {
            // 更新当前存档点的位置
            GameManager.Instance.SetRespawnPoint(collision.transform.position);
            Debug.Log("Checkpoint reached! Respawn point updated.");
        }
    }
}