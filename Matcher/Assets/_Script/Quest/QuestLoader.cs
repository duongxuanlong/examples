using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLoader {
	public class QuestData {
		public int ID;
		public int Level;
		//public BaseObject.ObjectType Type;

		public QuestData()
		{
			
		}
	};

	QuestLoader()
	{
		if (m_Quests == null)
			m_Quests = new List<QuestData> ();
	}

	static QuestLoader m_Instance;
	List<QuestData> m_Quests;

	public static QuestLoader GetInstance()
	{
		if (m_Instance == null)
			m_Instance = new QuestLoader ();
		return m_Instance;
	}

	public void LoadQuest (string path = "default")
	{
		if (m_Quests.Count > 0)
			return;
		//Load data from path

		//Currently, use fake data
		int fromLevel = 2;
		int toLevel = 3;
		int index = 0;

		for (int i = fromLevel; i <= toLevel; ++i) 
        {
            //int fromType = (int)BaseObject.ObjectType.Blue;
            //int toType = (int)BaseObject.ObjectType.Yellow;
            //for (int j = fromType; j <= toType; ++j) 
            //{
            //    QuestData info = new QuestData();
            //    info.Level = i;
            //    info.Type = (BaseObject.ObjectType)j;
            //    info.ID = index;
            //    index++;
            //    m_Quests.Add (info);
            //}
		}
	}

	public QuestData RetrieveQuests (/*ref List<QuestData> quests,*/ int level)/*, BaseObject.ObjectType type)*/
	{
		//int count = quests.Count;
        //bool success = false;
        //foreach (var quest in m_Quests) {
			
//			if (count == Constant.MAX_QUESTS)
//				break;
			
            //if (!quest.HasDone &&
            //    quest.Level == level &&
            //    !quest.IsUsed/*&& quest.Type == type*/) {
            //    quest.IsUsed = true;
            //    quests.Add (quest);
            //    success = true;
            //    //count++;
            //    break;
            //}
		//}

		//if (count < Constant.MAX_QUESTS) {
        //if (!success){
        //    RefreshQuests (quests);
        //    RetrieveQuests (ref quests, level/*, type*/);
        //}

        int minValue = 0;
        int maxValue = m_Quests.Count - 1;
        int index = 0;
        do
        {
            index = DelegateManager.GetRandomLevel(minValue, maxValue);
        } while (m_Quests[index].Level != level);

        return m_Quests[index];
	}

	public void RefreshQuests (List<QuestData> quests = null)
	{
		for (int i = 0; i < m_Quests.Count; ++i) {
			if (quests != null) {
				foreach (QuestData quest in quests)
					if (quest.ID != m_Quests [i].ID) {
                        //m_Quests [i].UpdateHasDone(false);
                        //m_Quests [i].UpdateIsUsed (false);
					}
			} else {
                //m_Quests [i].UpdateHasDone (false);
                //m_Quests [i].UpdateIsUsed (false);
			}
		}
	}
}
