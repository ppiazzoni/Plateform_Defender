using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamagePlayer : MonoBehaviour
{   
    public CharaController targetScript;
    public float m_burnKnockback = 80;
    public float m_damage;

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CharaController player = other.gameObject.GetComponentInParent<CharaController>();
            if (player != null)
            {
                player.m_health -= m_damage;
                if (player.m_health <= 0)
                {
                    targetScript.OnDeath();
                }
            }
            else
            {
                Debug.LogError("CharaController not found on Player object!");
            }
        }
    }

}