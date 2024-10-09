using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.VFX;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private bool scenelyTriggerable = false;
    public bool canTrigger = true;
    private void Start()
    {
        TypeEventSystem.Global.Register<OnSceneLoadedEvent>(e =>
        {
            scenelyTriggerable = true;
            
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
        TypeEventSystem.Global.Register<OnSceneLoadingStartEvent>(e =>
        {
            scenelyTriggerable = false;
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }
    public void OnTriggerStay2D(Collider2D other)
    {
        if (scenelyTriggerable && canTrigger && other.CompareTag("Player"))
        {
            canTrigger = false;
            DialogueManager.Instance.PutDialogue(dialogue);
        }
    }
}
