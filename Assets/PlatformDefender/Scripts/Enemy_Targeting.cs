using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Enemy_Targeting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private Transform target;
    private NavMeshAgent m_agent;
    [Header("Values")]
    [SerializeField] private float m_speed = 10;
    public float distance  ;
    public float speed = 10;

    [Header("Rotation")]
    [SerializeField] private Transform m_rotationHandler;
    [SerializeField] private float m_rotationSpeed = 25;
    // Start is called before the first frame update
    void Start()
    {
        Launch();
    }

    private void Launch()
    {
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        m_rb.AddForce(randomDir * m_speed, ForceMode2D.Impulse);
    }
    // Update is called once per frame
    void Update()
    {
        m_rotationHandler.Rotate(Vector3.forward * m_rotationSpeed * Time.deltaTime);
        {
            distance = Vector2.Distance(transform.position, target.transform.position);
            Vector2 direction = target.transform.position - transform.position;

            transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position, speed * Time.deltaTime);
        }
    }
}

