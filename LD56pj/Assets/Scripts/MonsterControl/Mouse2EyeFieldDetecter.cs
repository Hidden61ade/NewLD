using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
/// <summary>
/// 奶酪的名字一定要是Cheese！！！！
/// </summary>
public class MouseEyeFieldDetecter : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MouseController2.Instance.SwitchToKill();
        }
        else if(other.gameObject.name.Equals("Cheese"))
        {
            StartCoroutine(RunToEat());
        }
    }

    IEnumerator RunToEat()
    {
        yield return new WaitForSeconds(2f);
        MouseController2.Instance.SwitchToEat(transform.position);
        yield break;
    }
}
