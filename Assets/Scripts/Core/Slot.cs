using UnityEngine;

public class Slot
{
    public Transform[] SlotElements { get; set; } // Slot elements are the elements that will be turned in the slot
    public float SlotAnimationTime { get; set; } // How long the slot animation will take
    public Transform BingoElement { get; set; } // Bingo element is the element that will be in the middle of the slot when the animation is finished
    public bool IsSlotAnimationSlowed { get; set; } // Is the slot slowed down
    public int SlotElementMoveAnimationCounter { get; set; } // How many times the slot elements animation has been played
    public int TurnCounter { get; set; } // How many times the slot has been turned
    public int ShouldTurnFast { get; set; } // How many times the slot should turn fast
    public bool IsAnimationFinished { get; set; } // Is the slot animation completely finished
    
    public Slot(Transform[] slotElements, float slotAnimationTime, Transform bingoElement, int shouldTurnFast = 2)
    {
        SlotElements = slotElements;
        SlotAnimationTime = slotAnimationTime;
        BingoElement = bingoElement;
        ShouldTurnFast = shouldTurnFast;
    }
    
    public void ResetAnimationObject()
    {
        IsSlotAnimationSlowed = false;
        SlotElementMoveAnimationCounter = 0;
        TurnCounter = 0;
        IsAnimationFinished = false;
    }
}