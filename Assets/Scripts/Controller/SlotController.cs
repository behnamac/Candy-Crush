using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
        
    }
    public void DestroyVericalSlots()
    {
        for (int i = 0; i < VerticalSlots.Count; i++)
        {
            SpawnSlot(VerticalSlots[i].transform.position.x, spawnSlotPoint.position.y + (i * 0.96f));
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
        var slot = Instantiate(slotPrefab, spawnPoint, Quaternion.identity);
    }

    public IEnumerator CheckAllSlots() 
    {
        while (true)
        {
            if (CheckAuto)
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

                    if (slots[i] == null)
                    {
                        yield return null;
                        continue;
                    }

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
                    else
                    {
                        yield return null;
                        continue;
                    }

                    for (int j = 0; j < slotList.Count; j++)
                    {
                        if (slotList[j] == null)
                            slotList.RemoveAt(j);
                    }

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
