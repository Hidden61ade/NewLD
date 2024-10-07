using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse1Trigger : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MouseController1.Instance.curState = MouseController1.MouseState.Chasing;
        }
    }
}
