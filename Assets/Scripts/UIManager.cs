using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image[] slotImages; // The first three images are in the middle, the other images are top and bottom
    [SerializeField] private GameData gameData;
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
        SetGoldText(levelData.GetGold().ToString());
        SetCurrentEnergyText(levelData.GetCurrentEnergy().ToString());
        SetMaxEnergyText(levelData.GetMaxEnergy().ToString());
    }
    
    public void OnClickSpinButton()
    {
        slotManager.StartSpin();
    }
    
    public void ChangeSprites(SlotElementType[] sprites)
    {
        for (var i = 0; i < 3; i++)
        {
            slotImages[i].sprite = gameData.GetSprite(sprites[i]);
        }
        
        for (var i = 3; i < slotImages.Length; i++)
        {
            slotImages[i].sprite = gameData.GetRandomSprite();
        }
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