using UnityEngine;

// This class is used to hold the data of the level.
[CreateAssetMenu(fileName = "LevelData", menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    [SerializeField] private int spinCounter; // Spin counter is the counter that holds the number of the spins that the player has played
    [SerializeField] private int gold; // Gold is the gold that the player has
    [SerializeField] private int maxEnergy; // Max energy is the maximum energy that the player can have
    [SerializeField] private int currentEnergy; // Current energy is the current energy that the player has
    // Slot element groups dictionary is the dictionary that holds the slot element groups with their spin counters
    [SerializeField] private SerializedDictionary<int, SlotElementGroup> slotElementGroupsDictionary = new(); 
    
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
    
    public void IncreaseSpinCounter()
    {
        spinCounter += 1;
    }
    
    public void ResetSpinCounter()
    {
        spinCounter = 0;
    }
    
    public void IncreaseGold(int value)
    {
        gold += value;
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
}