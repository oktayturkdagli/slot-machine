using System;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private SlotManager slotManager;
    [SerializeField] private TimerManager timerManager;
    
    // Gold Animation
    [SerializeField] private Animation goldAnimation;
    
    // Slot Animation
    [SerializeField] private Transform[] checkPoints; // Start Point, Bingo Point, End Point
    private const float MarginOfDeviation = 10f; // Used when measuring the distance from one object to another
    private float _distanceThreshold; // Default distance between two Slot Element objects
    
    [SerializeField] private Transform[] slot1Elements;
    [SerializeField] private Transform[] slot2Elements; 
    [SerializeField] private Transform[] slot3Elements; 
    
    private const float CompletionTimeOfOneTurn = 0.1f;
    private const float Slot1Time = 0.1f;
    private const float Slot2Time = 0.1f;
    private const float Slot3Time = 2.5f;
    
    private SlotAnimationObject _slot1AnimationObject;
    private SlotAnimationObject _slot2AnimationObject;
    private SlotAnimationObject _slot3AnimationObject;
    
    public event Action OnEndAllSlotAnimations;
    
    private void Awake()
    {
        InitializeAnimations();
        slotManager.OnStartSpin -= StartSlotAnimations;
        slotManager.OnStartSpin += StartSlotAnimations;
    }
    
    private void OnDestroy()
    {
        slotManager.OnStartSpin -= StartSlotAnimations;
    }
    
    private void InitializeAnimations()
    {
        _distanceThreshold = Mathf.Abs(slot1Elements[0].localPosition.y - slot1Elements[1].localPosition.y) + MarginOfDeviation;
        _slot1AnimationObject = new SlotAnimationObject(slot1Elements, Slot1Time, slot1Elements[2], 4);
        _slot2AnimationObject = new SlotAnimationObject(slot2Elements, Slot2Time, slot2Elements[1], 8);
        _slot3AnimationObject = new SlotAnimationObject(slot3Elements, Slot3Time, slot3Elements[0], 12);
    }
    
    public void SetBingoElements(SlotElementType bingoElement1, SlotElementType bingoElement2, SlotElementType bingoElement3)
    {
        _slot1AnimationObject.BingoElement = slot1Elements[(int)bingoElement1];
        _slot2AnimationObject.BingoElement = slot2Elements[(int)bingoElement2];
        _slot3AnimationObject.BingoElement = slot3Elements[(int)bingoElement3];
    }
    
    // Gold Animations
    private void PlayGoldAnimation()
    {
        Debug.Log("Gold Animation Played");
    }
    
    // Slot Animations
    private void StartSlotAnimations()
    {
        StartSlotAnimationWithDelay(_slot1AnimationObject);
        StartSlotAnimationWithDelay(_slot2AnimationObject, Random.Range(0.1f, 0.3f));
        StartSlotAnimationWithDelay(_slot3AnimationObject, Random.Range(0.5f, 0.7f));
    }
    
    private void StartSlotAnimationWithDelay(SlotAnimationObject slotAnimationObject, float delay = 0)
    {
        slotAnimationObject.ResetAnimationObject();
        
        if (delay == 0)
        {
            OpenSprite(slotAnimationObject.SlotElements, true);
            PlaySlotAnimation(slotAnimationObject);
        }
        else
        {
            timerManager.CreateTimer(delay, () =>
            {
                OpenSprite(slotAnimationObject.SlotElements, true);
                PlaySlotAnimation(slotAnimationObject);
            });
        }
    }
    
    private void OpenSprite(Transform[] slotElements, bool willOpenBlurred = false)
    {
        foreach (var slotElement in slotElements)
        {
            slotElement.GetChild(0).gameObject.SetActive(!willOpenBlurred);
            slotElement.GetChild(1).gameObject.SetActive(willOpenBlurred);
        }
    }
    
    private void PlaySlotAnimation(SlotAnimationObject slotAnimationObject)
    {
        var targetPosition = CalculateTargetPositionsForAnimation(slotAnimationObject.SlotElements);
        
        // Play animations
        for (var i = 0; i < slotAnimationObject.SlotElements.Length; i++)
        {
            PlayElementAnimation(slotAnimationObject, slotAnimationObject.SlotElements[i], targetPosition[i], i == slotAnimationObject.SlotElements.Length - 1);
        }
    }
    
    private Vector3[] CalculateTargetPositionsForAnimation(Transform[] slotElements)
    {
        // Get check points
        var startPoint = checkPoints[0].localPosition;
        var endPoint = checkPoints[2].localPosition;
        
        // Get first Element
        var firstElement = slotElements[0];
        
        // Get target positions
        var targetPositions = new Vector3[slotElements.Length];
        for (var i = 0; i < slotElements.Length; i++)
        {
            var currentElement = slotElements[i];
            var nextElement = i + 1 < slotElements.Length ? slotElements[i + 1] : firstElement;
            if (Vector3.Distance(currentElement.localPosition, endPoint) < MarginOfDeviation)
            {
                currentElement.localPosition = startPoint;
                targetPositions[i] = startPoint;
            }
            targetPositions[i] = nextElement.localPosition;
            
            if (Vector3.Distance(currentElement.localPosition, nextElement.localPosition) > _distanceThreshold)
            {
                targetPositions[i] = endPoint;
            }
        }
        
        return targetPositions;
    }
    
    private void PlayElementAnimation(SlotAnimationObject slotAnimationObject, Transform element, Vector3 targetPosition, bool isLastElement = false)
    {
        var currentAnimationDuration = CompletionTimeOfOneTurn / slotAnimationObject.SlotElements.Length;
        if (slotAnimationObject.IsSlowed)
            currentAnimationDuration = slotAnimationObject.SlotTime / 3;
        
        element.DOLocalMove(targetPosition, currentAnimationDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            element.localPosition = targetPosition;
            if (isLastElement)
            {
                CheckLoop(slotAnimationObject);
            }
        });
    }
    
    private void CheckLoop(SlotAnimationObject slotAnimationObject)
    {
        slotAnimationObject.AnimationPlayCounter++;
        if (slotAnimationObject.AnimationPlayCounter % 5 == 0)
            slotAnimationObject.TurnCounter++;
        
        if (slotAnimationObject.TurnCounter >= slotAnimationObject.ShouldTurnFast && !slotAnimationObject.IsSlowed && IsElementOnEndPoint(slotAnimationObject.BingoElement))
            SlowDownSlot(slotAnimationObject);
        
        else if (slotAnimationObject.IsSlowed && IsElementOnBingoPoint(slotAnimationObject.BingoElement))
        {
            slotAnimationObject.IsAnimationFinished = true;
            StopElementAnimation();
        }
        
        if (!slotAnimationObject.IsAnimationFinished)
            PlaySlotAnimation(slotAnimationObject);
    }
    
    private void SlowDownSlot(SlotAnimationObject slotAnimationObject)
    {
        slotAnimationObject.IsSlowed = true;
        OpenSprite(slotAnimationObject.SlotElements);
    }
    
    private void StopElementAnimation()
    {
        if (_slot1AnimationObject.IsAnimationFinished && _slot2AnimationObject.IsAnimationFinished && _slot3AnimationObject.IsAnimationFinished)
        {
            OnEndAllSlotAnimations?.Invoke();
        }
    }
    
    private bool IsElementOnEndPoint(Transform element)
    {
        var endPoint = checkPoints[2].localPosition;
        return Vector3.Distance(element.localPosition, endPoint) < MarginOfDeviation;
    }
    
    private bool IsElementOnBingoPoint(Transform element)
    {
        var bingoPoint = checkPoints[1].localPosition;
        return Vector3.Distance(element.localPosition, bingoPoint) < MarginOfDeviation;
    }
}