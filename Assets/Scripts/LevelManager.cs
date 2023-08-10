using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelData leveData;
    [SerializeField] private ProbabilityManager probabilityManager;
    
    private void Awake()
    {
        CreateLevel();
    }
    
    // This method is called from the context menu of the LevelManager script
    [ContextMenu("Create Level")]
    public void CreateLevel()
    {
        ClearLevel();
        
        // Periodically placed objects are thrown into a dictionary for easier access
        var periodicallyPlacedObjects = probabilityManager.GenerateSlotPool();
        var slotElementGroupsDictionary = leveData.GetSlotElementGroupsDictionary();
        for (var i = 0; i < periodicallyPlacedObjects.Count; i++)
        {
            slotElementGroupsDictionary.Add(i, periodicallyPlacedObjects[i]);
        }
    }
    
    private void ClearLevel()
    {
        leveData.ClearSlotElementGroupsDictionary();
    }
}