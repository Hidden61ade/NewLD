using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAudio;

public class Ch2DACtrl2 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag("Player")) return;
        Debug.Log("Ch2DACtrl2");
        DynamicAudioManager.Instance.SetTrackVolume("TheRat3", 1, 1f, fadeDuration: 1.0f);
    }
}
