using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    Camera m_Camera;
    bool m_IsPaused;
    bool m_CanHandleTouch;

	// Use this for initialization
	void Start () {
        if (m_Camera == null)
            m_Camera = Camera.main;

        m_CanHandleTouch = true;

		DelegateManager.OnLoadGameData ();

		DelegateManager.StartGame ();

		//DelegateManager.DisplayQuests ();
	}

    void OnEnable ()
    {
        DelegateManager.CanHandleTouch += CanHandleTouch;
    }

    void OnDisable()
    {
        DelegateManager.CanHandleTouch -= CanHandleTouch;
    }

    void OnApplicationPause (bool pausedStatus)
    {
        if (DelegateManager.BroadcastGameStatus != null)
            DelegateManager.BroadcastGameStatus(pausedStatus);
        m_IsPaused = pausedStatus;
    }

    void OnApplicationFocus (bool hasFocus)
    {
        if (DelegateManager.BroadcastGameStatus != null)
            DelegateManager.BroadcastGameStatus(hasFocus);
        m_IsPaused = !hasFocus;
    }

    void CanHandleTouch (bool isActive)
    {
        m_CanHandleTouch = isActive;
    }

    void Update()
    {
        if (m_IsPaused)
            return;
#if UNITY_ANDROID || UNITY_IOS
		///<summary>
		/// Handle touch first touch
		/// </summary>
		if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Began))
		{
            if (m_CanHandleTouch)
            {
			    var input = Input.GetTouch(0).position;

			    if (Constant.IsDebug)
				    Debug.Log ("Touch Input: " + input);

                var worldPoint = m_Camera.ScreenToWorldPoint(input);
                worldPoint.z = 0;
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, m_Camera.transform.forward, /*Mathf.Infinity*/100f, 1 << LayerMask.NameToLayer(Constant.LAYER_BASE_OBJECT));
                if (hit)
                {
                    GameObject obj = hit.collider.gameObject;
                    DelegateManager.TouchBegin(obj, worldPoint);
                }
            }
		}

		///<summary>
		/// Handle touch with drag
		/// </summary>
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
            var worldPoint = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            worldPoint.z = 0;
            DelegateManager.TouchMove(worldPoint);
		}

		///<summary>
		/// Handle touch end
		/// </summary>
		if (Input.touchCount > 0 && (Input.GetTouch (0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended)) {
			
			if (Constant.IsDebug)
				Debug.Log("Touch phase: " + " Touch end");

            DelegateManager.TouchEnd();
		}
#endif

        DelegateManager.UpdateGame(Time.deltaTime);
    }

}
