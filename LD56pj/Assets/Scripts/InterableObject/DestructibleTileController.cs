using System.Collections;
using QFramework;
using UnityEngine;

public class DestructibleTileController : StatedParameter<DestructibleTileController>
{
    [Header("Destruction Settings")]
    public float destructionDelay = 2f; // 踩踏后消失的延迟时间

    [Header("Visual & Audio Effects")]
    public GameObject destructionEffectPrefab; // 消失时的粒子效果 Prefab
    public AudioClip destructionSound; // 消失时播放的音效
    private AudioSource audioSource;
    [StatedPara]
    bool IsActive
    {
        get { return gameObject.activeInHierarchy; }
        set
        {
            gameObject.SetActive(value);
        }
    }
    private void Start()
    {
        // 获取或添加 AudioSource 组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        LookPara(this);
        TypeEventSystem.Global.Register<OnLevelResetEvent>(e =>
        {
            StopAllCoroutines();
            ResetPara(this);
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            StartCoroutine(DestroyAfterDelay());
        }
    }

    IEnumerator DestroyAfterDelay()
    {
        // 等待指定的延迟时间
        yield return new WaitForSeconds(destructionDelay);

        // 播放消失效果
        if (destructionEffectPrefab != null)
        {
            StartCoroutine(ParticleEffect());
        }

        // 播放音效
        if (destructionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(destructionSound);
        }

        // 移除 Tile
        gameObject.SetActive(false);
    }
    IEnumerator ParticleEffect()
    {
        var a = Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2);
        Destroy(a);
    }
}