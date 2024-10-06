using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BalancingPlatforms : MonoBehaviour
{
    public GameObject platform1;  // ƽ̨1
    public GameObject platform2;  // ƽ̨2
    public float platformSpeed = 2f;  // ƽ̨���ƶ��ٶ�
    private Rigidbody2D rb1;  // ƽ̨1�ĸ���
    private Rigidbody2D rb2;  // ƽ̨2�ĸ���
    public bool isPlayerOnPlatform1 = false;
    public bool isPlayerOnPlatform2 = false;

    void Start()
    {
        rb1 = platform1.GetComponent<Rigidbody2D>();
        rb2 = platform2.GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        // ��������Ƿ�վ��ƽ̨�ϵ���ƽ̨���˶�
        if (platform1.GetComponent<BalancingPlatformSon>().isOn)
        {
            Debug.Log("�Ķ�");
            MovePlatform(rb1, -1);  // Platform 1 ����
            MovePlatform(rb2, 1);   // Platform 2 ����
        }
        else if (platform2.GetComponent<BalancingPlatformSon>().isOn)
        {
            MovePlatform(rb1, 1);   // Platform 1 ����
            MovePlatform(rb2, -1);  // Platform 2 ����
        }
        else
        {
            StopPlatforms();
        }
    }

    // ����ƽ̨�ƶ��ĺ���
    void MovePlatform(Rigidbody2D rb, int direction)
    {
        rb.velocity = new Vector2(rb.velocity.x, direction * platformSpeed);
    }

    // ֹͣƽ̨�˶�
    void StopPlatforms()
    {
        rb1.velocity = new Vector2(rb1.velocity.x, 0);
        rb2.velocity = new Vector2(rb2.velocity.x, 0);
    }

}

