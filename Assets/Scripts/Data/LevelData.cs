using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    [SerializeField] private int spinCounter;
    [SerializeField] private int gold;
    [SerializeField] private int maxEnergy;
    [SerializeField] private int currentEnergy;
    [SerializeField] private SerializedDictionary<int, SlotElementGroup> slotElementGroupsDictionary = new SerializedDictionary<int, SlotElementGroup>();
    
    private LevelData()
    {
        FillWithDefaultValues();
    }
    
    private void FillWithDefaultValues()
    {
        spinCounter = 0;
        gold = 0;
        maxEnergy = 1000;
        currentEnergy = 1000;
    }
    
    public void IncreaseSpinCounter(int value = 1)
    {
        spinCounter += 1;
    }
    
    public void DecreaseSpinCounter(int value = 1)
    {
        currentEnergy -= value;
    }
    
    public void ResetSpinCounter()
    {
        spinCounter = 0;
    }
    
    public void IncreaseGold(int value)
    {
        gold += value;
    }
    
    public void IncreaseEnergy(int value = 1)
    {
        currentEnergy += value;
    }
    
    public void DecreaseEnergy(int value = 1)
    {
        currentEnergy -= value;
    }
    
    public void ResetEnergy()
    {
        currentEnergy = maxEnergy;
    }
    
    public void ClearSlotElementGroupsDictionary()
    {
        slotElementGroupsDictionary.Clear();
    }
    
    public void AddSlotElementGroupToDictionary(int index, SlotElementGroup slotElementGroup)
    {
        slotElementGroupsDictionary.Add(index, slotElementGroup);
    }
    
    public SlotElementGroup GetSlotElementGroup()
    {
         slotElementGroupsDictionary.TryGetValue(spinCounter, out var slotElementGroup);
         return slotElementGroup;
    }
    
    // Getters and Setters
    public int GetSpinCounter()
    {
        return spinCounter;
    }
    
    public int GetGold()
    {
        return gold;
    }
    
    public int GetMaxEnergy()
    {
        return maxEnergy;
    }
    
    public int GetCurrentEnergy()
    {
        return currentEnergy;
    }
    
    public SerializedDictionary<int, SlotElementGroup> GetSlotElementGroupsDictionary()
    {
        return slotElementGroupsDictionary;
    }
    
    public void SetSpinCounter(int value)
    {
        spinCounter = value;
    }
    
    public void SetGold(int value)
    {
        gold = value;
    }
    
    public void SetMaxEnergy(int value)
    {
        maxEnergy = value;
    }
    
    public void SetCurrentEnergy(int value)
    {
        currentEnergy = value;
    }
    
    public void SetSlotElementGroupsDictionary(SerializedDictionary<int, SlotElementGroup> dictionary)
    {
        slotElementGroupsDictionary = dictionary;
    }
    
}