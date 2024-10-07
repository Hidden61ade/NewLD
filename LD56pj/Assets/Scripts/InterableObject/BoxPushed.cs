using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPushed : StatedParameter<BoxPushed>
{
    private Rigidbody2D rb;
    public bool isBePushed = false;

    [StatedPara]private Vector3 mPosition
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
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;

        LookPara(this);//TODO: 接收回档信息
        //TypeEventSystem.Global.Register<OnStateChangeEvent>(e =>
        //{
        //    ResetPara(this);
        //});
    }

    // Update is called once per frame
    void Update()
    {
        //TypeEventSystem.Global.Register<OnLevelResetEvent>(e => RestartBox()).UnRegisterWhenGameObjectDestroyed(gameObject);
        BePushed();
    }


    public void BePushed()
    {
        if(isBePushed)
        {
            rb.constraints = RigidbodyConstraints2D.None;  // 解冻所有约束
            rb.freezeRotation = true;
        }
        else
        {
            rb.freezeRotation = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }
}
