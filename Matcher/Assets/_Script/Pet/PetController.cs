using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController : MonoBehaviour
{

    #region game object reference
    Transform m_CachedTransform;
    #endregion

    void Awake()
    {
        if (m_CachedTransform == null)
            m_CachedTransform = gameObject.transform;
    }

    public void UpdatePosition (Vector3 pos)
    {
        m_CachedTransform.position = pos;    
    }
}
