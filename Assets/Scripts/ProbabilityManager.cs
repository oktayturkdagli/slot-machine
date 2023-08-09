using System.Collections.Generic;
using UnityEngine;

public class ProbabilityManager : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    private List<SlotElementGroup> _periodicallyPlacedObjects = new List<SlotElementGroup>(new SlotElementGroup[100]);
    private readonly List<SlotElementGroup> _notPlacedElements = new List<SlotElementGroup>();
    
    public List<SlotElementGroup> TryYourBest()
    {
        _periodicallyPlacedObjects = new List<SlotElementGroup>(new SlotElementGroup[100]);
        _notPlacedElements.Clear();
        foreach (var slotElementGroup in gameData.GetSlotElementGroups())
        {
            var periodSize = Mathf.CeilToInt(100 / (float)slotElementGroup.possibility);
            PlaceElements(slotElementGroup, periodSize);
        }
        
        PlaceNotPlacedElements();
        TestPeriodicallyPlacedObjects();
        return _periodicallyPlacedObjects;
    }
    
    private void PlaceElements(SlotElementGroup slotElementGroup, int periodSize)
    {
        var periodStart = 0;
        var periodEnd = periodSize;
        var currentCounter = 0;
        
        var suitablePlaces = new List<int>();
        while (currentCounter < 99)
        {
            suitablePlaces.Clear();
            for (var i = periodStart; i < periodEnd; i++)
            {
                if (_periodicallyPlacedObjects[i] == null)
                {
                    suitablePlaces.Add(i);
                }
            }
            
            if (suitablePlaces.Count > 0)
            {
                _periodicallyPlacedObjects[suitablePlaces[Random.Range(0, suitablePlaces.Count)]] = slotElementGroup;
            }
            else
            {
                _notPlacedElements.Add(slotElementGroup);
            }
            
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
        
        for (var i = 0; i < _periodicallyPlacedObjects.Count; i++)
        {
            if (_periodicallyPlacedObjects[i] != null) 
                continue;
            
            var notPlacedObject = _notPlacedElements[Random.Range(0, _notPlacedElements.Count)];
            _periodicallyPlacedObjects[i] = notPlacedObject;
            _notPlacedElements.Remove(notPlacedObject);
        }
    }
    
    private void TestPeriodicallyPlacedObjects()
    {
        foreach (var slotElementGroup in gameData.GetSlotElementGroups())
        {
            var counter = 0;
            foreach (var element in _periodicallyPlacedObjects)
            {
                if (element == slotElementGroup)
                    counter++;
            }
            
            Debug.Log(slotElementGroup.tripleGroup[0] + " " + slotElementGroup.tripleGroup[1] + " " + slotElementGroup.tripleGroup[2] + " %" + counter);
        }
    }
}