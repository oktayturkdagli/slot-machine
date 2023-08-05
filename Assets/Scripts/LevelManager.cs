using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private LevelData leveData;
    
    private void Awake()
    {
        CreateLevel();
    }
    
    public void CreateLevel()
    {
        ClearLevel();
        
        var counter = 0;
        foreach (var slotElementGroup in gameData.GetSlotElementGroups())
        {
            for (var i = 0; i < slotElementGroup.possibility; i++)
            {
                leveData.GetSlotElementGroupsDictionary().Add(counter, slotElementGroup);
                counter++;
            }
        }
    }
    
    private void ClearLevel()
    {
        leveData.ClearSlotElementGroupsDictionary();
    }
    
    private void RestartLevel()
    {
        leveData.SetSpinCounter(0);
    }
}