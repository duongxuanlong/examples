using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetList : MonoBehaviour
{

    #region Prefabs
    public List<GameObject> m_PrefabPets;
    public GameObject m_PrefabPet;
    #endregion

    #region reference variables
    public RectTransform m_TargetCanvas;
    #endregion

    List<PetObject> m_Pets;

    void Awake()
    {
        if (m_Pets == null)
            m_Pets = new List<PetObject>();
    }

    void OnLoadPet ()
    {
        #region Fake loading
        if (m_Pets.Count == 0)
        {
            for (int i = 0; i < Constant.PETS; ++i)
            {
                GameObject obj = Instantiate(m_PrefabPets[i]) as GameObject;
                if (obj != null)
                {
                    obj.transform.SetParent(gameObject.transform);
                    GameObject newobj = Instantiate(m_PrefabPet) as GameObject;
                    newobj.transform.SetParent(gameObject.transform);
                    PetObject pet = newobj.GetComponent<PetObject>();
                    PetStat stat = new PetStat();

                    if (i == 0)
                        stat.m_PetType = PetIconController.PetType.Pet01;
                    else if (i == 1)
                        stat.m_PetType = PetIconController.PetType.Pet02;
                    else if (i == 2)
                        stat.m_PetType = PetIconController.PetType.Pet03;

                    PetController controller = obj.GetComponent<PetController>();
                    pet.SetUpPet(stat, controller);
                    m_Pets.Add(pet);
                }
            }
        }

        DelegateManager.UpdateMaxLevel(FindMaxLevel());

        if (Constant.IsDebug)
            Debug.Log("Pets: " + m_Pets.Count);
        #endregion

    }

    #region Fake methods
    int FindMaxLevel ()
    {
        int level = 0;
        foreach (var pet in m_Pets)
            if (pet.PetStat.m_CurrentLevel > level)
                level = pet.PetStat.m_CurrentLevel;
           
        return level;
    }
    #endregion

    void ArrangePets ()
    {
        float y = (Constant.COLUMN / 2 + Constant.OFFSET_FROM_MATRIX + 0.5f) * Constant.UNIT;
        float startX = -2.0f * Constant.UNIT;
        float offsetX = 2.0f * Constant.UNIT;

        for (int i = 0; i < m_Pets.Count; ++i)
        {
            float x = startX + i * offsetX;
            Vector3 position = new Vector3(x, y, 0.0f);
            m_Pets[i].UpdatePosition(position);
        }
    }

    void PrepareOthersForPet()
    {
        //Set canvas reference
        foreach (var pet in m_Pets)
            pet.SetTargetCanvas(m_TargetCanvas);

        //Prepare for pets
        foreach (var pet in m_Pets)
            pet.PrepareForPet();
    }

    //void PrepareFoodForPets()
    //{
    //    foreach (var pet in m_Pets)
    //    {
    //        pet.PrepareFood();
    //    }
    //}

    //void InstanstiateProgressingBar ()
    //{
    //    foreach (var pet in m_Pets)
    //    {
    //        pet.InstantiateProgressingBar(m_TargetCanvas);
    //    }
    //}

    void StartGame ()
    {
        OnLoadPet();

        ArrangePets();

        PrepareOthersForPet();

        //PrepareFoodForPets();

        //InstanstiateProgressingBar();
    }

    void UpdateGame (float deltaTime)
    {
        foreach (var pet in m_Pets)
            pet.UpdateGame(deltaTime);
    }

    void OnEnable()
    {
        DelegateManager.StartGame += StartGame;
        DelegateManager.UpdateGame += UpdateGame;
    }

    void OnDisable()
    {
        DelegateManager.StartGame -= StartGame;
        DelegateManager.UpdateGame -= UpdateGame;
    }

}
