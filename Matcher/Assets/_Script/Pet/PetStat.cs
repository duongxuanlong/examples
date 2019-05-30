using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetStat{

	public PetStat ()
    {
        m_CurrentLevel = 1;
        m_PregressingNextLevel = 5;
        m_PregressingLevel = 0;

        m_RefreshTime = 2 * 60;
        m_Gold = 3;
    }

    public PetIconController.PetType m_PetType;
    public FoodController.FoodType m_FoodType;
    public int m_CurrentLevel;

    public int m_PregressingLevel;
    public int m_PregressingNextLevel;

    public int m_RefreshTime;

    public int m_Gold;

    public bool m_IsDead;

    //Coin rate generation
    public float[] m_CoinRate = { 0.7f, 0.2f, 0.1f};
    public int[] m_Coin = { 3, 5, 10 };
    
    //public int m_PetType;
}
