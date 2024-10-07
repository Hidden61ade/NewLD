using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropGroundColliderDetecter : MonoBehaviour
{
    public GameObject fatherDrop;
    void Start()
    {
        if (fatherDrop==null)
        {
            Debug.Log("父对象的drop不存在！");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            // Debug.Log("destroyDrop");
            Destroy(fatherDrop.gameObject);
        }
    }
}
