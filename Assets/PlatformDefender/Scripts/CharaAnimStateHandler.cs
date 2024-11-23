using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaAnimStateHandler : MonoBehaviour
{
    private CharaController m_charaController;

    private void Awake()
    {
        m_charaController = GetComponentInParent<CharaController>();
    }

    public void SetIsAttacking()
    {
        m_charaController.CharaAttack.SetIsAttacking(true);
    }

    public void UnsetIsAttacking()
    {
        m_charaController.CharaAttack.SetIsAttacking(false);
    }
}
