using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{
    public float damagePerSecond = 5f;
    private SpriteRenderer sr;
    private Collider2D col;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    public void Initialize(Vector2 dir, float duration, float fadeTime)
    {
        transform.right = dir;
        StartCoroutine(HandleLaser(duration, fadeTime));
    }

    IEnumerator HandleLaser(float duration, float fadeTime)
    {
        float t = 0f;
        Color c = sr.color;

        // Fade In
        while (t < fadeTime)
        {
            float alpha = t / fadeTime;
            sr.color = new Color(c.r, c.g, c.b, alpha);
            t += Time.deltaTime;
            yield return null;
        }

        sr.color = new Color(c.r, c.g, c.b, 1f);
        col.enabled = true;

        yield return new WaitForSeconds(duration);

        // Fade Out
        t = 0f;
        while (t < fadeTime)
        {
            float alpha = 1f - t / fadeTime;
            sr.color = new Color(c.r, c.g, c.b, alpha);
            t += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {


                enemy.TakeDamage(Mathf.CeilToInt(damagePerSecond * Time.deltaTime));
                //초당 틱 데미지로 적을 공격하는데, enemy.TakeDamage((int)(damagePerSecond * Time.deltaTime));
                //이전 저 코드는 소수점을 버림 처리해서 계속 0데미지를 주는 방법, 수정된 코드는 소수점을 올림처림함
            }
        }
    }
}
