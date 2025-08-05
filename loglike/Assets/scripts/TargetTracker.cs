using System.Collections.Generic;
using UnityEngine;

public class TargetTracker : MonoBehaviour
{
    public float radius = 5f;                  // 추적 반경
    public LayerMask enemyLayer;              // 적이 있는 레이어만 추적

    private Collider2D[] buffer = new Collider2D[20];

    public Transform GetFirstTarget()
    {
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, radius, buffer, enemyLayer);

        float closestDistance = Mathf.Infinity;
        Transform closest = null;

        for (int i = 0; i < count; i++)
        {
            float dist = Vector2.Distance(transform.position, buffer[i].transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = buffer[i].transform;
            }
        }

        return closest;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}


/*
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
*/