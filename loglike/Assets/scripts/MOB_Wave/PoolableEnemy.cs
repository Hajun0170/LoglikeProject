using UnityEngine;
public class PoolableEnemy : Poolable
{
    public int maxHp = 10;
    private int hp;

    public override void OnReuse()
    {
        hp = maxHp;
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            DropExp();
            gameObject.SetActive(false);
        }
    }

    void DropExp()
    {
        PoolManager.Instance.Spawn("ExpGem", transform.position, Quaternion.identity);
    }
}
