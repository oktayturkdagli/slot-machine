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
    public event Action OnEndSpin;

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
        animationManager.OnEndAllSlotAnimations -= OnEndAllSlotAnimations;
        animationManager.OnEndAllSlotAnimations += OnEndAllSlotAnimations;
        
        if (levelData.GetSlotElementGroupsDictionary().Count == 0)
            levelManager.CreateLevel();
    }
    
    private void DeinitializeSlot()
    {
        animationManager.OnEndAllSlotAnimations -= OnEndAllSlotAnimations;
    }

    public void StartSpin()
    {
        if (_isSpinning)
            return;
        
        var slotElementGroup = levelData.GetSlotElementGroup();
        if (slotElementGroup == default)
        {
            // TODO: If null, create new level
            Debug.Log("Hey, I did not find anything!");
            return;
        }
        
        _isSpinning = true;
        levelData.IncreaseSpinCounter();
        levelData.DecreaseEnergy();
        uiManager.SetCurrentEnergyText(levelData.GetCurrentEnergy().ToString());
        animationManager.SetBingoElements(slotElementGroup.tripleGroup[0], slotElementGroup.tripleGroup[1], slotElementGroup.tripleGroup[2]);
        OnStartSpin?.Invoke();
    }

    private void EndSpin()
    {
        _isSpinning = false;
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

    private void OnEndAllSlotAnimations()
    {
        EndSpin();
    }
}