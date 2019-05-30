using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetObject : MonoBehaviour
{

    #region Gameobject reference
    Transform m_CachedTransform;
    RectTransform m_TargetCanvas;
    #endregion

    #region Pet reference
    PetController m_PetController;
    PetStat m_PetStat;
    #endregion

    #region Food reference
    BaseObject m_FoodType;
    float m_Scale = 0.5f;
    bool m_IsActivePet;
    //float m_Epsilon = 0.2f;
    #endregion


    #region Experience Reference
    //public RectTransform m_Canvas;
    public GameObject m_HealthBarPrefab;
    private HealthBarController m_HealthBarController;
    #endregion

    #region Time Reference
    bool m_ShouldUpdateTime;
    public GameObject m_TimeCounterPrefab;
    private TextController m_TimeController;
    float m_RunningTime;
    string m_TextTime;
    int m_Factor;
    #endregion

    #region Levelup params
    bool m_IsLevelUp;
    #endregion

    void Awake()
    {
        m_CachedTransform = gameObject.transform;

        m_RunningTime = 0f;

        m_ShouldUpdateTime = true;

        m_IsActivePet = false;

        //m_IsDead = false;
    }

    void OnEnable()
    {
        DelegateManager.ShouldUpdatePetExperience += ShouldUpdatePetExperience;
        DelegateManager.UpdateStatWithFactor += UpdateStatWithFactor;
        DelegateManager.ShouldUpdateTime += ShouldUpdateTime;
        DelegateManager.ResetFood += ResetFoodType;
        //DelegateManager.MakeBaseObjectMoveToPet += MoveBaseObjectToPet;
    }

    void OnDisable()
    {
        DelegateManager.ShouldUpdatePetExperience -= ShouldUpdatePetExperience;
        DelegateManager.UpdateStatWithFactor -= UpdateStatWithFactor;
        DelegateManager.ShouldUpdateTime -= ShouldUpdateTime;
        DelegateManager.ResetFood -= ResetFoodType;
        //DelegateManager.MakeBaseObjectMoveToPet -= MoveBaseObjectToPet;
    }


    #region Pet Methods
    public PetStat PetStat
    {
        get { return m_PetStat; }
        set { m_PetStat = value; }
    }

    public void SetUpPet(PetStat stat, PetController controller)
    {
        m_PetStat = stat;
        m_PetController = controller;
    }

    public void UpdatePosition(Vector3 pos)
    {
        m_CachedTransform.position = pos;
        m_PetController.UpdatePosition(pos);
    }

    public void CabllbackForHealthBar ()
    {
        if (m_IsLevelUp)
        {

        }
        else
        {
            ResetFoodType();

            RefreshTime();

            m_IsActivePet = false;

            DelegateManager.ShouldKeepMoving(true);
            DelegateManager.ShouldUpdateTime(true);
            DelegateManager.NotifyOtherObjectState(null, BaseObject.ObjectState.CanMoved);
        }
    }

    void ShouldUpdatePetExperience(int exp, PetIconController.PetType type, FoodController.FoodType foodType, int level, ref BaseObject animatedObj, BaseObject sample)
    {
        //if (m_PetStat.m_IsDead)
        //    return;

        bool shouldContinue = false;
        if (m_PetStat.m_PetType == type && m_PetStat.m_FoodType == foodType)
        {
            if (m_PetStat.m_CurrentLevel == level)
            {
                if (!m_PetStat.m_IsDead)
                {
                    //int combo = DelegateManager.GetCombo();
                    //m_PetStat.m_PregressingLevel += (exp * combo);
                    //RefreshTime();

                    m_Factor = exp;

                    m_IsActivePet = true;
                    animatedObj = sample;

                    //animatedObj.UpdateObjectState(BaseObject.ObjectState.None);
                    animatedObj.SetFoodAndPetIconColor(true);
                    animatedObj.SetTarget(m_CachedTransform.transform.position);
                    animatedObj.UpdateObjectState(BaseObject.ObjectState.MoveWithoutTouch);
                    //StartCoroutine(animatedObj.RunToTarget(m_CachedTransform.position, true, exp));
                    animatedObj.SetUpForwardAnimation(m_CachedTransform.position, true);
                }
            }
            else if (level <= m_PetStat.m_CurrentLevel - 1)
            {
                shouldContinue = true;
            }
        }

        DelegateManager.ShouldContinueToMerge(shouldContinue);
    }

    void UpdateStatWithFactor (int factor)
    {
        if (m_IsActivePet)
        {
            int combo = DelegateManager.GetCombo();
            
            HandlePetXP(m_Factor, combo);

            //GenerateCoins(combo);

            //ResetFoodType();

            //RefreshTime();

            //m_IsActivePet = false;
        }
    }

    void HandlePetXP (int factor, int combo)
    {
        //int amount = factor * combo;
        int temp = factor * combo;
        if (m_PetStat.m_PregressingLevel + temp > m_PetStat.m_PregressingNextLevel)
        {
            m_IsLevelUp = true;
            m_PetStat.m_PregressingLevel = m_PetStat.m_PregressingLevel + temp - m_PetStat.m_PregressingNextLevel;
            temp = m_PetStat.m_PregressingNextLevel;
        }
        else
        {
            m_PetStat.m_PregressingLevel += temp;
            temp = m_PetStat.m_PregressingLevel;
        }
        //m_PetStat.m_PregressingLevel += (factor * combo);

        m_HealthBarController.ChangeValue(temp, m_PetStat.m_PregressingNextLevel, this);
    }

    void GenerateCoins (int combo)
    {
        int index = 0;
        float rate = DelegateManager.GetRandomFloat();
        for (int i = 0; i < m_PetStat.m_CoinRate.Length; ++i)
        {
            if (rate > m_PetStat.m_CoinRate[i])
                rate -= m_PetStat.m_CoinRate[i];
            else
            {
                index = i;
                break;
            }
        }

        DelegateManager.UpdateGameCoins(m_PetStat.m_Coin[index], combo);
    }

    public void HandleLevelUp()
    {

    }
    #endregion

    #region Food methods
    void ResetFoodType ()
    {
        if (m_IsActivePet)
        {
            m_FoodType.UpdateObjectState(BaseObject.ObjectState.Sleeping);
            m_FoodType.UnLinkFoodAndPetIcon();
            PrepareFood();

            //m_IsActivePet = false;
        }
    }

    public void PrepareFood()
    {
        SetUpFood();

        UpdateFoodPosition();

        m_FoodType.SetFoodAndPetIconColor(true);
    }

    void SetUpFood()
    {
        //PetIconController.PetType petType = DelegateManager.GetRandomPetType(m_PetStat.m_CurrentLevel);
        FoodController.FoodType foodType = DelegateManager.GetRandomFoodType();
        m_PetStat.m_FoodType = foodType;
        m_FoodType = DelegateManager.GetRequiredBaseObject(m_PetStat.m_CurrentLevel, m_PetStat.m_PetType, m_PetStat.m_FoodType);
        m_FoodType.UpdateObjectState(BaseObject.ObjectState.Frozen);
        m_FoodType.SetScale(m_Scale * Constant.UNIT);
    }

    void UpdateFoodPosition()
    {
        m_FoodType.FDController.SetFoodScale(m_Scale);
        Vector3 foodOffset = new Vector3(-m_Scale, 0f, 0f);
        m_FoodType.FDController.SetUnitOffset(foodOffset);

        float unit = m_Scale / 2;
        float offsetUnit = 0.25f;
        Vector3 peticonOffset = new Vector3(offsetUnit * Constant.UNIT - m_Scale, -(offsetUnit * Constant.UNIT) + unit, 0f);
        m_FoodType.PIController.SetPetIconScale(unit);
        m_FoodType.PIController.SetUnitOffset(peticonOffset);

        float delta = m_Scale * Constant.UNIT / 2;
        Vector3 cachedPos = m_CachedTransform.position;
        Vector3 pos = new Vector3(cachedPos.x + Constant.OFFSET_FROM_MATRIX * Constant.UNIT - delta, cachedPos.y + Constant.OFFSET_FROM_MATRIX * Constant.UNIT - delta, 0f);
        m_FoodType.UpdatePositionInWorld(pos);
    }
    #endregion

    #region Experience methods
    public void InstantiateProgressingBar(RectTransform targetCanvas)
    {
        GameObject obj = Instantiate(m_HealthBarPrefab) as GameObject;
        m_HealthBarController = obj.GetComponent<HealthBarController>();

        m_HealthBarController.SetFirstUpdate(true);
        m_HealthBarController.SetShuoldUpdate(true);

        m_HealthBarController.SetTargetCanvas(targetCanvas);

        m_HealthBarController.SetFollowObject(m_CachedTransform);

        Vector3 offset = new Vector3(0, -(0.5f * Constant.UNIT + 0.5f), 0f);
        m_HealthBarController.SetUnitOffset(offset);

        m_HealthBarController.SetScreenOffset(Vector3.zero);
    }
    #endregion

    #region Timebar methods
    void InstantiateTimebar(RectTransform targetCanvas)
    {
        GameObject obj = Instantiate(m_TimeCounterPrefab) as GameObject;
        m_TimeController = obj.GetComponent<TextController>();

        m_TimeController.SetFirstUpdate(true);
        m_TimeController.SetShuoldUpdate(true);

        m_TimeController.SetTargetCanvas(targetCanvas);

        m_TimeController.SetFollowObject(m_CachedTransform);

        m_TimeController.SetUnitOffset(new Vector3(-0.5f, 0.5f * Constant.UNIT + 0.5f, 0f));

        m_TimeController.SetScreenOffset(Vector3.zero);
    }

    void RefreshTime()
    {
        m_RunningTime = 0;
    }

    int CalculateRemaining(float delta)
    {
        m_RunningTime += delta;
        int currentTime = (int)m_RunningTime;
        int leftTime = PetStat.m_RefreshTime - currentTime;
        return leftTime;
    }

    void UpdateTimeRemain(float delta)
    {
        if (m_PetStat.m_IsDead)
        {
            m_ShouldUpdateTime = false;
            return;
        }

        int leftTime = CalculateRemaining(delta);
        int leftMinutes = 0, leftSeconds = 0;

        if (leftTime <= 0)
        {
            m_PetStat.m_IsDead = true;
            DelegateManager.UpdateDeadPet(1); //Notify that one pet has died
        }
        else
        {
            leftMinutes = leftTime / 60;
            leftSeconds = leftTime - leftMinutes * 60;
        }

        m_TextTime = leftMinutes.ToString("00") + ":" + leftSeconds.ToString("00");
        m_TimeController.SetText(m_TextTime);
    }

    void ShouldUpdateTime(bool updated)
    {
        m_ShouldUpdateTime = updated;
    }
    #endregion

    public void PrepareForPet()
    {
        //Food
        PrepareFood();

        //Experience progressing
        InstantiateProgressingBar(m_TargetCanvas);

        //Timebar
        InstantiateTimebar(m_TargetCanvas);
    }

    public void SetTargetCanvas(RectTransform canvas)
    {
        m_TargetCanvas = canvas;
    }

    //public void MoveBaseObjectToPet (BaseObject caller)
    //{
    //    if (m_IsActivePet)
    //    {
    //        caller.UpdateObjectState(BaseObject.ObjectState.None);
    //        caller.SetFoodAndPetIconColor(true);
    //        caller.SetUpForwardAnimation(m_CachedTransform.position, true);
    //    }
    //}

    public void UpdateGame(float deltaTime)
    {
        if (m_PetStat.m_IsDead)
            return;

        #region Pet's elements position
        //Update pet position
        m_PetController.UpdatePosition(m_CachedTransform.position);

        //Update food position
        //UpdateFoodPosition();

        //Because timebar is UI's element, so we need to update in LateUpdate
        #endregion

        #region Pet stat
        //m_HealthBarController.ChangeValue(m_PetStat.m_PregressingLevel);

        if (m_ShouldUpdateTime)
            UpdateTimeRemain(deltaTime);
        #endregion
    }





}
