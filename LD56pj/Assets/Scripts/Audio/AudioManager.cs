using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
namespace GameAudio
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        private readonly char[] randTab = new char[] { '0', '2', '1', '1', '0', '1', '3', '2', '1', '1', '3', '0', '3', '0', '2' };
        private int pRandTab = 0;
        public void PlayFootsteps()
        {
            AudioKit.PlaySound("ftstp-1-" + randTab[pRandTab++], volumeScale: 0.6f);
            if (pRandTab == 15) pRandTab = 0;
        }
    }
}
