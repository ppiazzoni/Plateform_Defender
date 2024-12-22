using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int m_health = 100;
    [SerializeField] private float m_weight = 5;
    [SerializeField] private bool m_useHitBlink = true;
    [SerializeField] private ParticleSystem m_deathVFX;

    private Rigidbody2D m_rb;
    private SpriteRenderer m_spriteRenderer;
    private bool m_isDying = false;
    private bool m_canBeHit = true;

    public float Weight { get => m_weight; set => m_weight = value; }

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void TakeDamage(int damage, Vector2 knockback)
    {
        if (m_isDying) return;
        m_health -= damage;

        if (m_health < 0)
        {
            m_health = 0;
            StartCoroutine(DeathCoroutine());
            knockback *= 3; // Je rends le knockback plus fort si l'ennemi va mourir avec ce coup. Pour augmenter la sensation de puissance sur le dernier coup
            knockback = Vector2.ClampMagnitude(knockback, 100);
        }

        Knockback(knockback);
        if (m_useHitBlink)
            Debug.Log("endive");
            StartCoroutine(ChangeAllSpritesOverrideColorCoroutine(Color.red, 0.5f));

        StartCoroutine(IFrameCoroutine());
    }

    public void Knockback(Vector2 knockback)
    {
        m_rb.AddForce(knockback, ForceMode2D.Impulse);
        m_rb.AddTorque(knockback.sqrMagnitude);
    }


    private IEnumerator ChangeAllSpritesOverrideColorCoroutine(Color endColor, float duration)
    {
        float timer = 0;
        float factor = 20;
        Color color = new Color(endColor.r * factor, endColor.g * factor, endColor.b * factor, endColor.a);
        endColor = color;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            Color newColor = Color.Lerp(Color.black, endColor, timer / duration);
            SetSpriteColor(newColor);
            yield return new WaitForEndOfFrame();
        }
        timer = 0;
        duration *= 0.6f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            Color newColor = Color.Lerp(endColor, Color.black, timer / duration);
            SetSpriteColor(newColor);
            yield return new WaitForEndOfFrame();
        }

        SetSpriteColor(Color.black);
    }

    public void SetSpriteColor(Color newColor)
    {
        m_spriteRenderer.material.SetColor("_OverrideColor", newColor);
    }

    private IEnumerator DeathCoroutine() // L'ennemi ne meurt pas directement quand ses PV tombent à 0. Il prend 0.05 sec avant de mourir, pour qu'on puisse le voir se faire projeter
    {
        m_isDying = true;
        yield return new WaitForSeconds(0.05f);

        if (m_deathVFX != null)
        {
            //m_deathVFX.transform.parent = null;
            //m_deathVFX.transform.up = m_rb.velocity.normalized;
            m_deathVFX.Play();
        }

        Destroy(gameObject);
    }

    private IEnumerator IFrameCoroutine()
    {
        m_canBeHit = false;
        yield return new WaitForSeconds(0.2f);
        m_canBeHit = true;
    }
}
