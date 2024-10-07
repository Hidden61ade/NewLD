using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System;

namespace GameAudio
{
    public class DynamicAudioManager : MonoSingleton<DynamicAudioManager>
    {
        // 存储所有的动态音乐，键为音乐名称
        private Dictionary<string, DynamicMusic> dynamicMusics = new Dictionary<string, DynamicMusic>();

        // 用于序列化配置
        [System.Serializable]
        public class DynamicMusicConfig
        {
            public string name; // 音乐名称
            public List<string> trackNames; // 每个音轨对应的AudioClip名称
        }

        public List<DynamicMusicConfig> dynamicMusicConfigs;

        void Start()
        {
            TypeEventSystem.Global.Register<OnAudioLoadedEvent>(e =>
            {
                // 初始化动态音乐
                foreach (var config in dynamicMusicConfigs)
                {
                    List<AudioClip> clips = new List<AudioClip>();
                    foreach (var trackName in config.trackNames)
                    {
                        if (AudioManager.Instance.audioContainer != null)
                        {
                            AudioClip clip;
                            try
                            {
                                clip = AudioManager.Instance.mMusics[trackName];
                            }
                            catch (Exception)
                            {
                                clip = null;
                            }
                            if (clip != null)
                            {
                                clips.Add(clip);
                            }
                            else
                            {
                                Debug.LogWarning($"AudioClip {trackName} not found in AudioContainer.");
                            }
                        }
                    }
                    if (clips.Count > 0)
                    {
                        dynamicMusics.Add(config.name, new DynamicMusic(config.name, clips));
                    }
                }
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        /// <summary>
        /// 播放动态音乐
        /// </summary>
        /// <param name="name">音乐名称</param>
        /// <param name="fadeDuration">淡入时长</param>
        public void PlayDynamicMusic(string name, float fadeDuration = 1.0f)
        {
            if (dynamicMusics.ContainsKey(name))
            {
                dynamicMusics[name].Play(fadeDuration);
            }
            else
            {
                Debug.LogWarning($"DynamicMusic {name} not found.");
            }
        }

        /// <summary>
        /// 停止动态音乐
        /// </summary>
        /// <param name="name">音乐名称</param>
        /// <param name="fadeDuration">淡出时长</param>
        public void StopDynamicMusic(string name, float fadeDuration = 1.0f)
        {
            if (dynamicMusics.ContainsKey(name))
            {
                dynamicMusics[name].Stop(fadeDuration);
            }
            else
            {
                Debug.LogWarning($"DynamicMusic {name} not found.");
            }
        }

        /// <summary>
        /// 调整动态音乐中某个音轨的音量
        /// </summary>
        /// <param name="name">音乐名称</param>
        /// <param name="trackIndex">音轨索引</param>
        /// <param name="targetVolume">目标音量</param>
        /// <param name="fadeDuration">渐变时长</param>
        public void SetTrackVolume(string name, int trackIndex, float targetVolume, float fadeDuration = 1.0f)
        {
            if (dynamicMusics.ContainsKey(name))
            {
                dynamicMusics[name].SetTrackVolume(trackIndex, targetVolume, fadeDuration);
            }
            else
            {
                Debug.LogWarning($"DynamicMusic {name} not found.");
            }
        }
    }
}