using System;
using UnityEngine;
using UnityEngine.UIElements;


[CreateAssetMenu(menuName = "CreatDialogue(一段对话)",fileName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public DialogueNode[] DialogueNodes;
}

[Serializable]
public class DialogueNode
{
    //[Header("Name")] public string playerName;
    [Header("PngSprite，如果没有设置则保持之前的图片不变")] public Sprite PNGSprite;
    [TextArea,Header("Content")] public string content;
}