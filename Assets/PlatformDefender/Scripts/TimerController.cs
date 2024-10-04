using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_timerText;
    [SerializeField] private CharaController m_charaController;

    private float m_currentTimer = 0;

    public float CurrentTimer { get => m_currentTimer; set => m_currentTimer = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsGameOver())
        {
            m_currentTimer += Time.deltaTime;
            float minutes = Mathf.FloorToInt(m_currentTimer / 60);
            float seconds = Mathf.FloorToInt(m_currentTimer % 60);

            m_timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

    }

    public bool IsGameOver()
    {
        if (!m_charaController) return true;
        if (!m_charaController.IsAlive) return true;
        return false;
    }
}
