using UnityEngine;

public class ExpGem : MonoBehaviour
{
    public int expValue = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats player = other.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.AddExp(expValue);
            }

            Destroy(gameObject);
        }
    }
}
