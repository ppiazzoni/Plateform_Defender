using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamagePlayer : MonoBehaviour
{   
    public CharaController targetScript;
    public float m_burnKnockback = 80;
    public float m_damage;


    void Awake()
    {
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CharaController player = other.gameObject.GetComponentInParent<CharaController>();
            
            if (player != null)
            {
                player.TakeDamage(m_damage);
                player.Rb.velocity = Vector2.zero;
                player.Rb.AddForce(Vector2.up * m_burnKnockback, ForceMode2D.Impulse);
                Debug.Log("oof");
            }
            else
            {
                Debug.LogError("ha non");               
            }
        }
    }

}