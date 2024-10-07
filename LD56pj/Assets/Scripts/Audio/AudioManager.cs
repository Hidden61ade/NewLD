using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System;
using UnityEditor;
namespace GameAudio
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        private Stack<AudioSource> mUsableAudioSources = new();
        private List<AudioSource> mUsedAS = new();
        private Dictionary<string, AudioClip> mMusics = new();

        private ResLoader resLoader = ResLoader.Allocate();

        public AudioContainer audioContainer;
        public bool IsContainerReady = false;

        public float defaultVol = 1f;

        private readonly char[] randTab = new char[] { '0', '2', '1', '1', '0', '1', '3', '2', '1', '1', '3', '0', '3', '0', '2' };
        private int pRandTab = 0;
        private void Start()
        {
            mUsableAudioSources.Push(gameObject.AddComponent<AudioSource>());
            TypeEventSystem.Global.Register<OnSceneLoadingStartEvent>(e =>
            {
                IsContainerReady = false;
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            TypeEventSystem.Global.Register<OnSceneLoadedEvent>(e =>
            {
                StartCoroutine(GetContainer());
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            ResKit.Init();
        }
        public void PlayFootsteps()
        {
            AudioKit.PlaySound("ftstp-1-" + randTab[pRandTab++], volumeScale: 0.6f);
            if (pRandTab == 15) pRandTab = 0;
        }
        public void PlayMusic(string name)
        {
            if (!mMusics.ContainsKey(name))
            {
                return;
            }
            var temp = AllocateChannel();
            temp.clip = mMusics[name];
            temp.volume = 0f;
            temp.Play();
            FadeIn(temp);
        }
        public void PlayMusic(string name, out AudioSourceHandler audioSourceHandler)
        {
            if (!mMusics.ContainsKey(name))
            {
                throw new KeyNotFoundException($"Music with name {name} not found.");
            }
            var temp = AllocateChannel();
            temp.clip = mMusics[name];
            temp.volume = 0f;
            temp.Play();
            audioSourceHandler = new(temp);
            FadeIn(temp);
        }
        public void StopMusic(AudioSource audioSource)
        {
            FadeOutAndRelease(audioSource);
        }

        public void PlayMusicWithoutTransition(string name)
        {
            if (!mMusics.ContainsKey(name))
            {
                return;
            }
            var t = AllocateChannel();
            t.clip = mMusics[name];
            t.Play();
        }
        public void PlayMusicWithoutTransition(string name, out AudioSourceHandler audioSourceHandler)
        {
            if (!mMusics.ContainsKey(name))
            {
                throw new KeyNotFoundException($"Music with name {name} not found.");
            }
            var t = AllocateChannel();
            t.clip = mMusics[name];
            audioSourceHandler = new(t);
            t.Play();
        }
        public void StopMusicWithoutTransition(AudioSource audioSource)
        {
            audioSource.Stop();
            ReleaseChannel(audioSource);
        }
        private void FadeIn(AudioSource audioSource, float target = 1)
        {
            StartCoroutine(FadeTo(audioSource, target));
        }
        private void FadeOutAndRelease(AudioSource audioSource, float target = 0)
        {
            StartCoroutine(FadeTo(audioSource, target, action: () => ReleaseChannel(audioSource)));
        }
        private void FadeOut(AudioSource audioSource, float target = 0)
        {
            StartCoroutine(FadeTo(audioSource, target));
        }
        IEnumerator GetContainer()
        {
            foreach (var i in mMusics)
            {
                i.Value.UnloadAudioData();
            }
            mMusics.Clear();
            yield return new WaitUntil(() => IsContainerReady);

            foreach (var i in Instance.audioContainer.audioClips)
            {
                i.LoadAudioData();
                mMusics.Add(i.name, i);
            }
            Debug.Log("AudiosReady");
        }
#nullable enable
        IEnumerator FadeTo(AudioSource audioSource, float target, Action? action = null, float duration = 1.0f)
        {
            float startVolume = audioSource.volume;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                yield return null;
                elapsedTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, target, elapsedTime / duration);
            }

            audioSource.volume = target;
            action?.Invoke();
        }
#nullable disable
        AudioSource AllocateChannel(bool isLoop = true)
        {
            AudioSource temp;
            if (mUsableAudioSources.Count == 0)
            {
                temp = gameObject.AddComponent<AudioSource>();
                temp.volume = defaultVol;
            }
            else
            {
                temp = mUsableAudioSources.Pop();
                temp.loop = isLoop;
                temp.enabled = true;
            }
            mUsedAS.Add(temp); // 确保添加到 mUsedAS 列表中
            return temp;
        }
        void ReleaseChannel(AudioSource audioSource)
        {
            audioSource.Stop();
            mUsedAS.Remove(audioSource);
            audioSource.enabled = false;
            audioSource.volume = defaultVol;
            mUsableAudioSources.Push(audioSource);
        }
        public class AudioSourceHandler
        {
            private AudioSource audioSource;
            public AudioSourceHandler(AudioSource audioSource)
            {
                this.audioSource = audioSource;
            }
            public void StopThis()
            {
                AudioManager.Instance.StopMusic(audioSource);
            }
            public void StopThisWithoutTransition()
            {
                AudioManager.Instance.StopMusicWithoutTransition(audioSource);
            }
        }
    }
}
