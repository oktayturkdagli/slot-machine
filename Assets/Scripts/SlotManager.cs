using System;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private AnimationManager animationManager;
    private bool _isSpinning;
    
    public event Action OnStartSpin;
    
    private void Awake()
    {
        InitializeSlot();
    }

    private void OnDestroy()
    {
        DeinitializeSlot();
    }

    private void InitializeSlot()
    {
        animationManager.OnEndAllSlotAnimations -= EndSpin;
        animationManager.OnEndAllSlotAnimations += EndSpin;
        
        // If slot element group dictionary is empty, create a new level
        if (levelData.GetSlotElementGroupsDictionary().Count == 0)
            levelManager.CreateLevel();
    }
    
    private void DeinitializeSlot()
    {
        animationManager.OnEndAllSlotAnimations -= EndSpin;
    }

    public void StartSpin()
    {
        if (_isSpinning)
            return;
        
        _isSpinning = true;
        levelData.DecreaseEnergy();
        uiManager.SetCurrentEnergyText(levelData.GetCurrentEnergy().ToString());
        var slotElementGroup = levelData.GetSlotElementGroup();
        animationManager.SetBingoElements(slotElementGroup.TripleGroup[0], slotElementGroup.TripleGroup[1], slotElementGroup.TripleGroup[2]);
        OnStartSpin?.Invoke();
    }

    private void EndSpin()
    {
        _isSpinning = false;
        
        var slotElementGroup = levelData.GetSlotElementGroup();
        if (slotElementGroup.GoldValue > 0)
            animationManager.PlayGoldEffect(slotElementGroup.GoldValue / 2);
        levelData.IncreaseGold(slotElementGroup.GoldValue);
        uiManager.SetGoldText(levelData.GetGold().ToString());
        levelData.IncreaseSpinCounter();
        
        if (levelData.GetSpinCounter() == 100)
        {
            levelManager.CreateLevel();
            levelData.ResetSpinCounter();
            levelData.ResetEnergy();
            uiManager.SetCurrentEnergyText(levelData.GetCurrentEnergy().ToString());
        }
    }
}