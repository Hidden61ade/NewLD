using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    public Dialogue d;
    private void Start()
    {
        DialogueManager.Instance.PutDialogue(d);
    }
}
