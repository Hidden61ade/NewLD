using System.Collections;
using System.Collections.Generic;
using GameAudio;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class AudioStopper : MonoBehaviour
{
    public AudioTrigger audioTrigger;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        if (audioTrigger == null || audioTrigger.audioSourceHandler == null) return;
        audioTrigger.audioSourceHandler.StopThis();
        audioTrigger.isPlaying = false;
    }
}
