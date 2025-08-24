using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private LevelData leveData;
    private ProbabilityManager _probabilityManager;
    
    private void Awake()
    {
        _probabilityManager = new ProbabilityManager(gameData);
        CreateLevel();
    }
    
    // This method is called from the context menu of the LevelManager script
    [ContextMenu("Create Level")]
    public void CreateLevel()
    {
        ClearLevel();
        
        // Periodically placed objects are thrown into a dictionary for easier access
        var periodicallyPlacedObjects = _probabilityManager.GenerateSlotPool();
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