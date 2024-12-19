using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamagePlayer : MonoBehaviour
{   
    public CharaController targetScript;
    public float m_burnKnockback = 80;
    public float m_damage;
    public MenuController menuController;
    


    void Awake()
    {
        //m_rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CharaController player = other.gameObject.GetComponentInParent<CharaController>();
            
            if (player != null)
            {
                player.m_health -= m_damage;
                //m_rb.AddForce(Vector2.up * transform.localScale.x * 5, ForceMode2D.Impulse);
                Debug.Log("oof");

                if (player.m_health <= 0)
                {
                    targetScript.OnDeath();
                    menuController.gameOver();
                }
            }
            else
            {
                Debug.LogError("ha non");               
            }
        }
    }

}