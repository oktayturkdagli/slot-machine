using UnityEngine;

// This class is used to hold the data of the game.
[CreateAssetMenu(fileName = "GameData", menuName = "Game Data")]
public class GameData : ScriptableObject
{
    [SerializeField] private SerializedDictionary<SlotElementType, SlotElement> slotElements; // hold the sprites of the elements
    [SerializeField] private SlotElementGroup[] slotElementGroups; // hold the groups of the elements and its possibilities
    
    private GameData()
    {
        FillWithDefaultValues();
    }
    
    private void FillWithDefaultValues()
    {
        FillSlotElementsWithDefaultValues();
        FillSlotElementGroupsWithDefaultValues();
    }
    
    private void FillSlotElementsWithDefaultValues()
    {
        slotElements = new SerializedDictionary<SlotElementType, SlotElement>
        {
            [SlotElementType.Jackpot] = new(SlotElementType.Jackpot, null, null),
            [SlotElementType.Wild] = new(SlotElementType.Wild, null, null),
            [SlotElementType.Seven] = new(SlotElementType.Seven, null, null),
            [SlotElementType.Bonus] = new(SlotElementType.Bonus, null, null),
            [SlotElementType.A] = new(SlotElementType.A, null, null)
        };
    }
    
    private void FillSlotElementGroupsWithDefaultValues()
    {
        slotElementGroups = new SlotElementGroup[10];
        slotElementGroups[0] = new SlotElementGroup(13,0, new[] {SlotElementType.A, SlotElementType.Wild, SlotElementType.Bonus});
        slotElementGroups[1] = new SlotElementGroup(13,0, new[] {SlotElementType.Wild, SlotElementType.Wild, SlotElementType.Seven});
        slotElementGroups[2] = new SlotElementGroup(13,0, new[] {SlotElementType.Jackpot, SlotElementType.Jackpot, SlotElementType.A});
        slotElementGroups[3] = new SlotElementGroup(13,0, new[] {SlotElementType.Wild, SlotElementType.Bonus, SlotElementType.A});
        slotElementGroups[4] = new SlotElementGroup(13,0, new[] {SlotElementType.Bonus, SlotElementType.A, SlotElementType.Jackpot});
        slotElementGroups[5] = new SlotElementGroup(9,100, new[] {SlotElementType.A, SlotElementType.A, SlotElementType.A});
        slotElementGroups[6] = new SlotElementGroup(8,200, new[] {SlotElementType.Bonus, SlotElementType.Bonus, SlotElementType.Bonus});
        slotElementGroups[7] = new SlotElementGroup(7,300, new[] {SlotElementType.Seven, SlotElementType.Seven, SlotElementType.Seven});
        slotElementGroups[8] = new SlotElementGroup(6,400, new[] {SlotElementType.Wild, SlotElementType.Wild, SlotElementType.Wild});
        slotElementGroups[9] = new SlotElementGroup(5,500, new[] {SlotElementType.Jackpot, SlotElementType.Jackpot, SlotElementType.Jackpot});
    }
    
    public SlotElementGroup[] GetSlotElementGroups()
    {
        return slotElementGroups;
    }
}