using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotRayCast : MonoBehaviour
{
    [SerializeField] private Vector2 direction;
    [SerializeField] private float length;
    public Slot GetSlot() 
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, direction, length);

        if(hit.collider != null) 
        {
            if(hit.collider.TryGetComponent(out Slot slot))
                return slot;
            else
                return null;
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, direction * length);
    }
}
