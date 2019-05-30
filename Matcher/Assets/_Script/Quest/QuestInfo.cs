using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInfo : MonoBehaviour
{
    #region QuestInfo Field
    private int m_ID;
    private bool m_HasDone;
    private bool m_IsUsed;
    //private bool m_OutOfTime;

    private float m_QuestTime;
    private float m_RunningTime;

    public int ID
    {
        get { return m_ID; }
        set { m_ID = value; }
    }

    public bool HasDone
    {
        get { return m_HasDone; }
        set { m_HasDone = value; }
    }

    public bool IsUsed
    {
        get { return m_IsUsed; }
        set { m_IsUsed = value; }
    }

    public float QuestTime
    {
        get { return m_QuestTime; }
        set { m_QuestTime = value; }
    }

    public float RunningTime
    {
        get { return m_RunningTime; }
        set { m_RunningTime = value; }
    }
    #endregion

    #region Reference Field
    public GameObject m_FakeObject;

    BaseObject m_QuestImage;
    public BaseObject QuestImage
    {
        get { return m_QuestImage; }
        set { m_QuestImage = value; }
    }
    #endregion

    #region Constructor
    public QuestInfo()
    {
        m_HasDone = false;
        m_IsUsed = false;
        //m_OutOfTime = false;

        m_ID = -1;
        m_QuestTime = 0f;
        m_RunningTime = 0f;
    }
    #endregion

    public void UpdateQuestTime (float time)
    {
        m_RunningTime += time;
    }

    public bool IsOutOfTime()
    {
        return m_RunningTime >= m_QuestTime;
    }

    public void SetQuestInfo (int id, float time, BaseObject image)
    {
        this.m_ID = id;
        this.m_QuestTime = time;
        this.m_QuestImage = image;
    }

    public void ResetQuestState()
    {
        m_HasDone = false;
        m_IsUsed = false;
    }
}
