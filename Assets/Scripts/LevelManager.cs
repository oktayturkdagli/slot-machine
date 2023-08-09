using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelData leveData;
    [SerializeField] private ProbabilityManager probabilityManager;
    
    private void Awake()
    {
        CreateLevel();
    }
    
    [ContextMenu("Create Level")]
    public void CreateLevel()
    {
        ClearLevel();
        
        var listOfWonders = probabilityManager.TryYourBest();
        for (var i = 0; i < listOfWonders.Count; i++)
        {
            leveData.GetSlotElementGroupsDictionary().Add(i, listOfWonders[i]);
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