using UnityEngine;
using GameAudio;
using System.Collections;

public class GameController : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Enumerator());
    }

    void Update()
    {
        // 示例：按下空格键时淡出第一个音轨，按下回车键时淡入
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 淡出第一个音轨（索引为0）
            DynamicAudioManager.Instance.SetTrackVolume("TheRat3", 1, 0f, fadeDuration: 1.0f);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            // 淡入第一个音轨（索引为0）
            DynamicAudioManager.Instance.SetTrackVolume("TheRat3", 1, 1.0f, fadeDuration: 1.0f);
        }
    }

    void OnDestroy()
    {
        // 游戏结束时，淡出并停止 "EpicTheme" 的所有音轨
        DynamicAudioManager.Instance.StopDynamicMusic("TheRat3", fadeDuration: 2.0f);
    }
    IEnumerator Enumerator(){
        yield return new WaitForSeconds(5);
        DynamicAudioManager.Instance.PlayDynamicMusic("TheRat3", fadeDuration: 2.0f);
    }
}