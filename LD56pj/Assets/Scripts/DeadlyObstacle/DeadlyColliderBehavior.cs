using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class DeadlyColliderBehavior : MonoBehaviour
{
    
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TypeEventSystem.Global.Send<OnPlayerDiedEvents>();
        }
    }
}
