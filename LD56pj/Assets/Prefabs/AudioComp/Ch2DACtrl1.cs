using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAudio;

public class Ch2DACtrl1 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag("Player")) return;
        Debug.Log("Ch2DACtrl1");
        DynamicAudioManager.Instance.SetTrackVolume("TheRat3", 0, 1f, fadeDuration: 2.0f);
    }
}
