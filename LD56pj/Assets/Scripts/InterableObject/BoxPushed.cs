using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPushed : MonoBehaviour
{
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        TypeEventSystem.Global.Register<OnLevelResetEvent>(e => RestartBox()).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void RestartBox()
    {
        transform.position = startPos;
    }
}
