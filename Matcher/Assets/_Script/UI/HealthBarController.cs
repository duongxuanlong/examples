using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : BaseUIController {

    public Slider m_Slider;
    float m_CurrentAmount;
    float m_Amount;
    float m_DeltaAmount;
    PetObject m_CachedPet;

    void Awake()
    {
        base.Awake();

        if (m_Slider == null)
            m_Slider = GetComponent<Slider>();

        m_DeltaAmount = 0.05f;
    }

    public void ChangeValue (int target, int maxValue, PetObject pet)
    {
        m_Amount = (float)target - m_Slider.value;
        m_CurrentAmount = 0;
        m_CachedPet = pet;
        StartCoroutine(ChangeValueToTarget());
        //m_Slider.
    }

    IEnumerator ChangeValueToTarget ()
    {
        while (m_CurrentAmount < m_Amount)
        {
            m_CurrentAmount += m_DeltaAmount;
            m_Slider.value = m_Slider.value + m_DeltaAmount;
            yield return null;
        }
        m_CurrentAmount = 0;
        m_Amount = 0;
        m_CachedPet.CabllbackForHealthBar();
        m_CachedPet = null;
        
    }

    void LateUpdate()
    {
        base.LateUpdate();
    }


}
