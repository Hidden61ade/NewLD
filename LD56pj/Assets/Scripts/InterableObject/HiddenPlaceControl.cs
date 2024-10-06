using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenPlaceControl : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Timer Reset!");
            CatController.Instance.TimerReset();
        }
    }
}
