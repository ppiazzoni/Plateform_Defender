using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamagePlayer : MonoBehaviour
{
    public CharaController pHealth;
    public CharaController targetScript;
    public float m_burnKnockback = 80;
    public float m_damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            CharaController charaController = collision.GetComponentInParent<CharaController>();
            if (charaController)
            {                
                pHealth.m_health -= m_damage;
                m_charaController.Rb.AddForce(m_burnKnockback * Vector2.up, ForceMode2D.Impulse);
            }
            
            if (pHealth.m_health < 0)
            {               
                targetScript.OnDeath();
            }
        }
    }
}
