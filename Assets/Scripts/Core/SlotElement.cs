using System;
using UnityEngine;

[Serializable]
public class SlotElement
{
    [field: SerializeField] public SlotElementType ElementType { get; set; } // element type is the type of the slot element
    [field: SerializeField] public Sprite NormalSprite { get; set; } // normal sprite is the sprite that will be shown when the slot is not blurred
    [field: SerializeField] public Sprite BlurredSprite { get; set; } // blurred sprite is the sprite that will be shown when the slot is blurred

    public SlotElement(SlotElementType elementType, Sprite normalSprite, Sprite blurredSprite)
    {
        ElementType = elementType;
        NormalSprite = normalSprite;
        BlurredSprite = blurredSprite;
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