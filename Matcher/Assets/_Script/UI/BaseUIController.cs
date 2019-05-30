using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUIController : MonoBehaviour {

    //object to follow
    protected Transform m_FollowObject;

    //Target canvas
    protected RectTransform m_TargetCanvas;

    //Local offset - offset between object and UI
    protected Vector3 m_UnitOffset;

    //Screen offset - offset between UI and screen
    protected Vector3 m_ScreenOffset;

    //Camera reference
    protected Camera m_CachedCamera;

    bool m_ShouldUpdate;
    bool m_FirstUpdate;

	public void Awake ()
    {
        m_ScreenOffset = Vector3.zero;
        m_ShouldUpdate = false;
        m_FirstUpdate = false;

        if (m_CachedCamera == null)
            m_CachedCamera = Camera.main;
    }

    public void LateUpdate()
    {
        if (m_FirstUpdate || m_ShouldUpdate)
        {
            //Vector3 worldPoint = m_FollowObject.TransformPoint(m_UnitOffset);

            //Vector3 viewportPoint = m_CachedCamera.WorldToViewportPoint(worldPoint);
            //viewportPoint.z = 0;
            //viewportPoint -= 0.5f * Vector3.one;

            //Rect rect = m_TargetCanvas.rect;
            //viewportPoint.x *= rect.width;
            //viewportPoint.y *= rect.height;

            //transform.position = viewportPoint + m_ScreenOffset;

            //Vector2 viewportPos = m_CachedCamera.WorldToViewportPoint(m_FollowObject.position);
            //Vector2 worldToScreen = new Vector2((viewportPos.x * m_TargetCanvas.sizeDelta.x) - (m_TargetCanvas.sizeDelta.x * 0.5f),
            //                                       (viewportPos.y * m_TargetCanvas.sizeDelta.y) - (m_TargetCanvas.sizeDelta.y * 0.5f));

            //worldToScreen.y = -worldToScreen.y;
            //viewportPos.y -= 0.5f;
            //m_AnchoredPos.position = viewportPos;
            //m_AnchoredPos.anchoredPosition = worldToScreen;
            //transform.position = m_FollowObject.position + m_UnitOffset;

            Vector3 screenPos = m_CachedCamera.WorldToScreenPoint(m_FollowObject.position + m_UnitOffset);
            Vector2 movedPos;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_TargetCanvas, screenPos, m_CachedCamera, out movedPos);
            transform.position = m_TargetCanvas.TransformPoint(movedPos);

            m_FirstUpdate = false;
        }
    }

    public void SetUnitOffset (Vector3 local)
    {
        m_UnitOffset = local;
    }

    public void SetScreenOffset(Vector3 offset)
    {
        m_ScreenOffset = offset;
    }

    public void SetTargetCanvas (RectTransform target)
    {
        m_TargetCanvas = target;
        transform.SetParent(target, false);
    }

    public void SetFollowObject (Transform follow)
    {
        m_FollowObject = follow;
    }

    public void SetShuoldUpdate (bool update)
    {
        m_ShouldUpdate = update;
    }

    public void SetFirstUpdate (bool update)
    {
        m_FirstUpdate = update;
    }
}
