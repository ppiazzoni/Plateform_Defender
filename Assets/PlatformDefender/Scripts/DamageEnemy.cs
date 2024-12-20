using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    [SerializeField] private int m_damage = 10;
    [SerializeField] private float m_knockbackForce = 10;

    [Header("VFX")]
    [SerializeField] private ParticleSystem m_hitVFX;

    private CharaController m_CharaController;


    private void Awake()
    {
        m_CharaController = GetComponentInParent<CharaController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemy = collision.GetComponentInParent<EnemyController>();

        if(enemy != null)
        {
            Vector2 dir = (enemy.transform.position - transform.position).normalized;
            Vector2 knockback = dir + Vector2.up * 0.7f; // J'ajoute un boost vers le haut pour le knockback, peu importe la direction du coup
            knockback *= m_knockbackForce;

            enemy.TakeDamage(m_damage, knockback);
            CameraShake.Instance.StartShaking(1, new Vector2(0,0), 0.06f, 0.05f);

            if(m_hitVFX != null)
            {
                m_hitVFX.transform.position = enemy.transform.position;
                m_hitVFX.transform.up = dir; // Le VFX est tourné en direction du coup. Pour que les particles qui partent en jet aillent bien dans la bonne direction
                m_hitVFX.Play();
                CameraShake.Instance.FreezeTime(0.07f, 0.1f);
            }

            //m_CharaController.FreezeAnim(0.15f);
        }
    }
}
