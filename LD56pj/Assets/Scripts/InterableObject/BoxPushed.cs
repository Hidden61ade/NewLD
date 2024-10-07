using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPushed : MonoBehaviour
{
    private Vector3 startPos;
    private Rigidbody2D rb;
    public bool isBePushed = false;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    // Update is called once per frame
    void Update()
    {
        TypeEventSystem.Global.Register<OnLevelResetEvent>(e => RestartBox()).UnRegisterWhenGameObjectDestroyed(gameObject);
        BePushed();
    }

    private void RestartBox()
    {
        transform.position = startPos;
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
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }
}
