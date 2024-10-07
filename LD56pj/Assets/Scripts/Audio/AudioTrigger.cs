using System.Collections;
using System.Collections.Generic;
using GameAudio;
using MoonSharp.VsCodeDebugger.SDK;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class AudioTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        StartCoroutine(Test());
    }
    IEnumerator Test(){
        AudioManager.Instance.PlayMusic("TheAnt", out var a);
        yield return new WaitForSeconds(10);
        a.StopThis();
    }
}
