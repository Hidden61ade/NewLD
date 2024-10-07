using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenPlaceControl : MonoBehaviour
{
    [Header("放在cat的子集")]
    public Transform Cat;

    public void Awake()
    {
        Cat = transform.parent;
        if (Cat==null)
        {
            Debug.Log("Cat not found!");
        }
    }
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Cat.GetComponent<CatHiddenController>().TimerReset();
        }
    }
}
