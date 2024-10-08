using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class HeartbeatController : MonoSingleton<HeartbeatController>
{
    public AudioClip heatbeatClip;
    private GameAudio.AudioManager.AudioSourceHandler audioSourceHandler;
    void Start()
    {
        heatbeatClip.LoadAudioData();
        GameAudio.AudioManager.Instance.PlayMusic(heatbeatClip, out this.audioSourceHandler, 0f);
        TypeEventSystem.Global.Register<OnSceneLoadingStartEvent>(e =>
        {
            audioSourceHandler.SetVolume(0, 1);
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
        // TypeEventSystem.Global.Register<OnSceneLoadedEvent>(e =>
        // {
        //     audioSourceHandler.SetVolume(0.5f, 1);
        // }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }
    public void SetVolume(float target, float duration = 1)
    {
        audioSourceHandler.SetVolume(target, duration);
    }
}
