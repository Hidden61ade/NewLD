using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool canTrigger = true;

    public void OnTriggerStay2D(Collider2D other)
    {
        if (canTrigger&&other.CompareTag("Player"))
        {
            canTrigger = false;
            StartCoroutine(putDialogue());
        }
    }

    public IEnumerator putDialogue()
    {
        yield return new WaitForSeconds(2.5f);
        DialogueManager.Instance.PutDialogue(dialogue);
    }
}
