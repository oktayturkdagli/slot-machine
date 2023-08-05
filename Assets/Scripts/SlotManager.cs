using System;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private UIManager uiManager;
    private bool isSpining = false;
    
    public event Action OnStartSpin;
    public event Action OnEndSpin;

    private void Awake()
    {
        InitializeSlot();
    }
    
    private void InitializeSlot()
    {
        if (levelData.GetSlotElementGroupsDictionary().Count == 0)
            levelManager.CreateLevel();
    }

    public void StartSpin()
    {
        if (isSpining)
            return;
        
        var slotElementGroup = levelData.GetSlotElementGroup();
        if (slotElementGroup == default)
        {
            // TODO: If null, create new level
            Debug.Log("Hey, I did not find anything!");
            return;
        }
        
        isSpining = true;
        levelData.IncreaseSpinCounter();
        // levelData.DecreaseEnergy();
        uiManager.SetCurrentEnergyText(levelData.GetCurrentEnergy().ToString());
        uiManager.ChangeSprites(slotElementGroup.tripleGroup);
        OnStartSpin?.Invoke();
        EndSpin();
    }
    
    public void EndSpin()
    {
        isSpining = false;
        var slotElementGroup = levelData.GetSlotElementGroup();
        if (slotElementGroup == default)
        {
            Debug.Log("Hey, I did not find anything!");
            return;
        }
        levelData.IncreaseGold(slotElementGroup.goldValue);
        uiManager.SetGoldText(levelData.GetGold().ToString());
        OnEndSpin?.Invoke();
    }
}