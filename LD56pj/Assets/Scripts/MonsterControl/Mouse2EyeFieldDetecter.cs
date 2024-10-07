using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
/// <summary>
/// 奶酪的名字一定要是Cheese！！！！
/// </summary>
public class Mouse2EyeFieldDetecter : MonoSingleton<Mouse2EyeFieldDetecter>
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
        Debug.Log("碰撞物不是玩家，也不是奶酪");
        
    }

    IEnumerator RunToEat()
    {
        yield return new WaitForSeconds(2f);
        MouseController2.Instance.SwitchToEat(transform.position);
        yield break;
    }
}
