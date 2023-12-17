using DG.Tweening;
using GG.Infrastructure.Utils.Swipe;
using System.Collections.Generic;
using UnityEngine;

public enum SlotType { Type1, Type2, Type3, Type4, Type5 }

public class Slot : MonoBehaviour
{
    public SlotType slotType;

    [SerializeField] private SlotRayCast slotLeftRay;
    [SerializeField] private SlotRayCast slotRightRay;
    [SerializeField] private SlotRayCast slotUpRay;
    [SerializeField] private SlotRayCast slotDownRay;

    [SerializeField] private bool randomType;
    [SerializeField] private SpriteRenderer iconImage;

    private bool _touch;

    private void Start()
    {
        SwipeListener.Instance.OnSwipe.AddListener(CheckSides);

        if (randomType)
            SetRandomType();
    }

    private void OnValidate()
    {
        if (!iconImage) return;

        SetIconColorByType();
    }

    private void OnDestroy()
    {
        // Handle OnDestroy logic if needed
    }

    private void OnMouseDown()
    {
        _touch = true;
        SlotController.Instance.CheckAuto = false;
    }

    private void OnMouseUp()
    {
        _touch = false;
    }

    private void CheckSides(string directionName)
    {
        if (!_touch) return;

        switch (directionName)
        {
            case "Left":
                Move(slotLeftRay.GetSlot());
                break;
            case "Right":
                Move(slotRightRay.GetSlot());
                break;
            case "Up":
                Move(slotUpRay.GetSlot());
                break;
            case "Down":
                Move(slotDownRay.GetSlot());
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
            SlotController.Instance.CheckAuto = true;

            SlotController.Instance.VerticalSlots.Add(this);
            SlotController.Instance.HorizontalSlots.Add(this);

            string[] directions = { "Left", "Right", "Up", "Down" };
            CheckDirectionSlot(directions);

            if (SlotController.Instance.VerticalSlots.Count >= 3)
            {
                SlotController.Instance.DestroySlots(SlotController.Instance.VerticalSlots);
            }
            else if (SlotController.Instance.HorizontalSlots.Count >= 3)
            {
                SlotController.Instance.DestroySlots(SlotController.Instance.HorizontalSlots);
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
        SetIconColorByType();
    }

    private void SetIconColorByType()
    {
        switch (slotType)
        {
            case SlotType.Type1:
                iconImage.color = Color.yellow;
                break;
            case SlotType.Type2:
                iconImage.color = Color.cyan;
                break;
            case SlotType.Type3:
                iconImage.color = Color.green;
                break;
            case SlotType.Type4:
                iconImage.color = Color.blue;
                break;
            case SlotType.Type5:
                iconImage.color = Color.red;
                break;
        }
    }

    public void CheckDirectionSlot(string[] directions)
    {
        foreach (var dir in directions)
        {
            var adjacentSlot = GetAdjacentSlot(dir);
            if (adjacentSlot && adjacentSlot.slotType == slotType)
            {
                if (dir == "Left" || dir == "Right")
                    SlotController.Instance.HorizontalSlots.Add(adjacentSlot);
                else
                    SlotController.Instance.VerticalSlots.Add(adjacentSlot);

                adjacentSlot.CheckDirectionSlot(new[] { dir });
            }
        }
    }

    private Slot GetAdjacentSlot(string direction)
    {
        switch (direction)
        {
            case "Left":
                return slotLeftRay.GetSlot();
            case "Right":
                return slotRightRay.GetSlot();
            case "Up":
                return slotUpRay.GetSlot();
            case "Down":
                return slotDownRay.GetSlot();
            default:
                return null;
        }
    }
}
