using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BalancingPlatforms : MonoBehaviour
{
    public GameObject platform1;  // 平台1
    public GameObject platform2;  // 平台2
    public float platformSpeed = 2f;  // 平台的移动速度
    private Rigidbody2D rb1;  // 平台1的刚体
    private Rigidbody2D rb2;  // 平台2的刚体
    public bool isPlayerOnPlatform1 = false;
    public bool isPlayerOnPlatform2 = false;

    void Start()
    {
        rb1 = platform1.GetComponent<Rigidbody2D>();
        rb2 = platform2.GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        // 根据玩家是否站在平台上调整平台的运动
        if (platform1.GetComponent<BalancingPlatformSon>().isOn)
        {
            Debug.Log("改动");
            MovePlatform(rb1, -1);  // Platform 1 下移
            MovePlatform(rb2, 1);   // Platform 2 上移
        }
        else if (platform2.GetComponent<BalancingPlatformSon>().isOn)
        {
            MovePlatform(rb1, 1);   // Platform 1 上移
            MovePlatform(rb2, -1);  // Platform 2 下移
        }
        else
        {
            StopPlatforms();
        }
    }

    // 控制平台移动的函数
    void MovePlatform(Rigidbody2D rb, int direction)
    {
        rb.velocity = new Vector2(rb.velocity.x, direction * platformSpeed);
    }

    // 停止平台运动
    void StopPlatforms()
    {
        rb1.velocity = new Vector2(rb1.velocity.x, 0);
        rb2.velocity = new Vector2(rb2.velocity.x, 0);
    }

}

