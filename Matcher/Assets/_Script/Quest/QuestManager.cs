using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    #region reference
    public QuestObject m_QOPrefab;
    #endregion
    List<QuestLoader.QuestData> m_RunningQuests;
	List<BaseObject> m_DisplayObjects;
	int m_CompleteQuests;

	void Awake()
	{
		QuestLoader.GetInstance ().LoadQuest ();

		if (m_RunningQuests == null)
			m_RunningQuests = new List<QuestLoader.QuestData> ();

		if (m_DisplayObjects == null)
			m_DisplayObjects = new List<BaseObject> ();

		m_CompleteQuests = 0;
	}

	void StartGame ()
	{
		m_RunningQuests.Clear ();

		//QuestLoader.GetInstance ().LoadQuest ();

		RetriveQuests ();

		//ClearDisplayObjects ();

		//GenerateDisplayObjects ();
	}

	#region Quests
	void RetriveQuests ()
	{
		int level2 = Constant.LEVEL_2;
		int quests2 = DelegateManager.RetrieveCompleteQuestsInLevel (level2);

		int level = level2;
		for (int i = 0; i < Constant.MAX_QUESTS; ++i) {
			if (quests2 >= 5) {
				int level3 = Constant.LEVEL_3;
				level = DelegateManager.GetRandomLevel (level2, level3);
			}

			//QuestLoader.GetInstance ().RetrieveQuests (ref m_RunningQuests, level);
		}
	}

    //void UpdateCompleteQuest (int level, BaseObject.ObjectType type)
    //{
    //    foreach (QuestLoader.QuestData info in m_RunningQuests) {
    //        if (info.Level == level && type == info.Type /*&& !info.HasDone*/) {
				

    //            DisableDisplayQuest (level, type);

    //            m_CompleteQuests++; //track status of the finished quest

    //            DelegateManager.UpdateCompleteQuestsInLevel (level); //Update the number of quests of that level that has been finished

    //            break;
    //        }
    //    }
    //}

	void ShouldRefreshQuests ()
	{
		if (m_CompleteQuests == Constant.MAX_QUESTS) {
			m_RunningQuests.Clear ();

			m_CompleteQuests = 0;

			RetriveQuests ();

			NotifyDisplayObjects ();
		}
	}
	#endregion

	#region Display quests
    //void DisableDisplayQuest (int level, BaseObject.ObjectType type)
    //{
    //    foreach (BaseObject obj in m_DisplayObjects) {
    //        if (obj.GetLevel () == level && obj.GetPetObjectType () == type) {
    //            obj.UpdateObjectState (BaseObject.ObjectState.Sleeping);
    //            break;
    //        }
    //    }	
    //}

	void ClearDisplayObjects ()
	{
        //foreach (QuestLoader.QuestData info in m_RunningQuests) {
        //    BaseObject obj = DelegateManager.GetRequiredBaseObject (info.Level, info.Type);
        //    obj.UpdateObjectState (BaseObject.ObjectState.Sleeping);
        //}
		m_DisplayObjects.Clear ();
	}

	void GenerateDisplayObjects ()
	{
        //foreach (QuestLoader.QuestData info in m_RunningQuests) {
        //    BaseObject obj = DelegateManager.GetRequiredBaseObject (info.Level, info.Type);
        //    obj.UpdateObjectState (BaseObject.ObjectState.Frozen);
        //    m_DisplayObjects.Add (obj);
        //}
	}

	void ArrangeDisplayObjects ()
	{
		float y = Constant.COLUMN / 2 + Constant.OFFSET_FROM_MATRIX;
		float startX = -2.0f;
		float offsetX = 2.0f;
		for (int i = 0; i < m_DisplayObjects.Count; ++i) {
			float x = startX + i * offsetX;
			Vector3 position = new Vector3 (x, y, 0.0f);
			m_DisplayObjects [i].UpdatePositionInWorld (position);
		}
	}

	void NotifyDisplayObjects ()
	{
		ClearDisplayObjects ();

		GenerateDisplayObjects ();

		ArrangeDisplayObjects ();
	}
	#endregion

	void OnEnable()
	{
		DelegateManager.StartGame += StartGame;
		DelegateManager.DisplayQuests += NotifyDisplayObjects;
		//DelegateManager.UpdateCompleteQuest += UpdateCompleteQuest;
		DelegateManager.ShouldRefreshQuests += ShouldRefreshQuests;
	}

	void OnDisable()
	{
		DelegateManager.StartGame -= StartGame;
		DelegateManager.DisplayQuests -= NotifyDisplayObjects;
		//DelegateManager.UpdateCompleteQuest -= UpdateCompleteQuest;
		DelegateManager.ShouldRefreshQuests -= ShouldRefreshQuests;
	}
}
