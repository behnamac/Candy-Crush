using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlotController : MonoBehaviour
{
    public static SlotController Instance;

    [SerializeField] private Slot slotPrefab;
    [SerializeField] private Transform spawnSlotPoint;
    [SerializeField] private Transform canvas;

    [HideInInspector] public List<Slot> VerticalSlots = new List<Slot>();
    [HideInInspector] public List<Slot> HorizontalSlots = new List<Slot>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(CheckAllSlotsCo());
    }
    private void Update()
    {
        
    }
    public void DestroyVericalSlots()
    {
        for (int i = 0; i < VerticalSlots.Count; i++)
        {
            SpawnSlot(VerticalSlots[i].transform.position.x, spawnSlotPoint.position.y + (i * 100));
            Destroy(VerticalSlots[i].gameObject);
        }
    }
    public void DestroyHorizontalSlots()
    {
        for (int i = 0; i < HorizontalSlots.Count; i++)
        {
            SpawnSlot(HorizontalSlots[i].transform.position.x, spawnSlotPoint.position.y);
            Destroy(HorizontalSlots[i].gameObject);
        }
    }

    public void SpawnSlot(float xAxis, float yAxis) 
    {
        Vector3 spawnPoint = new Vector3(xAxis, yAxis, 0);
        var slot = Instantiate(slotPrefab, spawnPoint, Quaternion.identity, canvas);
        slot.transform.localScale = Vector3.one;
    }

    public void CheckAllSlots() 
    {
        var slots = FindObjectsOfType<Slot>();
        List<Slot> slotList = new List<Slot>();
        slotList.AddRange(slots);
        for (int i = 0; i < slotList.Count; i++)
        {
            VerticalSlots = new List<Slot>();
            HorizontalSlots = new List<Slot>();

            VerticalSlots.Add(slots[i]);
            HorizontalSlots.Add(slots[i]);

            string[] directions = { "Left", "Right", "Up", "Down" };
            slots[i].CheckDirectionSlot(directions);

            if (VerticalSlots.Count >= 3)
            {
                DestroyVericalSlots();
            }
            else if (HorizontalSlots.Count >= 3)
            {
                DestroyHorizontalSlots();
            }

            for (int j = 0; j < slotList.Count; j++)
            {
                if (slotList[j] == null)
                    slotList.RemoveAt(j);
            }
        }
    }

    private IEnumerator CheckAllSlotsCo() 
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            CheckAllSlots();
        }
    }
}
