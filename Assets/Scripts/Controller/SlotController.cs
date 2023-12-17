using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlotController : MonoBehaviour
{
    public static SlotController Instance;

    [SerializeField] private Slot slotPrefab;
    [SerializeField] private Transform spawnSlotPoint;

    [HideInInspector] public List<Slot> VerticalSlots = new List<Slot>();
    [HideInInspector] public List<Slot> HorizontalSlots = new List<Slot>();

    public bool CheckAuto { get; set; }

    private void Awake()
    {
        Instance = this;
        CheckAuto = true;
    }

    private void Start()
    {
        AutoCheckSlots();
    }

    private void Update()
    {
        // Add any necessary updates here
    }

    public void DestroySlots(List<Slot> slots)
    {
        foreach (var slot in slots)
        {
            SpawnSlot(slot.transform.position.x, spawnSlotPoint.position.y + (slots.IndexOf(slot) * 0.96f));
            Destroy(slot.gameObject);
        }
    }

    public void SpawnSlot(float xAxis, float yAxis)
    {
        Vector3 spawnPoint = new Vector3(xAxis, yAxis, 0);
        Instantiate(slotPrefab, spawnPoint, Quaternion.identity);
    }

    public IEnumerator CheckAllSlots()
    {
        while (true)
        {
            if (CheckAuto)
            {
                var slots = FindObjectsOfType<Slot>();
                List<Slot> slotList = new List<Slot>(slots);

                foreach (var currentSlot in slotList)
                {
                    VerticalSlots.Clear();
                    HorizontalSlots.Clear();

                    VerticalSlots.Add(currentSlot);
                    HorizontalSlots.Add(currentSlot);

                    if (currentSlot == null)
                    {
                        yield return null;
                        continue;
                    }

                    string[] directions = { "Left", "Right", "Up", "Down" };
                    currentSlot.CheckDirectionSlot(directions);

                    if (VerticalSlots.Count >= 3)
                        DestroySlots(VerticalSlots);
                    else if (HorizontalSlots.Count >= 3)
                        DestroySlots(HorizontalSlots);
                    else
                    {
                        yield return null;
                        continue;
                    }

                    slotList.RemoveAll(slot => slot == null);

                    if (VerticalSlots.Count >= 3 || HorizontalSlots.Count >= 3)
                        yield return new WaitForSeconds(0.2f);
                    else
                        yield return null;
                }
            }

            yield return new WaitForSeconds(2);
        }
    }

    private void AutoCheckSlots()
    {
        StartCoroutine(CheckAllSlots());
    }
}
