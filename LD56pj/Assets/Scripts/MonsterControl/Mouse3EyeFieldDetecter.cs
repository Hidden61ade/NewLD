using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse3EyeFieldDetecter : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MouseController3.Instance.SwitchToKill();
        }
    }
}
