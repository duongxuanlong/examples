using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	//private float m_Width = 8f;

	private Camera m_Camera;

	void Awake()
	{
		m_Camera = GetComponent<Camera> ();
		//Debug.Log ("Camera size: " + m_Camera.orthographicSize);
		float width = Screen.width;
		float height = Screen.height;
		m_Camera.orthographicSize = Constant.WIDTH * height / (width * 2);
	}
}
