using System;

[Serializable]
public class SlotElementGroup
{
    public string name;
    public int possibility;
    public int goldValue;
    public SlotElementType[] tripleGroup;

    public SlotElementGroup(int possibility, int goldValue, SlotElementType[] tripleGroup)
    {
        name = tripleGroup[0] + "-" + tripleGroup[1] + "-" + tripleGroup[2];
        this.possibility = possibility;
        this.goldValue = goldValue;
        this.tripleGroup = tripleGroup;
    }
}