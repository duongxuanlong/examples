using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjectList : MonoBehaviour
{
    #region Prefabs
    public GameObject m_BaseObjectPrefab;
    private const int m_NumberObjects = 35;

    public BaseObject[] m_PreLevel_1;
    public BaseObject[] m_PreLevel_2;
    public BaseObject[] m_PreLevel_3;
    #endregion

    #region Object Position param and moving param
    float m_StartX;
    float m_PosY;
    float m_Offset;
    bool m_HasCalculated = false;

    float m_MovingSpeed = 1.5f;
    float m_EdgeRight;
    float m_EdgeLeft;
    BaseObject m_LastObject; //Get reference from last object
    bool m_CanMoved = true;
    #endregion

    /// <summary>
    /// the list is going to provide random objects
    /// </summary>
    //bool m_InitDictionary = false;
    //Dictionary<int, Dictionary<BaseObject.ObjectType, List<BaseObject>>> m_StoringList;
    CenterSpace.Free.MersenneTwister m_Random;

    #region List GameObject
    List<BaseObject> m_BaseObjects; // Pool objects for use
    List<BaseObject> m_ListObjects; // List objects in used
    List<BaseObject> m_RemovedObjects; //List objects need to removed
    #endregion

    Transform m_CachedTransform;

    #region Delegate methods
    void OnEnable()
    {
        DelegateManager.RemoveObjectFromList += RemoveObjectFromList;
        DelegateManager.GetRequiredBaseObject += GetRequiredBaseOjbect;
        DelegateManager.StartGame += StartGame;
        //DelegateManager.GetRandomFoodType += GetRandomFoodType;
        DelegateManager.GetRandomLevel += GetRandomLevel;
        DelegateManager.UpdateGame += UpdateGame;
        DelegateManager.ShouldKeepMoving += ShouldKeepMoving;
        DelegateManager.GetRandomFloat += GetRandomFloat;
    }

    void OnDisable()
    {
        DelegateManager.RemoveObjectFromList -= RemoveObjectFromList;
        DelegateManager.GetRequiredBaseObject -= GetRequiredBaseOjbect;
        DelegateManager.StartGame -= StartGame;
        //DelegateManager.GetRandomFoodType -= GetRandomFoodType;
        DelegateManager.GetRandomLevel -= GetRandomLevel;
        DelegateManager.UpdateGame -= UpdateGame;
        DelegateManager.ShouldKeepMoving -= ShouldKeepMoving;
        DelegateManager.GetRandomFloat -= GetRandomFloat;
    }
    #endregion


    #region Handle BaseObject
    void RemoveObjectFromList(BaseObject obj)
    {
        //if (m_ListObjects.Remove(obj))
        //{
        //    //UnLinkFoodAndPet(obj);
        //    GenerateRandomObject();
        //    //UpdateObjectsPosition();
        //}
        m_RemovedObjects.Add(obj);
    }

    BaseObject GetRequiredBaseOjbect(int level, PetIconController.PetType petType, FoodController.FoodType foodType)
    {
        FoodController fController = DelegateManager.GetFood(foodType);
        fController.IsUsed = true;

        PetIconController piController = DelegateManager.GetPetIcon(level, petType);
        piController.IsUsed = true;

        BaseObject obj = GetObjectInPool();
        obj.ActivateBaseObject(true);
        obj.LinkFoodAndPetIcon(ref fController, ref piController);
        obj.ActivateFoodAndPetIcon(true);
        //obj.FDController = fController;
        //obj.PIController = piController;

        return obj;
    }

    BaseObject GetObjectInPool()
    {
        BaseObject result = null;

        foreach (BaseObject obj in m_BaseObjects)
            if (!obj.IsInUsed())
            {
                result = obj;
                break;
            }


        if (result == null)
        {
            GameObject obj = Instantiate(m_BaseObjectPrefab) as GameObject;
            obj.transform.SetParent(m_CachedTransform);
            result = obj.GetComponent<BaseObject>();
            m_BaseObjects.Add(result);
        }

        result.SetIsInUsed(true);
        return result;
    }

    void UpdateObjectsPosition()
    {
        CalculateParamPos();

        if (Constant.IsDebug)
            Debug.Log("StartX is: " + m_StartX);

        int count = m_ListObjects.Count;
        for (int i = 0; i < count; ++i)
        {
            Vector3 position = new Vector3();
            position.x = m_StartX + i * m_Offset;
            position.y = m_PosY;
            position.z = 0;
            m_ListObjects[i].UpdatePositionInWorld(position);
            m_ListObjects[i].UpdateCachedPosition(position);
        }

        m_LastObject = m_ListObjects[m_ListObjects.Count - 1];
    }

    void GenerateRandomObject(int level = Constant.LEVEL_1)
    {
        FoodController.FoodType foodType = DelegateManager.GetRandomFoodType();
        FoodController fController = DelegateManager.GetFood(foodType);
        fController.IsUsed = true;

        PetIconController.PetType petType = DelegateManager.GetRandomPetType(level);
        PetIconController piController = DelegateManager.GetPetIcon(level, petType);
        piController.IsUsed = true;

        BaseObject controller = GetObjectInPool();
        controller.ActivateBaseObject(true);
        controller.LinkFoodAndPetIcon(ref fController, ref piController);
        controller.ActivateFoodAndPetIcon(true);
        //controller.FDController = fController;
        //controller.PIController = piController;
        controller.UpdateObjectState(BaseObject.ObjectState.CanMoved);
        m_ListObjects.Add(controller);

        //Update position in here now + update last object reference
        Vector3 newPos = new Vector3(m_LastObject.GetTransformation().position.x + m_Offset, m_PosY, 0);
        controller.UpdatePositionInWorld(newPos);
        m_LastObject = controller;

    }

    void CreateListObjects(int level = Constant.LEVEL_1)
    {
        for (int i = 0; i < Constant.NUMBER_OBJECTS; ++i)
        {
            FoodController.FoodType foodType = DelegateManager.GetRandomFoodType();
            FoodController fController = DelegateManager.GetFood(foodType);
            fController.IsUsed = true;

            PetIconController.PetType petType = DelegateManager.GetRandomPetType(level);
            PetIconController piController = DelegateManager.GetPetIcon(level, petType);
            piController.IsUsed = true;

            BaseObject controller = GetObjectInPool();
            //controller.FDController = fController;
            //controller.PIController = piController;
            controller.ActivateBaseObject(true);
            controller.LinkFoodAndPetIcon(ref fController, ref piController);
            controller.ActivateFoodAndPetIcon(true);
            m_ListObjects.Add(controller);
        }

    }

    #endregion

    void ShouldKeepMoving (bool canMoved)
    {
        m_CanMoved = canMoved;
    }

    int GetRandomLevel(int minLevel, int maxLevel)
    {
        int randomValue = m_Random.Next(minLevel, maxLevel);
        return randomValue;
    }

    float GetRandomFloat ()
    {
        return m_Random.NextFloat();
    }

    void Awake()
    {
        if (m_CachedTransform == null)
            m_CachedTransform = gameObject.transform;

        if (m_ListObjects == null)
            m_ListObjects = new List<BaseObject>();


        if (m_BaseObjects == null)
        {
            m_BaseObjects = new List<BaseObject>();
            for (int i = 0; i < m_NumberObjects; ++i)
            {
                GameObject obj = Instantiate(m_BaseObjectPrefab) as GameObject;
                obj.transform.SetParent(m_CachedTransform);
                BaseObject controller = obj.GetComponent<BaseObject>();
                controller.SetIsInUsed(false);
                controller.UpdateObjectState(BaseObject.ObjectState.Sleeping);
                m_BaseObjects.Add(controller);
            }
        }

        if (m_RemovedObjects == null)
            m_RemovedObjects = new List<BaseObject>();

        if (m_Random == null)
            m_Random = new CenterSpace.Free.MersenneTwister();

        m_EdgeLeft = -Constant.WIDTH / 2 - 1;
        m_EdgeRight = Constant.WIDTH / 2 + 1;
    }

    void StartGame()
    {
        CreateListObjects();
        UpdateObjectsPosition();

        //update object state
        foreach (BaseObject obj in m_ListObjects)
            obj.UpdateObjectState(BaseObject.ObjectState.CanMoved);
    }

    void CalculateParamPos()
    {
        if (!m_HasCalculated)
        {
            m_Offset = Constant.UNIT + 0.5f;
            //m_StartX = -(float)System.Math.Round(Constant.HALF_ARRAY_WIDTH * Constant.UNIT, System.MidpointRounding.AwayFromZero);
            m_StartX = 0;
            m_PosY = Constant.UNIT * (-Constant.HALF_ARRAY_HEIGHT - Constant.OFFSET_FROM_MATRIX);
            m_HasCalculated = false;
        }
    }

    void UpdateGame (float time)
    {
        if (m_RemovedObjects.Count > 0)
        {
            foreach (var obj in m_RemovedObjects)
            {
                m_ListObjects.Remove(obj);
                if (obj.GetObjectState() != BaseObject.ObjectState.Frozen)
                {
                    obj.UpdateObjectState(BaseObject.ObjectState.Sleeping);
                    obj.UnLinkFoodAndPetIcon();
                }
            }

            for (int i = 0; i < m_RemovedObjects.Count; ++i)
                GenerateRandomObject();

            m_RemovedObjects.Clear();
        }

        if (m_CanMoved)
        {
            float delta = m_MovingSpeed * time;
            //List<BaseObject> removedList = new List<BaseObject>();

            foreach (var obj in m_ListObjects)
            {
                if (obj.GetTransformation().position.x <= m_EdgeLeft)
                {
                    //m_ListObjects.Remove(obj);
                    //removedList.Add(obj);
                    m_RemovedObjects.Add(obj);
                    continue;
                }

                Vector3 pos = new Vector3(obj.GetTransformation().position.x - delta, obj.GetTransformation().position.y, 0);
                obj.UpdatePositionInWorld(pos);
                obj.UpdateCachedPosition(pos);
            }
        }
        
        
    }




}
