using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CharaController charaController = collision.GetComponentInParent<CharaController>();
            if (charaController)
            {
                charaController.OnDeath();
            }
        }
    }
}
