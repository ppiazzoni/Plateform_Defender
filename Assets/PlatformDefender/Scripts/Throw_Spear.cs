using UnityEngine;

public class Throw_Spear : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Vector3 mousePos;
    [SerializeField] private bool m_canThrow;
    [SerializeField] private float m_timer;
    [SerializeField] private float m_timeBetweenThrows;
    
    [Header("References")]
    
    [SerializeField] private GameObject m_spear;
    [SerializeField] private Transform m_spearTransform;
    

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

  
    void Update()
    {
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        if (!m_canThrow)
        {
            m_timer += Time.deltaTime;
            if(m_timer > m_timeBetweenThrows)
            {
                m_canThrow = true;
                m_timer = 0;
            }
        }

        if (Input.GetMouseButton(0) && m_canThrow)
        {
            m_canThrow = false;
            Instantiate(m_spear, m_spearTransform.position, Quaternion.identity);
        }

    }
}
