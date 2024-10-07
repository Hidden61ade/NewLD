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

    public float maxHight = 0.1f;

    private Vector3 platPos1;
    private Vector3 platPos2;
    void Start()
    {
        rb1 = platform1.GetComponent<Rigidbody2D>();
        rb2 = platform2.GetComponent<Rigidbody2D>();
        platPos1 = platform1.transform.position;
        platPos2 = platform2.transform.position;

        rb1.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        rb2.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        rb1.freezeRotation = true;
        rb2.freezeRotation = true;


    }

    void Update()
    {
        //Debug.Log(platform1.transform.position.y - platPos1.y >= -maxHight);
        // 根据玩家是否站在平台上调整平台的运动
        if (platform1.GetComponent<BalancingPlatformSon>().isOn && platform1.transform.position.y - platPos1.y >= -maxHight)
        {
            MovePlatform(rb1, -1);  // Platform 1 下移
            MovePlatform(rb2, 1);   // Platform 2 上移
        }
        else if (platform2.GetComponent<BalancingPlatformSon>().isOn && platform2.transform.position.y - platPos2.y >= -maxHight)
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
        //rb.constraints = RigidbodyConstraints2D.None;
        //rb.constraints = RigidbodyConstraints2D.FreezePositionX;// 解冻所有约束
        //rb.freezeRotation = true;
        //rb.velocity = new Vector2(rb.velocity.x, direction * platformSpeed);+
        rb.transform.position = new Vector2(rb.transform.position.x, rb.transform.position.y + (direction * platformSpeed * Time.deltaTime));


    }

    // 停止平台运动
    void StopPlatforms()
    {
        rb1.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        rb2.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        rb1.freezeRotation = true;
        rb2.freezeRotation = true;
        //rb2.velocity = new Vector2(rb2.velocity.x, 0);
        //Debug.Log(platform1.transform.position.y - platPos1.y);
    }

}

