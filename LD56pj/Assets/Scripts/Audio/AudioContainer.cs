using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameAudio
{
    public class AudioContainer : MonoBehaviour
    {
        public AudioClip[] audioClips;
        private void Start() {
            AudioManager.Instance.audioContainer = this;
            AudioManager.Instance.IsContainerReady = true;
        }
    }
}
