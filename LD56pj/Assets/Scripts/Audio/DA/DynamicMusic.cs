using System.Collections.Generic;
using UnityEngine;

namespace GameAudio
{
    public class DynamicMusic
    {
        public string Name { get; private set; }
        private List<AudioManager.AudioSourceHandler> tracks = new List<AudioManager.AudioSourceHandler>();

        public DynamicMusic(string name, List<AudioClip> audioClips)
        {
            Name = name;
            foreach (var clip in audioClips)
            {
                AudioManager.AudioSourceHandler handler = null;
                AudioManager.Instance.PlayMusic(clip.name, out handler, volumeScale: 0f);
                handler.SetVolume(0f, 0f); // 确保初始音量为0
                tracks.Add(handler);
            }
        }

        /// <summary>
        /// 播放所有音轨，带有淡入效果
        /// </summary>
        /// <param name="fadeDuration">淡入时长</param>
        public void Play(float fadeDuration)
        {
            foreach (var track in tracks)
            {
                track.SetVolume(AudioManager.Instance.defaultVol, fadeDuration);
                track.SetLoop(true);
            }
        }

        /// <summary>
        /// 停止所有音轨，带有淡出效果
        /// </summary>
        /// <param name="fadeDuration">淡出时长</param>
        public void Stop(float fadeDuration)
        {
            foreach (var track in tracks)
            {
                track.SetVolume(0f, fadeDuration);
                // 根据 AudioSourceHandler 的实现，音量淡出至0后会释放通道
            }
        }

        /// <summary>
        /// 设置指定音轨的音量
        /// </summary>
        /// <param name="trackIndex">音轨索引</param>
        /// <param name="targetVolume">目标音量</param>
        /// <param name="fadeDuration">渐变时长</param>
        public void SetTrackVolume(int trackIndex, float targetVolume, float fadeDuration)
        {
            if (trackIndex >= 0 && trackIndex < tracks.Count)
            {
                tracks[trackIndex].SetVolume(targetVolume, fadeDuration);
            }
            else
            {
                Debug.LogWarning($"Track index {trackIndex} is out of range for DynamicMusic {Name}.");
            }
        }
    }
}