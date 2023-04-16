using DG.Tweening;
using GG.Infrastructure.Utils.Swipe;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SlotType { Type1, Type2, Type3, Type4, Type5 }
public class Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public SlotType slotType; 

    [SerializeField] private SlotRayCast slotLeftRay;
    [SerializeField] private SlotRayCast slotRightRay;
    [SerializeField] private SlotRayCast slotUpRay;
    [SerializeField] private SlotRayCast slotDownRay;

    [SerializeField] private bool randomType;

    [SerializeField] private Image iconeImage;

    private bool _touch;

    private void Start()
    {
        SwipeListener.Instance.OnSwipe.AddListener(CheckSides);

        if(randomType)
            SetRandomType();
    }
    private void OnValidate()
    {
        if (!iconeImage) return;
        switch (slotType)
        {
            case SlotType.Type1:
                iconeImage.color = Color.yellow;
                break;
            case SlotType.Type2:
                iconeImage.color = Color.cyan;
                break;
            case SlotType.Type3:
                iconeImage.color = Color.green;
                break;
            case SlotType.Type4:
                iconeImage.color = Color.blue;
                break;
            case SlotType.Type5:
                iconeImage.color = Color.red;
                break;
        }
    }
    private void OnDestroy()
    {
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _touch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _touch = false;
    }

    private void CheckSides(string DirectionName) 
    {
        if(!_touch) return;

        switch (DirectionName)
        {
            case "Left":
                Slot slotLeft = slotLeftRay.GetSlot();
                if (slotLeft) 
                    Move(slotLeft);
                break;
                //-----------------
            case "Right":
                Slot slotRight = slotRightRay.GetSlot();
                if (slotRight)
                    Move(slotRight);
                break;
                //-----------------
            case "Up":
                Slot slotUp = slotUpRay.GetSlot();
                if (slotUp)
                    Move(slotUp);
                break;
                //-----------------
            case "Down":
                Slot slotDown = slotDownRay.GetSlot();
                if (slotDown)
                    Move(slotDown);
                break;
        }
    }

    private void Move(Slot slot)
    {
        SlotController.Instance.VerticalSlots = new List<Slot>();
        SlotController.Instance.HorizontalSlots = new List<Slot>();

        var firstOtherSlotPos = slot.transform.position;
        var firstSelfSlotPos = transform.position;
        transform.DOMove(slot.transform.position, 0.2f).OnComplete(() => 
        {
            SlotController.Instance.VerticalSlots.Add(this);
            SlotController.Instance.HorizontalSlots.Add(this);

            string[] directions = { "Left", "Right", "Up", "Down" };
            CheckDirectionSlot(directions);

            if (SlotController.Instance.VerticalSlots.Count >= 3)
            {
                SlotController.Instance.DestroyVericalSlots();
            }
            else if (SlotController.Instance.HorizontalSlots.Count >= 3)
            {
                SlotController.Instance.DestroyHorizontalSlots();
            }
            else
            {
                slot.transform.DOMove(firstOtherSlotPos, 0.2f).SetDelay(0.2f);
                transform.DOMove(firstSelfSlotPos, 0.2f).SetDelay(0.2f);
            }
        });
        slot.transform.DOMove(transform.position, 0.2f);
    }
    private void SetRandomType() 
    {
        slotType = (SlotType)Random.Range(0, 5);
        switch (slotType)
        {
            case SlotType.Type1:
                iconeImage.color = Color.yellow;
                break;
            case SlotType.Type2:
                iconeImage.color = Color.cyan;
                break;
            case SlotType.Type3:
                iconeImage.color = Color.green;
                break;
            case SlotType.Type4:
                iconeImage.color = Color.blue;
                break;
            case SlotType.Type5:
                iconeImage.color = Color.red;
                break;
        }
    }

    public void CheckDirectionSlot(string[] directions) 
    {
        for (int i = 0; i < directions.Length; i++)
        {
            switch (directions[i])
            {
                case "Left":
                    var leftSlot = slotLeftRay.GetSlot();
                    if(leftSlot && leftSlot.slotType == slotType)
                    {
                        SlotController.Instance.HorizontalSlots.Add(leftSlot);
                        string[] derictionLeft = { "Left" };
                        leftSlot.CheckDirectionSlot(derictionLeft);
                    }
                    break;
                //-----------------
                case "Right":
                    var rightSlot = slotRightRay.GetSlot();
                    if (rightSlot && rightSlot.slotType == slotType)
                    {
                        SlotController.Instance.HorizontalSlots.Add(rightSlot);
                        string[] derictionRight = { "Right" };
                        rightSlot.CheckDirectionSlot(derictionRight);
                    }
                    break;
                //-----------------
                case "Up":
                    var upSlot = slotUpRay.GetSlot();
                    if (upSlot && upSlot.slotType == slotType)
                    {
                        SlotController.Instance.VerticalSlots.Add(upSlot);
                        string[] derictionUp = { "Up" };
                        upSlot.CheckDirectionSlot(derictionUp);
                    }
                    break;
                //-----------------
                case "Down":
                    var downSlot = slotDownRay.GetSlot();
                    if (downSlot && downSlot.slotType == slotType)
                    {
                        SlotController.Instance.VerticalSlots.Add(downSlot);
                        string[] derictionDown = { "Down" };
                        downSlot.CheckDirectionSlot(derictionDown);
                    }
                    break;
            }
        }
    }
}
