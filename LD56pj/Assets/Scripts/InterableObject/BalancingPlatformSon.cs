using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancingPlatformSon : StatedParameter<BalancingPlatformSon>
{
    public bool isOn = false;
    private Rigidbody2D rb;

    [StatedPara]
    private Vector3 mPosition
    {
        get { return transform.position; }
        set
        {
            transform.position = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        LookPara(this);//TODO: 接收回档信息
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box"))
        {
            //Debug.Log("装了");
            if (collision.gameObject.transform.position.y - 0.5f > transform.position.y)
            {
                isOn = true;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box"))
        {
            isOn = false;
        }
    }

}
