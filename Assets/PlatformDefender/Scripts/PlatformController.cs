using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private float m_upSpeed = 5;
    [SerializeField] private float m_minYPos = -20;

    private float m_maxYPos = 0;
    private float m_currentMovingSpeed = 0;
    private List<EnemyController> m_currentEnemiesInContact = new List<EnemyController>();

    private void Start()
    {
        m_maxYPos= transform.position.y;
    }

    private void Update()
    {
        m_currentMovingSpeed = GetCurrentSpeed(GetTotalWeight());
        if (m_currentMovingSpeed != 0)
        {
            transform.position += Vector3.up * m_currentMovingSpeed * Time.deltaTime;

            float yPos = Mathf.Clamp(transform.position.y, m_minYPos, m_maxYPos);
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        }    
    }

    public void AddEnemyInContact(EnemyController enemyController)
    {
        if (!m_currentEnemiesInContact.Contains(enemyController))
        {
            m_currentEnemiesInContact.Add(enemyController);
        }
    }

    public void RemoveEnemyInContact(EnemyController enemyController)
    {
        if (m_currentEnemiesInContact.Contains(enemyController))
        {
            m_currentEnemiesInContact.Remove(enemyController);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemyController = collision.gameObject.GetComponentInParent<EnemyController>();
        if (enemyController)
        {
            AddEnemyInContact(enemyController);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyController enemyController = collision.gameObject.GetComponentInParent<EnemyController>();
        if (enemyController)
        {
            RemoveEnemyInContact(enemyController);
        }
    }

    public float GetTotalWeight()
    {
        float totalWeight = 0;
        m_currentEnemiesInContact.ForEach(x => totalWeight += x.Weight);
        return totalWeight;
    }

    public float GetCurrentSpeed(float totalWeight)
    {
        if(totalWeight == 0)
        {
            return m_upSpeed;
        }
        else
        {
            return -totalWeight;
        }
    }
}
