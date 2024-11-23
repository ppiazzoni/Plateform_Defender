using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float m_spring = 1;
    [SerializeField, Range(0.01f, 0.07f)] private float m_frequency = 0.04f;
    [SerializeField] private float m_drag = 4;

    private bool m_isShaking = false;
    private Vector3 m_targetPos;
    private Vector3 m_basePosition;
    private float m_currentShakingForce;
    private float m_timer = 0;
    private Coroutine m_freezeTimeCoroutine;

    static public CameraShake Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        m_basePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isShaking)
        {
            m_currentShakingForce -= Time.deltaTime * m_drag;
            m_timer += Time.deltaTime;
            if(m_timer >= m_frequency)
            {
                m_targetPos = m_basePosition + (Random.insideUnitSphere * m_currentShakingForce);
                m_timer = 0;
            }

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, m_targetPos, Time.deltaTime * 30);
            if (m_currentShakingForce <= 0.1f)
            {
                StopShaking();
            }
        }
    }

    public void StartShaking(float force, Vector2 dir)
    {    
        m_targetPos = m_basePosition + (Random.insideUnitSphere * m_currentShakingForce);
        m_targetPos += (Vector3)dir;
        m_isShaking = true;
        transform.localPosition = m_targetPos;
        m_currentShakingForce = force * m_spring;
        m_timer = m_frequency;
    }

    public void StartShaking(float force, Vector2 dir, float freezeTimeDuration = 0, float freezeDelay = 0)
    {
        StartShaking(force, dir);
        FreezeTime(freezeTimeDuration, freezeDelay);
    }

    public void FreezeTime(float freezeTimeDuration, float delay = 0)
    {
        m_freezeTimeCoroutine = StartCoroutine(FreezeTmeCoroutine(freezeTimeDuration, delay));
    }

    public void StopShaking()
    {
        m_isShaking = false;
        transform.localPosition = m_basePosition;
    }

    public void ForceStopHitFreeze()
    {
        StopAllCoroutines();
        StopShaking();
    }

    private IEnumerator FreezeTmeCoroutine(float duration, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        float timer = 0;
        Time.timeScale = 0.1f;
        while(timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 1;
    }

    private void OnDestroy()
    {
        if(m_freezeTimeCoroutine != null)
        {
            StopCoroutine(m_freezeTimeCoroutine);
            Time.timeScale = 1;
        }
    }
}
