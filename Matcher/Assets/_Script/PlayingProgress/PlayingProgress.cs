using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine;

public class PlayingProgress : MonoBehaviour
{

    #region Game params with: Coins, Energies, XP ranks and combo
    int m_GameCoins;  // for buying items
    int m_GameEnergies; // for play turn
    int m_GameXP;  // for ranking in leaderboard
    int m_Combo; //combo for XP and coins
    #endregion

    #region GamePlay field
    //bool m_IsMapFull;
    int m_DeadPets;
    #endregion

    Gameinfo m_Info;

    int m_MaxLevel;

    void UpdateMaxLevel (int level)
    {
        m_MaxLevel = level;
    }

    int GetMaxLevel ()
    {
        return m_MaxLevel;
    }

    #region Methods for coins, energies, xp ranks and combo
    int GetCoins()
    {
        return m_GameCoins;
    }

    int GetXP()
    { 
        return m_GameXP; 
    }

    int GetEnergies()
    {
        return m_GameEnergies;
    }

    int GetCombo()
    {
        return m_Combo;
    }

    void UpdateGameCoins(int coins, int combo)
    {
        if (combo == 1)
            m_GameCoins = coins;
        else
            m_GameCoins = coins * combo;
    }

    void UpdateEnergies(int energies)
    {
        m_GameEnergies = energies;
    }

    void UpdateGameXP(int xp)
    {
        m_GameXP = xp * m_Combo;
    }

    void UpdateCombo(bool up, int delta = 1)
    {
        if (up)
            m_Combo += delta;
        else
        {
            m_Combo -= delta;
            if (m_Combo < 1)
                m_Combo = 1;
        }
    }
    #endregion

    #region GamePlay field
    void StartGame ()
    {
        m_DeadPets = 0;
        m_Combo = 1;
    }

    void UpdateDeadPet(int number = 1)
    {
        m_DeadPets += number;
        if (m_DeadPets >= Constant.PETS)
            DelegateManager.BroadcastEndGame();

    }
    #endregion

    void OnEnable()
	{
		DelegateManager.OnSaveGameData += OnSaveGameData;
		DelegateManager.OnLoadGameData += OnLoadGameData;
		DelegateManager.UpdateCompleteQuestsInLevel += UpdateCompleteQuests;
		DelegateManager.RetrieveCompleteQuestsInLevel += RetrieveCompleteQuestsInLevel;
        DelegateManager.UpdateMaxLevel += UpdateMaxLevel;
        DelegateManager.GetMaxLevel += GetMaxLevel;
        DelegateManager.UpdateDeadPet += UpdateDeadPet;
        DelegateManager.UpdateCombo += UpdateCombo;
        DelegateManager.GetCombo += GetCombo;
        DelegateManager.UpdateGameCoins += UpdateGameCoins;
        DelegateManager.StartGame += StartGame;
	}

	void OnDisable()
	{
		DelegateManager.OnSaveGameData -= OnSaveGameData;
		DelegateManager.OnLoadGameData -= OnLoadGameData;
		DelegateManager.UpdateCompleteQuestsInLevel -= UpdateCompleteQuests;
		DelegateManager.RetrieveCompleteQuestsInLevel -= RetrieveCompleteQuestsInLevel;
        DelegateManager.UpdateMaxLevel -= UpdateMaxLevel;
        DelegateManager.GetMaxLevel -= GetMaxLevel;
        DelegateManager.UpdateDeadPet -= UpdateDeadPet;
        DelegateManager.UpdateCombo -= UpdateCombo;
        DelegateManager.GetCombo -= GetCombo;
        DelegateManager.UpdateGameCoins -= UpdateGameCoins;
        DelegateManager.StartGame -= StartGame;
	}

	void UpdateCompleteQuests (int level)
	{
		int quests = 0;
		if (m_Info.m_DataProgress.ContainsKey (level))
			quests = m_Info.m_DataProgress [level];
		quests += 1;
		m_Info.m_DataProgress [level] = quests;
	}

	int RetrieveCompleteQuestsInLevel (int level)
	{
		int quests = 0;
		if (m_Info.m_DataProgress.ContainsKey (level))
			quests = m_Info.m_DataProgress [level];
		return quests;
	}

	void OnSaveGameData()
	{
		BinaryFormatter bif = new BinaryFormatter ();
		string path = Application.persistentDataPath + Constant.CONSTANT_NUMBER;
		FileStream f = File.Open (path, FileMode.OpenOrCreate);

		bif.Serialize (f, m_Info);
		f.Close ();
	}

	void OnLoadGameData()
	{
		if (m_Info == null) {
			m_Info = new Gameinfo ();
			m_Info.m_DataProgress = new Dictionary<int, int> ();
		} else
			m_Info.m_DataProgress.Clear ();

		string path = Application.persistentDataPath + Constant.CONSTANT_NUMBER;
		if (File.Exists (path)) {
			FileStream f = File.Open (path, FileMode.Open);
			BinaryFormatter bif = new BinaryFormatter ();
			m_Info = (Gameinfo) bif.Deserialize (f);
			f.Close ();
		}
	}

	[Serializable]
	class Gameinfo
	{
		//first is the level and the second is the number of quets that have been finished
		public Dictionary<int, int> m_DataProgress;
	};
}
