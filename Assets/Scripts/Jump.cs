using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField, Range(5, 120), Tooltip("Force du saut")] private float m_jumpForce = 40;
    [SerializeField, Range(1, 15), Tooltip("Hauteur de saut maximale")] private float m_maxJumpHeight = 8;
    [SerializeField, Range(0.1f, 3), Tooltip("Hauteur de saut maximale")] private float m_maxJumpDuration = 1.5f;

    [SerializeField] private ParticleSystem m_spinVFX;

    private bool m_canJump = true;
    private bool m_isJumping = false;
    private Vector3 m_startJumpPos;

    private bool m_inputJump = false;
    private float m_jumpTime = 0;

    private CharaController m_charaController;
    private Animator m_anim;

    public bool IsJumping { get => m_isJumping; set => m_isJumping = value; }
    public bool InputJump { get => m_inputJump; set => m_inputJump = value; }

    // Start is called before the first frame update
    void Start()
    {
        m_charaController = GetComponent<CharaController>();
        m_anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_inputJump = Input.GetKey(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CanJump())
            {
                JumpUp();
            }
        }
    }

    private void FixedUpdate()
    {
        //if (m_isJumping && !m_charaController.IsFalling && m_inputJump)
        //{
        //    m_charaController.Rb.AddForce(Vector2.up * m_jumpForce * 7, ForceMode2D.Force);
        //}
    }

    private void JumpUp()
    {
        StartCoroutine(JumpCoroutine());
    }

    private IEnumerator JumpCoroutine()
    {
        m_anim.ResetTrigger("Land");
        m_anim.SetTrigger("Jump");
        m_jumpTime = Time.time;

        yield return new WaitForSeconds(0.01f);

        m_startJumpPos = transform.position;
        m_isJumping = true;
        m_charaController.Rb.AddForce(m_jumpForce * Vector2.up, ForceMode2D.Impulse);
        CameraShake.Instance.StartShaking(0.4f, Vector2.down * 0.6f);

        yield return new WaitForSeconds(0.1f);

        if(m_spinVFX)
            m_spinVFX.Play();

        yield return new WaitForSeconds(m_maxJumpDuration);

    }


    private bool CanJump()
    {
        if (!m_charaController.IsGrounded) return false;
        if (m_charaController.CharaAttack.IsAttacking) return false;
        return m_canJump;
    }

    public void CheckForFall()
    {
        if (m_isJumping)
        {
            if (!m_charaController.IsFalling)
            {
                //if (!m_inputJump)
                //{
                //    m_charaController.OnFalling();
                //}
                if (Time.time - m_jumpTime >= m_maxJumpDuration || (Vector2.Distance(transform.position, m_startJumpPos) >= m_maxJumpHeight && transform.position.y > m_startJumpPos.y))
                {
                    m_charaController.OnFalling();
                }
            }
        }
    }
}
