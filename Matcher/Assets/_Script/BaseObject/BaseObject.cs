using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour {
	
    //public enum ObjectType
    //{
    //    Blue,
    //    Green,
    //    Orange,
    //    Pink,
    //    Purple,
    //    Red,
    //    White,
    //    Yellow,
    //    None
    //};

	public enum ObjectState
	{
		CanMoved,
		Moving,
		Frozen,
		Sleeping,
        MoveWithoutTouch,
		None
	};

	#region Object field
    //[SerializeField]
    //int m_Level;
    //[SerializeField]
    //ObjectType m_Type;
	#endregion

	#region Object position
	int m_Row;
	int m_Column;
	Vector3 m_CachedPosition;

    float m_BorderOffset;
    float m_Offset;
	#endregion

	#region Reference

	Camera m_Camera;
	//SpriteRenderer m_Renderer;
	Transform m_Transform;

    PetIconController m_PetIconController;
    public PetIconController PIController
    {
        get { return m_PetIconController; }
        set { m_PetIconController = value; }
    }

    FoodController m_FoodController;
    public FoodController FDController
    {
        get { return m_FoodController; }
        set { m_FoodController = value; }
    }
    #endregion

    #region BaseObject Animation
    bool m_IsRunningToTarget;
    Vector3 m_TargetPosition;
    Vector3 m_Velocity;
    bool m_SelfRemoved;
    //float m_SmoothTime;
    #endregion

    //Flags
	bool m_CanHandleTouch = true;
	//bool m_IsSelected = false;
	bool m_IsInUsed = false;
	bool m_HasChecked = false;
	public ObjectState m_State;

    #region Object Status
    public void SetIsInUsed (bool isUsed)
	{
		m_IsInUsed = isUsed;
	}

    public void ActivateBaseObject (bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void SetScale (float scale)
    {
        Vector3 newScale = new Vector3(scale, scale, 0f);
        m_Transform.localScale = newScale;
    }

	public bool IsInUsed()
	{
		return m_IsInUsed;
	}

	public PetIconController.PetType GetPetObjectType()
	{
        return PIController.CurrentPetType;
	}

    public FoodController.FoodType GetFoodType ()
    {
        return FDController.CurrentFoodType;
    }

    public int GetLevel()
    {
        return PIController.CurrentLevel;
    }

	public ObjectState GetObjectState()
	{
		return m_State;
	}

	public void SetChecked (bool isChecked)
	{
		m_HasChecked = isChecked;
	}

	public bool Haschecked ()
	{
		return m_HasChecked;
	}

	public int GetRow ()
	{
		return m_Row;
	}

	public int GetColumn()
	{
		return m_Column;
	}

    public Transform GetTransformation()
    {
        return m_Transform;
    }

    #endregion

    #region Handle Object
    public void ActivateFoodAndPetIcon (bool isActive)
    {
        if (FDController != null)
            FDController.ActivateFood(isActive);

        if (PIController != null)
            PIController.ActivatePetIcon(isActive);
    }

    public void SetFoodAndPetIconColor (bool isActive)
    {
        if (FDController != null)
            FDController.SetFoodColor(isActive);

        if (PIController != null)
            PIController.SetPetIconColor(isActive);
    }

    public void LinkFoodAndPetIcon (ref FoodController fController, ref PetIconController piController)
    {
        
        if (piController != null)
        {
            PIController = piController;
            float offsetUnit = 0.25f;
            Vector3 peticonOffset = new Vector3(offsetUnit * Constant.UNIT, -(offsetUnit * Constant.UNIT), 0f);
            piController.SetUnitOffset(peticonOffset);
            piController.SetPetIconScale(0.5f);
        }

        if (fController != null)
        {
            FDController = fController;
            Vector3 foodScale = new Vector3(1.0f, 1.0f, 0f);
            FDController.SetFoodScale(1.0f);
            FDController.SetUnitOffset(Vector3.zero);
        }
    }

    public void UnLinkFoodAndPetIcon ()
    {
        ActivateFoodAndPetIcon(false);

        if (PIController != null)
        {
            PIController.IsUsed = false;
            PIController = null;
        }

        if (FDController != null)
        {
            FDController.IsUsed = false;
            FDController = null;
        }
    }

    public void UpdatePositionInMatrix (int row, int column)
	{
		m_Row = row;
		m_Column = column;
	}

	public void UpdatePositionInWorld (Vector3 position)
	{
		m_Transform.position = position;
        if (FDController != null)
            FDController.UpdatePosition(position);
        if (PIController != null)
            PIController.UpdatePosition(position);
	}

	public void UpdateCachedPosition (Vector3 cache)
	{
		m_CachedPosition = cache;
	}

    public Vector3 GetCachedPosition ()
    {
        return m_CachedPosition;
    }

    public void SetUpForwardAnimation (Vector3 target, bool selfRemoved)
    {
        m_IsRunningToTarget = true;
        m_TargetPosition = target;
        m_SelfRemoved = selfRemoved;
    }

	void UpdateObjectStatus (bool isActive, bool isEnable/*, bool isVisible*/)
	{
		gameObject.SetActive (isActive);
		//m_Renderer.enabled = isVisible;
		this.enabled = isEnable;
	}

	public void UpdateObjectState(ObjectState state)
	{
		m_State = state;

		switch (m_State) {
			case ObjectState.None:
				m_CanHandleTouch = false;
				UpdateObjectStatus (true, true);
                //ActivateFoodAndPetIcon(true);
                SetFoodAndPetIconColor(false);
				break;
			//Never enter here
			case ObjectState.Frozen:
				m_CanHandleTouch = false;
				UpdateObjectStatus (true, false);
                //ActivateFoodAndPetIcon(true);
				break;
			//This state is ready for use
			case ObjectState.Sleeping:
				m_CanHandleTouch = false;
				m_IsInUsed = false;
				m_HasChecked = false;
				m_Column = -1;
				m_Row = -1;
				UpdateObjectStatus (false, false);
                //UnLinkFoodAndPetIcon();
				break;
			case ObjectState.CanMoved:
				m_CanHandleTouch = true;
				UpdateObjectStatus (true, true);
                //ActivateFoodAndPetIcon(true);
                SetFoodAndPetIconColor(true);
				break;
			case ObjectState.Moving:
				m_CanHandleTouch = true;
				break;
            case BaseObject.ObjectState.MoveWithoutTouch:
                m_CanHandleTouch = false;
                UpdateObjectStatus(true, true);
                break;
		}
			
	}

	public void ReturnToCachedPosition()
	{
		//m_Transform.position = m_CachedPosition;
        UpdatePositionInWorld(m_CachedPosition);
	}

    #endregion

	void Awake()
	{
		if (m_Camera == null)
			m_Camera = Camera.main;

//		if (m_Renderer == null)
//			m_Renderer = GetComponent<SpriteRenderer> ();

		if (m_Transform == null)
			m_Transform = gameObject.transform;

		m_State = ObjectState.None;

        //Set up border offset
        //m_BorderOffset = (Constant.HALF_ARRAY_WIDTH - Constant.OFFSET) * Constant.UNIT;
        m_BorderOffset = Constant.WIDTH / 2 - (Constant.OFFSET * Constant.UNIT);
        m_Offset = Constant.OFFSET * Constant.UNIT;

        //change object scale to unit
        Vector3 currentScale = m_Transform.localScale;
        float remainder = 1 - currentScale.x;
        float factor = currentScale.x * Constant.UNIT + remainder * (Constant.UNIT - 1);
        Vector3 scale = new Vector3(factor, factor, 0f); ;
        m_Transform.localScale = scale;

        //Moving forward animation params
        m_Velocity = Vector3.zero;
        //m_SelfRemoved = false;
        m_IsRunningToTarget = false;
	}


	void UpdateStateFromNotification(BaseObject mainObj, ObjectState state)
	{
		if (this != mainObj)
			UpdateObjectState (state);
	}

    public void SetTarget (Vector3 target)
    {
        m_TargetPosition = target;
    }

    //public IEnumerator RunToTarget (Vector3 target, bool selfRemoved, int factor)
    //{
    //    m_IsRunningToTarget = true;
    //    //m_TargetPosition = target;
    //    DelegateManager.CanHandleTouch(false);

    //    while (m_Transform.position != m_TargetPosition)
    //    {
    //        Vector3 pos = Vector3.SmoothDamp(m_Transform.position, m_TargetPosition, ref m_Velocity, Constant.SMOOTH_TIME);
    //        UpdatePositionInWorld(pos);
    //        yield return null;
    //    }

    //    m_Velocity = Vector3.zero;
    //    m_IsRunningToTarget = false;

    //    if (selfRemoved)
    //    {
    //        //Handle current and other baseobject
    //        DelegateManager.NotifyOtherObjectState(this, ObjectState.CanMoved);
    //        UpdateObjectState(ObjectState.Sleeping);
    //        UnLinkFoodAndPetIcon();

    //        //Remove this from list
    //        DelegateManager.RemoveObjectFromList(this);

    //        //Update other game stat
    //        DelegateManager.UpdateStatWithFactor(factor);
    //    }
    //    else
    //        DelegateManager.NotifyOtherObjectState(null, ObjectState.CanMoved);

    //    DelegateManager.ShouldUpdateTime(true);
    //    DelegateManager.ShouldKeepMoving(true);
    //    DelegateManager.CanHandleTouch(true);

    //    yield break;

    //}

    void UpdateGame (float deltaTime)
    {
        if (m_IsRunningToTarget)
        {
            if (m_Transform.position == m_TargetPosition)
            {
                m_IsRunningToTarget = false;

                if (m_TargetPosition == m_CachedPosition)
                    DelegateManager.NotifyOtherObjectState(null, ObjectState.CanMoved);
                /*else
                    DelegateManager.NotifyOtherObjectState(this, ObjectState.CanMoved);*/

                m_Velocity = Vector3.zero;
                m_TargetPosition = Vector3.zero;

                /////////////////////Try to change for level uo///////////////////////
                //DelegateManager.ShouldUpdateTime(true);
                //if (m_SelfRemoved)
                //{
                //    m_SelfRemoved = false;
                //    UpdateObjectState(ObjectState.Sleeping);
                //    UnLinkFoodAndPetIcon();
                //    DelegateManager.ResetFood();
                //    

                //    DelegateManager.UpdateStatWithFactor(1);
                //}

                //DelegateManager.ShouldKeepMoving(true);

                if (m_SelfRemoved)
                {
                    m_SelfRemoved = false;
                    UpdateObjectState(ObjectState.Sleeping);
                    UnLinkFoodAndPetIcon();

                    DelegateManager.RemoveObjectFromList(this);
                    DelegateManager.UpdateStatWithFactor(1);
                }
                else
                {
                    DelegateManager.ShouldUpdateTime(true);
                    DelegateManager.ShouldKeepMoving(true);
                }
                ////////////////////////////////////////////////////////////////////////////
            }
            else
            {
                Vector3 pos = Vector3.SmoothDamp(m_Transform.position, m_TargetPosition, ref m_Velocity, Constant.SMOOTH_TIME);
                UpdatePositionInWorld(pos);
            }
        }
    }

    void OnPause (bool pausedStatus)
    {
        if (pausedStatus)
        {
            if (m_State == ObjectState.Moving)
            {
                UpdatePositionInWorld(m_CachedPosition);
                m_State = ObjectState.CanMoved;
                DelegateManager.NotifyOtherObjectState(this, ObjectState.CanMoved);
                DelegateManager.ShouldKeepMoving(true);
            }
        }
    }

    #region Delegate Methods
    void OnEnable()
	{
		DelegateManager.NotifyOtherObjectState += UpdateStateFromNotification;
        DelegateManager.TouchBegin += TouchBegin;
        DelegateManager.TouchMove += TouchMove;
        DelegateManager.TouchEnd += TouchEnd;
        DelegateManager.UpdateGame += UpdateGame;
        DelegateManager.BroadcastGameStatus += OnPause;
	}

	void OnDisable()
	{
		DelegateManager.NotifyOtherObjectState -= UpdateStateFromNotification;
        DelegateManager.TouchBegin -= TouchBegin;
        DelegateManager.TouchMove -= TouchMove;
        DelegateManager.TouchEnd -= TouchEnd;
        DelegateManager.UpdateGame -= UpdateGame;
        DelegateManager.BroadcastGameStatus -= OnPause;
	}

    #endregion

    #if UNITY_EDITOR
    void OnMouseDown()
	{
		if (m_CanHandleTouch && Input.GetMouseButtonDown (0)) {
			var input = Input.mousePosition;

			if (Constant.IsDebug)
				Debug.Log ("Input Position: " + input);

			if (m_State == ObjectState.CanMoved) {
				//var worldPoint = Camera.main.ScreenToWorldPoint (input);
				var worldPoint = m_Camera.ScreenToWorldPoint (input);
				worldPoint.z = 0;

				if (Constant.IsDebug)
					Debug.Log ("world point: " + worldPoint);
				
				RaycastHit2D hit = Physics2D.Raycast (worldPoint, m_Camera.transform.forward);
				if (hit) {
					GameObject obj = hit.collider.gameObject;
					if (obj == this.gameObject) {
						//Debug.Log ("GameObject get touched with position: " + obj.transform.position);
						UpdateObjectState (ObjectState.Moving);
						worldPoint.z = 0;
						//m_Transform.position = worldPoint;
                        UpdatePositionInWorld(worldPoint);
						DelegateManager.NotifyOtherObjectState (this, ObjectState.None);
                        DelegateManager.ShouldKeepMoving(false);
					}
				}
			}
		}
	}

	void OnMouseDrag()
	{
		if (m_CanHandleTouch && m_State == ObjectState.Moving) {
			//var worldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			var worldPoint = m_Camera.ScreenToWorldPoint (Input.mousePosition);
			worldPoint.z = 0;

			//check border drag
            //if (worldPoint.x < -(Constant.HALF_WIDTH - Constant.OFFSET))
            //    worldPoint.x = -(Constant.HALF_WIDTH - Constant.OFFSET);
            //if (worldPoint.x > (Constant.HALF_WIDTH - Constant.OFFSET))
            //    worldPoint.x = Constant.HALF_WIDTH - Constant.OFFSET;
            //if (worldPoint.y < -(m_Camera.orthographicSize - Constant.OFFSET))
            //    worldPoint.y = -(m_Camera.orthographicSize - Constant.OFFSET);
            //if (worldPoint.y > (m_Camera.orthographicSize - Constant.OFFSET))
            //    worldPoint.y = m_Camera.orthographicSize - Constant.OFFSET;
            if (worldPoint.x < -m_BorderOffset)
                worldPoint.x = -m_BorderOffset;
            if (worldPoint.x > m_BorderOffset)
                worldPoint.x = m_BorderOffset;
            if (worldPoint.y < -(m_Camera.orthographicSize - m_Offset))
                worldPoint.y = -(m_Camera.orthographicSize - m_Offset);
            if (worldPoint.y > (m_Camera.orthographicSize - m_Offset))
                worldPoint.y = m_Camera.orthographicSize - m_Offset;
			
			//m_Transform.position = worldPoint;
            UpdatePositionInWorld(worldPoint);
		}
	}

	void OnMouseUp()
	{
		if (m_State == ObjectState.Moving && m_CanHandleTouch) {
			DelegateManager.NotifyBaseObjectDeselect (this);
            if (!m_IsRunningToTarget)
                DelegateManager.ShouldKeepMoving(true);
//			m_IsSelected = false;
//			DelegateManager.TouchNotification (true);
		}
	}
	#endif

    #region Touch Handler
    void TouchBegin(GameObject obj, Vector3 pos)
    {
        if (m_State == ObjectState.CanMoved && m_CanHandleTouch && this.gameObject == obj)
        {
            UpdateObjectState(ObjectState.Moving);
            //m_Transform.position = pos;
            UpdatePositionInWorld(pos);
            DelegateManager.NotifyOtherObjectState(this, ObjectState.None);
            DelegateManager.ShouldKeepMoving(false);
        }
    }

    void TouchMove(Vector3 worldPoint)
    {
        if (m_State == ObjectState.Moving && m_CanHandleTouch)
        {
            //check border drag
            //if (worldPoint.x < -(Constant.HALF_WIDTH - Constant.OFFSET))
            //    worldPoint.x = -(Constant.HALF_WIDTH - Constant.OFFSET);
            //if (worldPoint.x > (Constant.HALF_WIDTH - Constant.OFFSET))
            //    worldPoint.x = Constant.HALF_WIDTH - Constant.OFFSET;
            //if (worldPoint.y < -(m_Camera.orthographicSize - Constant.OFFSET))
            //    worldPoint.y = -(m_Camera.orthographicSize - Constant.OFFSET);
            //if (worldPoint.y > (m_Camera.orthographicSize - Constant.OFFSET))
            //    worldPoint.y = m_Camera.orthographicSize - Constant.OFFSET;
            if (worldPoint.x < -m_BorderOffset)
                worldPoint.x = -m_BorderOffset;
            if (worldPoint.x > m_BorderOffset)
                worldPoint.x = m_BorderOffset;
            if (worldPoint.y < -(m_Camera.orthographicSize - m_Offset))
                worldPoint.y = -(m_Camera.orthographicSize - m_Offset);
            if (worldPoint.y > (m_Camera.orthographicSize - m_Offset))
                worldPoint.y = m_Camera.orthographicSize - m_Offset;

            //m_Transform.position = worldPoint;
            UpdatePositionInWorld(worldPoint);
        }
    }

    void TouchEnd()
    {
        if (m_State == ObjectState.Moving && m_CanHandleTouch)
        {
            DelegateManager.NotifyBaseObjectDeselect(this);
            if (!m_IsRunningToTarget)
                DelegateManager.ShouldKeepMoving(true);
        }
    }
    #endregion

}
