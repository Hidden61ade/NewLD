using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse3Trigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            MouseController3.Instance.curState = MouseController3.MouseState.Chasing;
        }
    }
}