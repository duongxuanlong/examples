using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodListController : MonoBehaviour
{

    #region Food Prefabs
    public List<GameObject> m_FoodPrefabs;
    #endregion

    #region Food Instances
    Dictionary<FoodController.FoodType, List<FoodController>> m_Foods;
    #endregion

    #region Gameplay field
    
    #endregion

    Transform m_CachedTransform;

    void OnEnable ()
    {
        DelegateManager.GetFood += GetFood;
        DelegateManager.GetRandomFoodType += GetRandomFoodType;
    }

    void OnDisable ()
    {
        DelegateManager.GetFood -= GetFood;
        DelegateManager.GetRandomFoodType -= GetRandomFoodType;
    }

    void Awake ()
    {
        if (m_CachedTransform == null)
            m_CachedTransform = gameObject.transform;

        int neededObjects = 3;

        if (m_Foods == null)
        {
            m_Foods = new Dictionary<FoodController.FoodType, List<FoodController>>();

            foreach (var prefab in m_FoodPrefabs)
            {
                List<FoodController> foods = new List<FoodController>();
                for (int i = 0; i < neededObjects; ++i)
                {
                    GameObject obj = Instantiate(prefab) as GameObject;
                    obj.SetActive(false);
                    obj.transform.SetParent(m_CachedTransform);
                    FoodController food = obj.GetComponent<FoodController>();
                    foods.Add(food);
                }
                m_Foods[foods[0].CurrentFoodType] = foods;
            }
        }
    }

    public FoodController GetFood (FoodController.FoodType type)
    {
        List<FoodController> foods = m_Foods[type];
        FoodController result = null;
        foreach (var food in foods)
            if (!food.IsUsed)
            {
                result = food;
                break;
            }

        if (result == null)
        {
            GameObject sample = foods[0].gameObject;
            GameObject obj = Instantiate(sample) as GameObject;
            obj.SetActive(false);
            obj.transform.SetParent(m_CachedTransform);
            result = obj.GetComponent<FoodController>();
            m_Foods[type].Add(result);
        }

        result.IsUsed = true;
        return result;
    }

    FoodController.FoodType GetRandomFoodType ()
    {
        int minValue = (int)FoodController.FoodType.Food01;
        int maxValue = ((int)FoodController.FoodType.FoodMax) - 1;

        int randomValue = DelegateManager.GetRandomLevel(minValue, maxValue);
        return (FoodController.FoodType)randomValue;
    }

}
