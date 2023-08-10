using System;
using UnityEngine;

[Serializable]
public class SlotElementGroup
{
    [field: SerializeField] public int Possibility { get; set; } // Possibility is the possibility of the triple group to be shown in the slot
    [field: SerializeField] public int GoldValue { get; set; } // Gold value is the gold value of the triple group
    [field: SerializeField] public SlotElementType[] TripleGroup { get; set; } // Triple group is the group of the slot elements that will be shown in the slot
    
    public SlotElementGroup(int possibility, int goldValue, SlotElementType[] tripleGroup)
    {
        Possibility = possibility;
        GoldValue = goldValue;
        TripleGroup = tripleGroup;
    }
}