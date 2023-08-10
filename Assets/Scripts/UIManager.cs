using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private SlotManager slotManager;
    
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI currentEnergyText;
    [SerializeField] private TextMeshProUGUI maxEnergyText;
    
    private void Awake()
    {
        InitializeUI();
    }
    
    private void InitializeUI()
    {
        // Set UI texts
        SetGoldText(levelData.GetGold().ToString());
        SetCurrentEnergyText(levelData.GetCurrentEnergy().ToString());
        SetMaxEnergyText(levelData.GetMaxEnergy().ToString());
    }
    
    public void OnClickSpinButton()
    {
        slotManager.StartSpin();
    }
    
    // Getter and Setter
    public void SetGoldText(string gold)
    {
        goldText.text = gold;
    }
    
    public void SetCurrentEnergyText(string currentEnergy)
    {
        currentEnergyText.text = currentEnergy;
    }

    private void SetMaxEnergyText(string maxEnergy)
    {
        maxEnergyText.text = maxEnergy;
    }
}