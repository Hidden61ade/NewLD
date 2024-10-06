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
    [StatedPara]public int b = 2;
    [StatedPara]public int c = 3;
    [StatedPara]public bool d = false;
    private void Start() {
        LookPara(this);
        TypeEventSystem.Global.Register<OnStateChageEvent>(e=>{
            ResetPara(this);
        });
        StartCoroutine(TESTER());
    }

    IEnumerator TESTER(){
        yield return new WaitForSeconds(10);
        TypeEventSystem.Global.Send<OnStateChageEvent>();
        yield return new WaitForSeconds(5);
        LookPara(this);
        Debug.Log("PARAMETER LOOKED");
        yield return new WaitForSeconds(10);
        TypeEventSystem.Global.Send<OnStateChageEvent>();
    }
}
