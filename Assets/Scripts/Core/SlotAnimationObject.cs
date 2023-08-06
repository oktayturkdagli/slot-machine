using UnityEngine;

public class SlotAnimationObject
{
    public readonly Transform[] SlotElements;
    public readonly float SlotTime;
    public Transform BingoElement;
    public bool IsSlowed;
    public int AnimationPlayCounter;
    public int TurnCounter;
    public readonly int ShouldTurnFast;
    public bool IsAnimationFinished;
    
    public SlotAnimationObject(Transform[] slotElements, float slotTime, Transform bingoElement, int shouldTurnFast = 2)
    {
        SlotElements = slotElements;
        SlotTime = slotTime;
        BingoElement = bingoElement;
        ShouldTurnFast = shouldTurnFast;
    }
    
    public void ResetAnimationObject()
    {
        IsSlowed = false;
        AnimationPlayCounter = 0;
        TurnCounter = 0;
        IsAnimationFinished = false;
    }
}