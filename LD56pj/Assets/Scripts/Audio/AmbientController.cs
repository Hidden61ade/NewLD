using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class AmbientController : MonoSingleton<AmbientController>
{
    public AudioClip ambientClip;
    private GameAudio.AudioManager.AudioSourceHandler audioSourceHandler;
    void Start()
    {
        ambientClip.LoadAudioData();
        GameAudio.AudioManager.Instance.PlayMusic(ambientClip, out this.audioSourceHandler, 0.5f);
        TypeEventSystem.Global.Register<OnSceneLoadingStartEvent>(e =>
        {
            audioSourceHandler.SetVolume(0, 1);
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
        TypeEventSystem.Global.Register<OnSceneLoadedEvent>(e =>
        {
            audioSourceHandler.SetVolume(0.5f, 1);
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }
}
