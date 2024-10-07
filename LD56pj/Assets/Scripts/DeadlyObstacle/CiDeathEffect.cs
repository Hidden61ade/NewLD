using System.Runtime.InteropServices;
using QFramework;
using UnityEngine;

public class CiDeathEffect : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 检查碰撞的是否为玩家
        if (collision.collider.CompareTag("Player"))
        {
            // 更新当前存档点的位置
            TypeEventSystem.Global.Send<OnPlayerDiedEvents>();
            Debug.Log("You died!");
        }
    }
}