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
        public Dictionary<string, AudioClip> mMusics { get; private set; } = new();

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
        }
        public void PlayFootsteps()
        {
            AudioKit.PlaySound("ftstp-1-" + randTab[pRandTab++], volumeScale: 0.4f);
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
        public void PlayMusic(string name, out AudioSourceHandler audioSourceHandler, float volumeScale = 1.0f)
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
            FadeIn(temp, defaultVol * volumeScale);
        }
        public void StopMusic(AudioSource audioSource)
        {
            FadeOutAndRelease(audioSource);
        }

        public void PlayMusicWithoutTransition(string name, float volumeScale = 1.0f)
        {
            if (!mMusics.ContainsKey(name))
            {
                return;
            }
            var t = AllocateChannel();
            t.volume = defaultVol * volumeScale;
            t.clip = mMusics[name];
            t.Play();
        }
        public void PlayMusicWithoutTransition(string name, out AudioSourceHandler audioSourceHandler, float volumeScale = 1.0f)
        {
            if (!mMusics.ContainsKey(name))
            {
                throw new KeyNotFoundException($"Music with name {name} not found.");
            }
            var t = AllocateChannel();
            t.clip = mMusics[name];
            t.volume = defaultVol * volumeScale;
            audioSourceHandler = new(t);
            t.Play();
        }
        public void StopMusicWithoutTransition(AudioSource audioSource)
        {
            audioSource.Stop();
            ReleaseChannel(audioSource);
        }
        public void StopAll()
        {
            foreach (var i in mUsedAS)
            {
                StopMusic(i);
            }
        }
        private void FadeIn(AudioSource audioSource, float target = 1)
        {
            StartCoroutine(FadeTo(audioSource, target));
        }
        private void FadeOutAndRelease(AudioSource audioSource, float target = 0)
        {
            StartCoroutine(FadeTo(audioSource, target, action: () => ReleaseChannel(audioSource)));
        }
        public void FadeOut(AudioSource audioSource, float target = 0)
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
            TypeEventSystem.Global.Send<OnAudioLoadedEvent>();
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
            private Coroutine currentFadeCoroutine;
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
            /// <summary>
            /// 设置音源的音量，带有渐变效果
            /// </summary>
            /// <param name="targetVolume">目标音量</param>
            /// <param name="duration">渐变时长</param>
            public void SetVolume(float targetVolume, float duration)
            {
                if (currentFadeCoroutine != null)
                {
                    AudioManager.Instance.StopCoroutine(currentFadeCoroutine);
                }
                currentFadeCoroutine = AudioManager.Instance.StartCoroutine(FadeTo(targetVolume, duration));
            }

            /// <summary>
            /// 渐变音量的协程
            /// </summary>
            private IEnumerator FadeTo(float targetVolume, float duration)
            {
                float startVolume = audioSource.volume;
                float elapsedTime = 0f;

                while (elapsedTime < duration)
                {
                    yield return null;
                    elapsedTime += Time.deltaTime;
                    audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
                }

                audioSource.volume = targetVolume;
            }
            public void SetLoop(bool var){
                audioSource.loop = var;
            }
        }
    }
}
public class OnAudioLoadedEvent{

}
