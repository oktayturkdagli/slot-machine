using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProbabilityManager : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    private List<SlotElementGroup> _periodicallyPlacedObjects = new List<SlotElementGroup>(new SlotElementGroup[100]);
    private readonly List<SlotElementGroup> _notPlacedElements = new List<SlotElementGroup>();
    
    public List<SlotElementGroup> GenerateSlotPool()
    {
        // Clear data
        _periodicallyPlacedObjects = new List<SlotElementGroup>(new SlotElementGroup[100]);
        _notPlacedElements.Clear();
        
        // Place elements one-by-one
        foreach (var slotElementGroup in gameData.GetSlotElementGroups())
        {
            var periodSize = Mathf.CeilToInt(100 / (float)slotElementGroup.Possibility);
            PlaceElements(slotElementGroup, periodSize);
        }
        
        // Place not placed elements
        PlaceNotPlacedElements();
        
        // Print results
        TestPeriodicallyPlacedObjects();
        
        // Return result
        return _periodicallyPlacedObjects;
    }
    
    private void PlaceElements(SlotElementGroup slotElementGroup, int periodSize)
    {
        var periodStart = 0;
        var periodEnd = periodSize;
        var currentCounter = 0;
        var suitablePlaces = new List<int>();
        
        // Place elements in periods
        while (currentCounter < 99)
        {
            // Find suitable places
            suitablePlaces.Clear();
            for (var i = periodStart; i < periodEnd; i++)
            {
                if (_periodicallyPlacedObjects[i] == null)
                {
                    suitablePlaces.Add(i);
                }
            }
            
            // Place element
            if (suitablePlaces.Count > 0)
            {
                _periodicallyPlacedObjects[suitablePlaces[Random.Range(0, suitablePlaces.Count)]] = slotElementGroup;
            }
            
            // Hold not placed elements
            else
            {
                _notPlacedElements.Add(slotElementGroup);
            }
            
            // Update counters
            currentCounter += periodSize;
            periodStart += periodSize;
            periodEnd += periodSize;
            
            if (periodStart > 99)
                break;
            
            if (periodEnd > 100)
                periodEnd = 100;
        }
    }
    
    private void PlaceNotPlacedElements()
    {
        if (_notPlacedElements.Count == 0)
            return;
        
        // Place not placed elements in empty places
        for (var i = 0; i < _periodicallyPlacedObjects.Count; i++)
        {
            if (_periodicallyPlacedObjects[i] != null) 
                continue;
            
            // Take random not placed element
            var notPlacedObject = _notPlacedElements[Random.Range(0, _notPlacedElements.Count)];
            
            // Place element in empty place
            _periodicallyPlacedObjects[i] = notPlacedObject;
            
            // Remove element from not placed list
            _notPlacedElements.Remove(notPlacedObject);
        }
    }
    
    private void TestPeriodicallyPlacedObjects()
    {
        // Print elements and its probability results
        foreach (var slotElementGroup in gameData.GetSlotElementGroups())
        {
            var counter = _periodicallyPlacedObjects.Count(element => element == slotElementGroup);
            var text = slotElementGroup.TripleGroup[0] + " " + slotElementGroup.TripleGroup[1] + " " + slotElementGroup.TripleGroup[2];
            text += " | Requested : %" + slotElementGroup.Possibility;
            text += " | Real value : %" + counter;
            if (slotElementGroup.Possibility == counter)
            {
                text += " | <color=green>OK</color>";
                Debug.Log(text);
            }
            else
            {
                text += " |  <color=red>ERROR</color>";
                Debug.LogWarning(text);
            }
        }
    }
}