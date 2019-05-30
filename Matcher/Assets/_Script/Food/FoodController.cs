using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{

    #region Enum FoodType
    public enum FoodType
    {
        Food01,
        Food02,
        Food03,
        Food04,
        FoodMax
    };
    #endregion

    #region Set Type for prefab
    public FoodType CurrentFoodType;
    #endregion

    #region Gameplay Field
    bool m_IsUsed;
    public bool IsUsed
    {
        get { return m_IsUsed; }
        set { m_IsUsed = value; }
    }

    Transform m_CachedTransform;
    Vector3 m_UnitOffset;
    SpriteRenderer m_Renderer;
    Color m_ActiveColor;
    Color m_FrozenColor;
    #endregion

    void Awake()
    {
        m_IsUsed = false;

        if (m_CachedTransform == null)
            m_CachedTransform = gameObject.transform;

        if (m_Renderer == null)
            m_Renderer = GetComponent<SpriteRenderer>();

        m_ActiveColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        m_FrozenColor = new Color(1.0f, 0.7f, 0.2f, 0.4f);

        m_UnitOffset = Vector3.zero;
    }

    public void UpdatePosition (Vector3 pos)
    {
        if (m_CachedTransform == null)
            m_CachedTransform = gameObject.transform;
        m_CachedTransform.position = pos + m_UnitOffset;
    }

    public void SetFoodScale(float scale)
    {
        Vector3 newScale = new Vector3(scale, scale, 0f);

        if (m_CachedTransform == null)
            m_CachedTransform = gameObject.transform;
        m_CachedTransform.localScale = newScale;
    }

    public void SetUnitOffset(Vector3 offSet)
    {
        m_UnitOffset = offSet;
    }

    public void ActivateFood (bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void SetFoodColor (bool isActive = true)
    {
        if (m_Renderer == null)
            m_Renderer = GetComponent<SpriteRenderer>();

        if (isActive)
        {
            m_Renderer.color = m_ActiveColor;
        }
        else
        {
            m_Renderer.color = m_FrozenColor;
        }
    }

    

}
