using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAudio;

public class Ch2DACtrl3 : MonoBehaviour{
        private void OnTriggerEnter2D(Collider2D other) {
            if(!other.CompareTag("Player")) return;
        Debug.Log("Ch2DACtrl3");
        DynamicAudioManager.Instance.StopDynamicMusic("TheRat3",2.0f);
    }
}
