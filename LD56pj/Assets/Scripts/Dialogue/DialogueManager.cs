using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public TextMeshProUGUI contentText;
    public GameObject talkPanel;
    public Image backgroundImage;
    private int _remainDialogueNode;
    private int _dialogueIndex;
    private DialogueNode[] _dialogueNodes;
    private int _remainCharacterSpace = 2;
    private void Start()
    {
        Instance = this;
        talkPanel.SetActive(false);
        if (talkPanel==null)
        {
            Debug.Log("talkPanel is null");
        }

        if (contentText ==null)
        {
            Debug.Log("contentText is null");
        }
    }

    private void Update()
    {
        //backGround = transform.Find("GraphImg");
        if (Input.GetKeyDown(KeyCode.Tab))//按下space进入下一个对话
        {
            Debug.Log("get key down");
            PutDialogueNode();
        }
    }

    public void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        // 创建一个透明的 Texture2D
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(0, 0, 0, 0)); // 透明色
        texture.Apply();

        // 创建一个 Sprite
        Sprite transparentSprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        
        // 将背景图像设置为透明 Sprite
        backgroundImage.sprite = transparentSprite;
        contentText.text = "";
    }
    public void PutDialogue(Dialogue dialogue)//放置对话段落
    {
        SetDialogueState(true);
        Init();
        _remainDialogueNode = dialogue.DialogueNodes.Length;
        _dialogueNodes = dialogue.DialogueNodes;
        _dialogueIndex = 0;
        PutDialogueNode();
    }
    public void PutDialogueNode()//放置一句话
    {
        if (_remainDialogueNode<=0)
        {
            SetDialogueState(false);
            return;
        }
        
        _remainDialogueNode--;
        DialogueNode dialogueNode = _dialogueNodes[_dialogueIndex];
        _dialogueIndex++;
        contentText.text = dialogueNode.content;
        if (dialogueNode.PNGSprite != null)
        {
            backgroundImage.sprite = dialogueNode.PNGSprite;
        }
    }

    public void SetDialogueState(bool state)
    {
        talkPanel.SetActive(state);
        if (state==true)
        {
            Time.timeScale = 0;
        }

        if (state==false)
        {
            Time.timeScale = 1;
        }
    }
}
