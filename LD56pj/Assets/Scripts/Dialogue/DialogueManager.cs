using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public TextMeshProUGUI contentText;
    //public TextMeshProUGUI playerNameText;
    public GameObject talkPanel;
    //public SpriteLib spriteLib;
    //public float brightnessWhenNotSpeak = 0.6f;
    //public Image leftCharacterImg;
    //public Image rightCharacterImg;
    public GameObject backGround;

    private int _remainDialogueNode;
    private int _dialogueIndex;
    private DialogueNode[] _dialogueNodes;
    private int _remainCharacterSpace = 2;
    private void Awake()
    {
        Instance = this;
        talkPanel.SetActive(false);
    }

    private void Update()
    {
        //backGround = transform.Find("GraphImg");
        if (Input.GetKeyDown(KeyCode.Space))//按下space进入下一个对话
        {
            PutDialogueNode();
        }
    }

    public void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        backGround = null;
        contentText.text = "";
    }
    public void PutDialogue(Dialogue dialogue)//放置对话段落
    {
        talkPanel.SetActive(true);
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
            talkPanel.SetActive(false);
            return;
        }
        
        _remainDialogueNode--;
        DialogueNode dialogueNode = _dialogueNodes[_dialogueIndex];
        _dialogueIndex++;
        if (dialogueNode.PNGSprite != null)
        {
            backGround.GetComponent<Image>().sprite = dialogueNode.PNGSprite;
        }
        /*if (!_charactersIsFull)
        {
            //set the character on talk panel
            //第一个出现在对话内的角色讲放在对话框右边,第二个放在左边
            if (_remainCharacterSpace == 2)
            {
                _remainCharacterSpace--;
                rightCharacterImg.enabled = true;
                rightName = dialogueNode.playerName;
                rightCharacterImg.sprite = spriteLib.GetSprite(dialogueNode.playerName);
            }
            else if (_remainCharacterSpace==1)
            {
                _remainCharacterSpace--;
                leftCharacterImg.enabled = true;
                leftName = dialogueNode.playerName;
                leftCharacterImg.sprite = spriteLib.GetSprite(dialogueNode.playerName);
                _charactersIsFull = true;
            }
        }
        //Set the text components
        contentText.text = dialogueNode.content;
        playerNameText.text = dialogueNode.playerName;
        
        //让不在对话时候的角色变灰
        if (!leftName.Equals(dialogueNode.playerName))
        {
            SetBrightNess(brightnessWhenNotSpeak,leftCharacterImg);
        }
        else
        {
            SetBrightNess(1,leftCharacterImg);
        }
        
        if (!rightName.Equals(dialogueNode.playerName))
        {
            SetBrightNess(brightnessWhenNotSpeak,rightCharacterImg);
        }
        else
        {
            SetBrightNess(1,rightCharacterImg);
        }*/
    }

    /*private void SetBrightNess(float brightness,Image img)
    {
        Color color = img.color;
        color.r = brightness;
        color.g = brightness;
        color.b = brightness;
        img.color = color;
    }*/
}
