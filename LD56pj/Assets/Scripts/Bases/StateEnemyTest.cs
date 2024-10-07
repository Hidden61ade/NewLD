using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 实现分段保存的数据
/// </summary>
public class StateEnemyTest : StatedParameter<StateEnemyTest>
{
    public int a = 1;
    [StatedPara] public int b = 2;
    [StatedPara] public int c = 3;
    [StatedPara] public bool d = false;
    [StatedPara]
    public Vector3 A
    {
        set
        {
            transform.position = value;
        }
        get
        {
            return transform.position;
        }
    }
    private void Start()
    {
        LookPara(this);
        TypeEventSystem.Global.Register<OnStateChangeEvent>(e =>
        {
            ResetPara(this);
        });
        StartCoroutine(TESTER());
    }

    IEnumerator TESTER()
    {
        yield return new WaitForSeconds(10);
        TypeEventSystem.Global.Send<OnStateChangeEvent>();
        yield return new WaitForSeconds(5);
        LookPara(this);
        Debug.Log("PARAMETER LOOKED");
        yield return new WaitForSeconds(10);
        TypeEventSystem.Global.Send<OnStateChangeEvent>();
    }
}
