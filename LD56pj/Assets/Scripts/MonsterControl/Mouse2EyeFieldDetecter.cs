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
        else if(other.gameObject.GetComponent<BoxPushed>()!=null)
        {
            Debug.Log("cheese!");
            AudioKit.PlaySound("RatSound");
            StartCoroutine(RunToEat(other.transform.position));
        }
    }

    IEnumerator RunToEat(Vector3 position)
    {
        yield return new WaitForSeconds(1f);
        MouseController2.Instance.SwitchToEat(position);
        yield break;
    }
}
