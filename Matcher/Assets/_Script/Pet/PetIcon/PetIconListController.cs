using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetIconListController : MonoBehaviour
{

    #region List Prefab
    public List<GameObject> m_PetIconPrefabsLevel01;
    public List<GameObject> m_PetIconPrefabsLevel02;
    #endregion

    #region Pet Icon Controller
    Dictionary<PetIconController.PetType, List<PetIconController>> m_PetIconLevel01;
    Dictionary<PetIconController.PetType, List<PetIconController>> m_PetIconLevel02;
    #endregion

    #region Gameplay field
    
    #endregion

    Transform m_CachedTransform;

    void OnEnable ()
    {
        DelegateManager.GetPetIcon += GetPetIcon;
        DelegateManager.GetRandomPetType += GetRandomPetType;
    }

    void OnDisable()
    {
        DelegateManager.GetPetIcon -= GetPetIcon;
        DelegateManager.GetRandomPetType -= GetRandomPetType;
    }

    void Awake()
    {
        if (m_CachedTransform == null)
            m_CachedTransform = gameObject.transform;

        int needIcons = 3;

        if (m_PetIconLevel01 == null)
        {
            m_PetIconLevel01 = new Dictionary<PetIconController.PetType,List<PetIconController>>();
            foreach (var prefab in m_PetIconPrefabsLevel01)
            {
                List<PetIconController> controllerList = new List<PetIconController>();
                for (int i = 0; i < needIcons; ++i)
                {
                    GameObject obj = Instantiate(prefab) as GameObject;
                    obj.SetActive(false);
                    obj.transform.SetParent(m_CachedTransform);
                    PetIconController controller = obj.GetComponent<PetIconController>();
                    controllerList.Add(controller);
                }
                m_PetIconLevel01[controllerList[0].CurrentPetType] = controllerList;
            }
        }

        if (m_PetIconLevel02 == null)
        {
            m_PetIconLevel02 = new Dictionary<PetIconController.PetType,List<PetIconController>>();
            foreach (var prefab in m_PetIconPrefabsLevel02)
            {
                List<PetIconController> controllerList = new List<PetIconController>();
                for (int i = 0; i < needIcons; ++i)
                {
                    GameObject obj = Instantiate(prefab) as GameObject;
                    obj.SetActive(false);
                    obj.transform.SetParent(m_CachedTransform);
                    PetIconController controller = obj.GetComponent<PetIconController>();
                    controllerList.Add(controller);
                }
                m_PetIconLevel02[controllerList[0].CurrentPetType] = controllerList;
            }
        }
    }

    PetIconController GetPetIcon (int level, PetIconController.PetType type)
    {
        List<PetIconController> icons = null;
        PetIconController icon = null;

        if (level == 1)
            icons = m_PetIconLevel01[type];
        else if (level == 2)
            icons = m_PetIconLevel02[type];

        foreach (var i in icons)
            if (!i.IsUsed)
            {
                icon = i;
                break;
            }

        if (icon == null)
        {
            GameObject sample = icons[0].gameObject;
            GameObject obj = Instantiate(sample) as GameObject;
            obj.SetActive(false);
            obj.transform.SetParent(m_CachedTransform);
            icon = obj.GetComponent<PetIconController>();
            icons.Add(icon);
        }

        icon.IsUsed = true;
        return icon;
    }

    PetIconController.PetType GetRandomPetType(int level)
    {
        int minValue = (int)PetIconController.PetType.Pet01;
        int maxValue = ((int)PetIconController.PetType.PetMax) - 1;

        int randomValue = DelegateManager.GetRandomLevel(minValue, maxValue);
        return (PetIconController.PetType)randomValue;
    }
}
