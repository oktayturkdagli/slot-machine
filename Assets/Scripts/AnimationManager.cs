using System;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private SlotManager slotManager; 
    [SerializeField] private TimerManager timerManager;
    
    // Gold Animation
    [SerializeField] private ParticleSystem goldEffect;
    
    // Slot Animation
    [SerializeField] private Transform[] checkPoints; // Start Point, Bingo Point, End Point
    private const float MarginOfDeviation = 10f; // Used when measuring the distance from one object to another
    private float _distanceThreshold; // Default distance between two Slot Element objects
    
    [SerializeField] private Transform[] slot1Elements;
    [SerializeField] private Transform[] slot2Elements; 
    [SerializeField] private Transform[] slot3Elements; 
    
    private const float CompletionTimeOfOneTurn = 0.1f; // Time to complete one fast turn
    private const float Slot1Time = 0.1f;
    private const float Slot2Time = 0.1f;
    private const float Slot3Time = 2.5f;
    
    private Slot _slot1;
    private Slot _slot2;
    private Slot _slot3;
    
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
        _slot1 = new Slot(slot1Elements, Slot1Time, slot1Elements[2], 4);
        _slot2 = new Slot(slot2Elements, Slot2Time, slot2Elements[1], 8);
        _slot3 = new Slot(slot3Elements, Slot3Time, slot3Elements[0], 12);
    }
    
    public void SetBingoElements(SlotElementType bingoElement1, SlotElementType bingoElement2, SlotElementType bingoElement3)
    {
        _slot1.BingoElement = slot1Elements[(int)bingoElement1];
        _slot2.BingoElement = slot2Elements[(int)bingoElement2];
        _slot3.BingoElement = slot3Elements[(int)bingoElement3];
    }
    
    // Gold Animations
    public void PlayGoldEffect()
    {
        goldEffect.Play();
    }
    
    // Slot Animations
    private void StartSlotAnimations()
    {
        StartSlotAnimationWithDelay(_slot1);
        StartSlotAnimationWithDelay(_slot2, Random.Range(0.1f, 0.3f));
        StartSlotAnimationWithDelay(_slot3, Random.Range(0.5f, 0.7f));
    }
    
    private void StartSlotAnimationWithDelay(Slot slot, float delay = 0)
    {
        // Reset animation object of this slot
        slot.ResetAnimationObject();
        
        // Open blurred sprites and play animation
        if (delay == 0)
        {
            OpenSprite(slot.SlotElements, true);
            PlaySlotAnimation(slot);
        }
        else
        {
            // Open blurred sprites after delay
            timerManager.CreateTimer(delay, () =>
            {
                OpenSprite(slot.SlotElements, true);
                PlaySlotAnimation(slot);
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
    
    private void PlaySlotAnimation(Slot slot)
    {
        // Calculate target positions
        var targetPosition = CalculateTargetPositionsForAnimation(slot.SlotElements);
        
        // Play animations
        for (var i = 0; i < slot.SlotElements.Length; i++)
        {
            PlayElementAnimation(slot, slot.SlotElements[i], targetPosition[i], i == slot.SlotElements.Length - 1);
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
    
    private void PlayElementAnimation(Slot slot, Transform element, Vector3 targetPosition, bool isLastElement = false)
    {
        // Calculate animation duration
        var currentAnimationDuration = CompletionTimeOfOneTurn / slot.SlotElements.Length;
        
        // If slot is slowed, calculate slowed animation time
        if (slot.IsSlotAnimationSlowed)
            currentAnimationDuration = slot.SlotAnimationTime / 3;
        
        // Play animation
        element.DOLocalMove(targetPosition, currentAnimationDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            // Set element position to target position
            element.localPosition = targetPosition;
            slot.SlotElementMoveAnimationCounter++;
            
            // If this is last element, check loop, because one turn is completed
            if (isLastElement)
            {
                CheckLoop(slot);
            }
        });
    }
    
    private void CheckLoop(Slot slot)
    {
        // AnimationPlayCounter % 5 == 0 means one turn is completed
        if (slot.SlotElementMoveAnimationCounter % 5 == 0)
            slot.TurnCounter++;
        
        // If turn counter is equal to should turn fast, slow down slot
        if (slot.TurnCounter >= slot.ShouldTurnFast && !slot.IsSlotAnimationSlowed && IsElementOnEndPoint(slot.BingoElement))
            SlowDownSlot(slot);
        
        // If slot is slowed and element is on bingo point, stop animation
        else if (slot.IsSlotAnimationSlowed && IsElementOnBingoPoint(slot.BingoElement))
        {
            slot.IsAnimationFinished = true;
            StopElementAnimation();
        }
        
        // If the slot animation is not finished, continue playing the animation
        if (!slot.IsAnimationFinished)
            PlaySlotAnimation(slot);
    }
    
    private void SlowDownSlot(Slot slot)
    {
        slot.IsSlotAnimationSlowed = true;
        OpenSprite(slot.SlotElements);
    }
    
    private void StopElementAnimation()
    {
        // If all slots are finished, stop all animations and invoke OnEndAllSlotAnimations event
        if (!_slot1.IsAnimationFinished || !_slot2.IsAnimationFinished || !_slot3.IsAnimationFinished) 
            return;
        
        DOTween.KillAll();
        OnEndAllSlotAnimations?.Invoke();
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