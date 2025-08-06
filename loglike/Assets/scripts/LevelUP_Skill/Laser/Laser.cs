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
                enemy.TakeDamage((int)(damagePerSecond * Time.deltaTime));
            }
        }
    }
}
