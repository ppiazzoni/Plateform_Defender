using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CharaController : MonoBehaviour
{
    [Header("Movement")]

    [SerializeField, Range(0, 100), Tooltip("Accélération du personnage lorsqu'on appuie sur la touche de déplacement")] private float m_acceleration = 20;
    [SerializeField, Range(1, 50), Tooltip("Vitesse de déplacement maximale")] private float m_maxMoveSpeed = 5;
    [SerializeField, Range(0, 15), Tooltip("Vitesse à laquelle le personnage s'immobilise après un déplacement")] private float m_movementDrag = 2;
    [SerializeField, Range(0, 10), Tooltip("Vitesse de chute après avoir sauté")] private float m_gravityForce = 1;

    [Header("Visuals")]
    [SerializeField] private Animator m_anim;
    [SerializeField] private ParticleSystem m_landVFX;
    [SerializeField] private ParticleSystem m_deathVFX;
    [SerializeField] private Image m_healthBar;


    [Header("Ground Detection")]
    [SerializeField] [Tooltip("Length of the ground-checking collider")] private float m_groundLength = 0.3f;
    [SerializeField] [Tooltip("Distance between the ground-checking colliders")] private Vector3 m_detectionOffset = new Vector3(0, -0.9f, 0);
    [SerializeField] [Tooltip("Which layers are read as the ground")] private LayerMask m_groundLayer;

    [Header("HealthValues")]
    public float m_health;
    public float m_maxHealth;
    private bool m_isAlive = true;

    private bool m_canDetectGround = true;
    private bool m_isGrounded = true;
    private bool m_isFalling = false;

    private Rigidbody2D m_rb;
    private Collider2D m_collider;

    private CharaAttack m_charaAttack;

    private Vector2 m_rawInput;

    private Jump m_jump;

    private bool m_canMove = true;

    public Rigidbody2D Rb { get => m_rb; set => m_rb = value; }
    public bool IsGrounded { get => m_isGrounded; set => m_isGrounded = value; }
    public bool IsFalling { get => m_isFalling; set => m_isFalling = value; }
    public Vector2 RawInput { get => m_rawInput; set => m_rawInput = value; }
    public CharaAttack CharaAttack { get => m_charaAttack; set => m_charaAttack = value; }
    public bool IsAlive { get => m_isAlive; set => m_isAlive = value; }

    // Start is called before the first frame update
    void Start()
    {
        m_maxHealth = m_health;
        m_rb = GetComponent<Rigidbody2D>();
        m_collider = GetComponentInChildren<Collider2D>();
        m_jump = GetComponent<Jump>();
        m_charaAttack = GetComponent<CharaAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        m_healthBar.fillAmount = Mathf.Clamp(m_health / m_maxHealth, 0, 1);

        m_rawInput.x = 0;
        m_rawInput.y = 0;

        if (CanMove())
        {
            m_rawInput.x = Input.GetAxisRaw("Horizontal");
            m_rawInput.y = Input.GetAxisRaw("Vertical");
        }

        SetDrag();
        CheckIfIsOnGround();
        CheckForFallingState();
        HandleSpriteOrientation();
    }

    private void FixedUpdate()
    {
        if(m_rawInput.x != 0)
        {
            if (Mathf.Abs(m_rb.velocity.x) < m_maxMoveSpeed)
                m_rb.AddForce(Vector2.right * m_rawInput.x * m_acceleration, ForceMode2D.Force);
        }

        if (Mathf.Sign(m_rawInput.x) != Mathf.Sign(m_rb.velocity.x))
            m_rb.velocity *= 0.9f;

        m_rb.velocity = Vector2.ClampMagnitude(m_rb.velocity, 100);

    }

    private void HandleSpriteOrientation()
    {
        if (m_anim)
        {
            if (m_rawInput != Vector2.zero)
            {
                if (m_anim.transform.localScale.x > 0 && m_rawInput.x < 0)
                    m_anim.transform.localScale = new Vector3(-m_anim.transform.localScale.x, m_anim.transform.localScale.y, m_anim.transform.localScale.z);
                else if (m_anim.transform.localScale.x < 0 && m_rawInput.x > 0)
                    m_anim.transform.localScale = new Vector3(-m_anim.transform.localScale.x, m_anim.transform.localScale.y, m_anim.transform.localScale.z);
            }
        }
    }

    void SetDrag()
    {
        if (m_isFalling)
        {
            m_rb.drag = 0;
        }

        else if (IsMoving())
        {
            m_rb.drag = 0;
        }

        else
        {
            m_rb.drag = m_movementDrag;
        }
    }

    private bool IsMoving()
    {
        if (m_rawInput.x != 0)
            return true;
            return false;
    }
    private IEnumerator GroundDetectionDelayCoroutine()
    {
        m_canDetectGround = false;
        yield return new WaitForSeconds(0.1f);
        m_canDetectGround = true;
    }

    private void CheckIfIsOnGround()
    {
        if (!CanDetectGround()) return;

        bool isOnGround = Physics2D.Raycast(transform.position + m_detectionOffset, Vector2.down, m_groundLength, m_groundLayer);

        if (isOnGround != m_isGrounded)
        {
            SetIsOnGround(isOnGround);

            if (isOnGround)
            {
                OnLanding();
            }

            else
            {
                StartCoroutine(GroundDetectionDelayCoroutine());
            }
        }
    }

    private void SetIsOnGround(bool value)
    {
        m_isGrounded = value;
    }

    private bool CanDetectGround()
    {
        return m_canDetectGround;
    }

    private void OnLanding()
    {
        m_isFalling = false;
        if (m_jump)
        {
            if (m_jump.IsJumping)
            {
                m_jump.IsJumping = false;
                m_anim.SetTrigger("Land");
                CameraShake.Instance.StartShaking(1.2f, Vector2.up * 3, 0.07f, 0.05f);
                if (m_landVFX)
                    m_landVFX.Play();
            }
        }

        m_rb.gravityScale = 0.2f;
    }

    public void OnFalling()
    {
        m_isFalling = true;
        m_rb.gravityScale = m_gravityForce;
        m_rb.velocity = new Vector2(m_rb.velocity.x, m_rb.velocity.y * 0.5f);
    }

    private void CheckForFallingState()
    {
        if (m_jump)
        {
             if (m_jump.IsJumping)
                m_jump.CheckForFall();

            else if (!m_jump.IsJumping && !m_isFalling && !m_isGrounded)
            {
                OnFalling();
            }
        }

        else
        {
            if (!m_isFalling && !m_isGrounded)
            {
                OnFalling();
            }
        }
    }

    public void SetDefaultGravityScale()
    {
        if (m_isGrounded)
            m_rb.gravityScale = 1;
        else
            m_rb.gravityScale = m_gravityForce;
    }

    public bool CanMove()
    {
        if (m_charaAttack.IsAttacking) return false;

        return m_canMove;
    }

    public void FreezeAnim(float duration)
    {
        StartCoroutine(FreezeAnimCoroutine(duration));
    }

    private IEnumerator FreezeAnimCoroutine(float duration)
    {
        m_anim.speed = 0;
        yield return new WaitForSeconds(duration);
        m_anim.speed = 1;
    }

    public void OnDeath()
    {
        if (m_isAlive)
        {
            m_isAlive = false;           
            CameraShake.Instance.StartShaking(2.5f, Vector2.up * 0.6f);
            CameraShake.Instance.FreezeTime(0.1f, 0.05f);
            if (m_deathVFX)
            {
                m_deathVFX.transform.parent = null;
                m_deathVFX.Play();
            }
            Destroy(gameObject);
            ReloadScene();
        }
    }

    private void OnDrawGizmos()
    {
        //Draw the ground colliders on screen for debug purposes
        if (m_isGrounded) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        Gizmos.DrawLine(transform.position + m_detectionOffset, transform.position + m_detectionOffset + Vector3.down * m_groundLength);
    }

    public IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(2);
       
        Scene currentScene = SceneManager.GetActiveScene();       
        SceneManager.LoadScene(currentScene.name);
    }
}
