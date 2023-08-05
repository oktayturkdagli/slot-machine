using System;
using UnityEngine;

[Serializable]
public class SlotElement
{
    public string name;
    public SlotElementType type;
    public Sprite normalSprite;
    public Sprite blurredSprite;

    public SlotElement(SlotElementType type, Sprite normalSprite, Sprite blurredSprite)
    {
        name = type.ToString();
        this.type = type;
        this.normalSprite = normalSprite;
        this.blurredSprite = blurredSprite;
    }
}

[Serializable]
public enum SlotElementType
{
    Jackpot,
    Wild,
    Seven,
    Bonus,
    A
}