using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteLib", menuName = "SpriteLib(sprite与img的一一对应")]
public class SpriteLib : ScriptableObject
{
    public List<NSpair> pairs = new List<NSpair>();

    public Sprite GetSprite(string spriteName)
    {
        foreach (var pair in pairs)
        {
            if (pair.spriteName == spriteName)
            {
                return pair.img;
            }
        }
        return null;
    }
}

[System.Serializable]
public struct NSpair//name_sprite_pair
{
    [Header("Name")]
    public string spriteName;
    [Header("Image")]
    public Sprite img;
    
}