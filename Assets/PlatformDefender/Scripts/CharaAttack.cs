using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaAttack : MonoBehaviour
{
    [SerializeField] private float m_attackCooldown = 1f;
    [SerializeField] private TrailRenderer m_swordTrail;

    private Animator m_anim;
    private Rigidbody2D m_rb;
    private bool m_canAttack = true;
    private bool m_isAttacking = false;


    public bool IsAttacking { get => m_isAttacking; set => m_isAttacking = value; }

    // Start is called before the first frame update
    void Awake()
    {
        m_anim = GetComponentInChildren<Animator>();
        m_rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)||Input.GetMouseButtonDown(0)) 
        {                            
            TriggerAttack();
        }
    }

    public void TriggerAttack()
    {
        if (!m_canAttack) return;

        if(m_swordTrail)
            m_swordTrail.Clear(); 

        m_anim.SetTrigger("Attack");
        StartCoroutine(AttackCooldownCoroutine(m_attackCooldown));

        m_rb.AddForce(Vector2.right * m_anim.transform.localScale.x * 5, ForceMode2D.Impulse);// Fait légèrement avancer le perso dans la direction de son attaque
    }

    private IEnumerator AttackCooldownCoroutine(float cooldown)
    {
        m_canAttack = false;
        yield return new WaitForSeconds(cooldown);
        m_canAttack = true;
    }

    public void SetIsAttacking(bool isAttacking)
    {
        m_isAttacking = isAttacking;
    }
}
