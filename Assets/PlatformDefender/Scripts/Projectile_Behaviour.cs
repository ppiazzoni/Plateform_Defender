using UnityEngine;

public class Projectile_Behaviour : MonoBehaviour
{
    private Vector3 m_mousePos;
    private Camera m_mainCam;
    private Rigidbody2D m_rb;
    public float m_force;

    void Start()
    {
        m_mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        m_rb = GetComponent<Rigidbody2D>();
        m_mousePos = m_mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = m_mousePos - transform.position;
        Vector3 rotation = transform.position - m_mousePos;
        m_rb.velocity = new Vector2(direction.x, direction.y).normalized * m_force;
        float rot = Mathf.Atan2(rotation.x, rotation.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }
}
