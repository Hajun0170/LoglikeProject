using UnityEngine;

public class Boss : Poolable
{
    public int hp = 300;

    public override void OnReuse()
    {
        hp = 300;
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            gameObject.SetActive(false);
            GameManager.Instance.spawnManager.OnBossDefeated();
        }
    }
}
