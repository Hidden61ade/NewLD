using System;
using UnityEngine;


[CreateAssetMenu(menuName = "CreatDialogue(一段对话)",fileName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public DialogueNode[] DialogueNodes;
}

[Serializable]
public class DialogueNode
{
    [Header("Name")] public string playerName;
    [TextArea,Header("Content")] public string content;
}