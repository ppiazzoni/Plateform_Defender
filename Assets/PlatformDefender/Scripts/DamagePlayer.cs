using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamagePlayer : MonoBehaviour
{
    public CharaController pHealth;
    public float m_damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CharaController charaController = collision.GetComponentInParent<CharaController>();
            if (charaController)
            {
                pHealth.m_health -= m_damage;                
            }
        }
    }
}
