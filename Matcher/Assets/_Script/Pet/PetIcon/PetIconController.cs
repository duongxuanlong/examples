using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetIconController : MonoBehaviour
{

    #region PetType
    public enum PetType
    {
        Pet01,
        Pet02,
        Pet03,
        PetMax
    };
    #endregion

    #region Set Type and level for Prefab
    public PetType CurrentPetType;
    public int CurrentLevel;
    #endregion

    #region Gameplayfield
    bool m_IsUsed;
    public bool IsUsed
    {
        get { return m_IsUsed; }
        set { m_IsUsed = value; }
    }

    Transform m_CachedTransform;
    public Vector3 m_UnitOffset;
    SpriteRenderer m_Renderer;
    Color m_ActiveColor;
    Color m_FrozenColor;
    #endregion

    void Awake ()
    {
        m_IsUsed = false;

        if (m_CachedTransform == null)
            m_CachedTransform = gameObject.transform;

        if (m_Renderer == null)
            m_Renderer = GetComponent<SpriteRenderer>();

        m_ActiveColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        m_FrozenColor = new Color(1.0f, 0.7f, 0.2f, 0.4f);

        m_CachedTransform.localScale = new Vector3(0.5f, 0.5f, 0f);

        float offsetUnit = 0.25f;
        Vector3 peticonOffset = new Vector3(offsetUnit * Constant.UNIT, -(offsetUnit * Constant.UNIT), 0f);
        m_UnitOffset = peticonOffset;
    }

    public void UpdatePosition(Vector3 pos)
    {
        if (m_CachedTransform == null)
            m_CachedTransform = gameObject.transform;
        m_CachedTransform.position = pos + m_UnitOffset;
    }

    public void SetPetIconScale(float scale)
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

    public void ActivatePetIcon(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void SetPetIconColor (bool isActive)
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
