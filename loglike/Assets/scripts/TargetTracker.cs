using System.Collections.Generic;
using UnityEngine;

public class TargetTracker : MonoBehaviour
{
    public List<Transform> targetsInRange = new List<Transform>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !targetsInRange.Contains(other.transform))
        {
            targetsInRange.Add(other.transform);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            targetsInRange.Remove(other.transform);
        }
    }

    public Transform GetFirstTarget()
    {
        targetsInRange.RemoveAll(t => t == null); // null 제거
        return targetsInRange.Count > 0 ? targetsInRange[0] : null;
    }
}
