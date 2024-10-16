using System.Collections;
using System.Collections.Generic;
using GameAudio;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class AudioTrigger : MonoBehaviour
{
    [HideInInspector] public AudioManager.AudioSourceHandler audioSourceHandler;
    public string songname;
    public bool isPlaying;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlaying) return;
        if (!other.CompareTag("Player")) return;
        AudioManager.Instance.PlayMusic(songname, out audioSourceHandler);
        isPlaying = true;
    }

}
