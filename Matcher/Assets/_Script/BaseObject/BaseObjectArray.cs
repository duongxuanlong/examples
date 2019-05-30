using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjectArray : MonoBehaviour
{
    #region Properties
    BaseObject[,] m_Array;
    List<BaseObject> m_SelectedObjects;
    //	int m_LineCount = 100;
    //	float m_Radius = 3.0f;
    Color m_Color;
    Material m_LineMaterial;
    BaseObject m_AnimatedObject;
    #endregion


    public void OnRenderObject()
    {
        if (m_LineMaterial != null)
        {
            m_LineMaterial.SetPass(0);
            GL.PushMatrix();
            GL.MultMatrix(transform.localToWorldMatrix);

            GL.Begin(GL.LINES);
            //		for (int i = 0; i < m_LineCount; ++i) {
            //			float a = i / (float)m_LineCount;
            //			float angle = a * Mathf.PI * 2;
            //			GL.Color (new Color (a, 1 - a, 0, 0.8f));
            //			GL.Vertex3 (0, 0, 0);
            //			GL.Vertex3 (Mathf.Cos (angle) * m_Radius, Mathf.Sin (angle) * m_Radius, 0);
            //		}
            //Draw horizontal lines
            //for (int i = -Constant.HALF_ARRAY_HEIGHT; i <= Constant.HALF_ARRAY_HEIGHT; ++i) {
            //    GL.Color (new Color (m_Color.r, m_Color.g, m_Color.b, m_Color.a));
            //    GL.Vertex3 (-Constant.HALF_ARRAY_WIDTH, i, 0);
            //    GL.Vertex3 (Constant.HALF_ARRAY_WIDTH, i, 0);
            //}
            for (int i = 0; i <= Constant.COLUMN; ++i)
            {
                GL.Color(new Color(m_Color.r, m_Color.g, m_Color.b, m_Color.a));
                GL.Vertex3(-Constant.HALF_ARRAY_WIDTH * Constant.UNIT, Constant.UNIT * (-Constant.HALF_ARRAY_HEIGHT + i), 0);
                GL.Vertex3(Constant.HALF_ARRAY_WIDTH * Constant.UNIT, Constant.UNIT * (-Constant.HALF_ARRAY_HEIGHT + i), 0);
            }

            //Draw vertical lines
            //for (int i = -Constant.HALF_ARRAY_WIDTH; i <= Constant.HALF_ARRAY_WIDTH; ++i) {
            //    GL.Color (new Color (m_Color.r, m_Color.g, m_Color.b, m_Color.a));
            //    GL.Vertex3 (i, -Constant.HALF_ARRAY_WIDTH, 0);
            //    GL.Vertex3 (i, Constant.HALF_ARRAY_WIDTH, 0);
            //}
            for (int i = 0; i <= Constant.ROW; ++i)
            {
                GL.Color(new Color(m_Color.r, m_Color.g, m_Color.b, m_Color.a));
                GL.Vertex3(Constant.UNIT * (-Constant.HALF_ARRAY_HEIGHT + i), -Constant.HALF_ARRAY_WIDTH * Constant.UNIT, 0);
                GL.Vertex3(Constant.UNIT * (-Constant.HALF_ARRAY_HEIGHT + i), Constant.HALF_ARRAY_WIDTH * Constant.UNIT, 0);
            }
            GL.End();
            GL.PopMatrix();
        }
    }

    void StartGame()
    {
        if (m_LineMaterial == null)
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            m_LineMaterial = new Material(shader);
            m_LineMaterial.hideFlags = HideFlags.HideAndDontSave;

            //Turn on alpha blending
            m_LineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            m_LineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            //Turn backface culling off
            m_LineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            //Turn off depth write
            m_LineMaterial.SetInt("_ZWrite", 0);


            //Calculate color for later use
            float r = 1 / (1.0f * Constant.HALF_ARRAY_HEIGHT);
            float g = 1 - r;
            float b = (r + g) / 3;
            m_Color = new Color(r, g, b, 1.0f);

            if (Constant.IsDebug)
                Debug.Log("Line color is: " + m_Color);
        }
    }

    void Awake()
    {
        if (m_Array == null)
            m_Array = new BaseObject[Constant.ROW, Constant.COLUMN];

        if (m_SelectedObjects == null)
            m_SelectedObjects = new List<BaseObject>();
    }

    bool CheckValidPosition(float x, float y)
    {
        if (x < -Constant.HALF_ARRAY_WIDTH * Constant.UNIT || x > Constant.HALF_ARRAY_WIDTH * Constant.UNIT)
            return false;

        if (y < -Constant.HALF_ARRAY_HEIGHT * Constant.UNIT || y > Constant.HALF_ARRAY_HEIGHT * Constant.UNIT)
            return false;

        return true;
    }

    int ConvertToColumnIndex(float x)
    {
        //if (x < -2.0f)
        //    return 0;

        //if (x < -1.0f)
        //    return 1;

        //if (x < 0.0f)
        //    return 2;

        //if (x < 1.0f)
        //    return 3;

        //if (x < 2.0f)
        //    return 4;

        //if (x < 3.0f)
        //    return 5;
        int index = -1;
        int i = 0;
        float offsetUnit = 0;
        float start = -Constant.HALF_ARRAY_WIDTH * Constant.UNIT;

        do
        {
            ++index;
            i++;
            offsetUnit = i * Constant.UNIT;
            if (Constant.IsDebug)
            {
                Debug.Log("index column: " + index);
                Debug.Log("current positionX: " + i);
            }
        } while (x > start + offsetUnit);
        return index;
    }

    int ConvertToRowIndex(float y)
    {
        //if (y < -2.0f)
        //    return 5;

        //if (y < -1.0f)
        //    return 4;

        //if (y < 0.0f)
        //    return 3;

        //if (y < 1.0f)
        //    return 2;

        //if (y < 2.0f)
        //    return 1;

        //if (y < 3.0f)
        //    return 0;

        int index = Constant.ROW;
        int i = 0;
        float offsetUnit = 0;
        float start = -Constant.HALF_ARRAY_HEIGHT * Constant.UNIT;

        do
        {
            --index;
            ++i;
            offsetUnit = i * Constant.UNIT;
        } while (y > start + offsetUnit);

        return index;
    }

    void UpdateObjectInMatrixAndWorld(BaseObject caller, int row, int column)
    {
        float offsetUnit = Constant.UNIT / 2;
        float posX = -Constant.HALF_ARRAY_WIDTH * Constant.UNIT + offsetUnit + column * Constant.UNIT;
        float posY = Constant.HALF_ARRAY_HEIGHT * Constant.UNIT - (offsetUnit + row * Constant.UNIT);
        //float posX = (-Constant.HALF_ARRAY_WIDTH + offsetUnit + column) * Constant.UNIT;
        //float posY = (Constant.HALF_ARRAY_HEIGHT - offsetUnit + row) * Constant.UNIT;
        Vector3 newPos = new Vector3(posX, posY, 0);
        caller.UpdatePositionInWorld(newPos);
        caller.UpdatePositionInMatrix(row, column);
    }

    void CheckRelevant(BaseObject obj)
    {
        int row = obj.GetRow();
        int column = obj.GetColumn();
        int level = obj.GetLevel();
        PetIconController.PetType petType = obj.GetPetObjectType();
        FoodController.FoodType foodType = obj.GetFoodType();

        //check up
        if ((column - 1) >= 0)
        {
            if (m_Array[row, column - 1] != null &&
                !m_Array[row, column - 1].Haschecked() &&
                m_Array[row, column - 1].GetPetObjectType() == petType &&
                m_Array[row, column - 1].GetFoodType() == foodType &&
                m_Array[row, column - 1].GetLevel() == level)
            {

                m_Array[row, column - 1].SetChecked(true);
                m_SelectedObjects.Add(m_Array[row, column - 1]);
                CheckRelevant(m_Array[row, column - 1]);

            }
        }

        //check down
        if ((column + 1) < Constant.COLUMN)
        {
            if (m_Array[row, column + 1] != null &&
                !m_Array[row, column + 1].Haschecked() &&
                m_Array[row, column + 1].GetPetObjectType() == petType &&
                m_Array[row, column + 1].GetFoodType() == foodType &&
                m_Array[row, column + 1].GetLevel() == level)
            {

                m_Array[row, column + 1].SetChecked(true);
                m_SelectedObjects.Add(m_Array[row, column + 1]);
                CheckRelevant(m_Array[row, column + 1]);

            }
        }

        //check right
        if ((row + 1) < Constant.ROW)
        {
            if (m_Array[row + 1, column] != null &&
                !m_Array[row + 1, column].Haschecked() &&
                m_Array[row + 1, column].GetPetObjectType() == petType &&
                m_Array[row + 1, column].GetFoodType() == foodType &&
                m_Array[row + 1, column].GetLevel() == level)
            {

                m_Array[row + 1, column].SetChecked(true);
                m_SelectedObjects.Add(m_Array[row + 1, column]);
                CheckRelevant(m_Array[row + 1, column]);

            }
        }

        //check left
        if ((row - 1) >= 0)
        {
            if (m_Array[row - 1, column] != null &&
                !m_Array[row - 1, column].Haschecked() &&
                m_Array[row - 1, column].GetPetObjectType() == petType &&
                m_Array[row - 1, column].GetFoodType() == foodType &&
                m_Array[row - 1, column].GetLevel() == level)
            {

                m_Array[row - 1, column].SetChecked(true);
                m_SelectedObjects.Add(m_Array[row - 1, column]);
                CheckRelevant(m_Array[row - 1, column]);

            }
        }
    }

    void HandleNewObject(BaseObject obj)
    {
        m_SelectedObjects.Clear();
        obj.SetChecked(true);
        m_SelectedObjects.Add(obj);

        CheckRelevant(obj);



        HandleMergedObjects();

    }

    void HandleMergedObjects()
    {
        if (m_SelectedObjects.Count >= 2)
        {

            RemoveMergedObjectsFromMatrix();

            int level = m_SelectedObjects[0].GetLevel();
            /*BaseObject.ObjectType*/
            PetIconController.PetType type = m_SelectedObjects[0].GetPetObjectType();
            FoodController.FoodType foodType = m_SelectedObjects[0].GetFoodType();

            DelegateManager.ShouldUpdatePetExperience(m_SelectedObjects.Count - 1, type, foodType, level, ref m_AnimatedObject, m_SelectedObjects[0]);
        }
        else
        {
            m_SelectedObjects[0].SetChecked(false);
        }
    }

    void ContinueToMerge(bool shouldContinue)
    {
        if (shouldContinue)
        {
            int level = m_SelectedObjects[0].GetLevel();
            //BaseObject.ObjectType type = m_SelectedObjects[0].GetObjectType();
            FoodController.FoodType foodType = m_SelectedObjects[0].FDController.CurrentFoodType;
            PetIconController.PetType petType = m_SelectedObjects[0].PIController.CurrentPetType;

            if (level >= DelegateManager.GetMaxLevel())
            {
                //ClearBaseObjectsFromList();
                return;
            }

            if (level + 1 <= Constant.LEVEL_MAX)
            {
                //New level object
                BaseObject newobj = DelegateManager.GetRequiredBaseObject(level + 1, petType, foodType);

                //raise the event for the finished object coordinate with the quest
                //DelegateManager.UpdateCompleteQuest (newobj.GetLevel(), newobj.GetObjectType());

                int row = m_SelectedObjects[0].GetRow();
                int column = m_SelectedObjects[0].GetColumn();
                Transform trans = m_SelectedObjects[0].GetTransformation();
                float x = trans.position.x;
                float y = trans.position.y;
                Vector3 pos = new Vector3(x, y, 0);
                newobj.UpdatePositionInMatrix(row, column);
                newobj.UpdatePositionInWorld(pos);
                newobj.UpdateObjectState(BaseObject.ObjectState.Frozen);
                m_Array[row, column] = newobj;
                
                ClearBaseObjectsFromList();

                HandleNewObject(newobj);
            }
            else
            {
                ClearBaseObjectsFromList();
            }
        }
    }
    void RemoveMergedObjectsFromMatrix()
    {

        foreach (BaseObject obj in m_SelectedObjects)
        {
            int row = obj.GetRow();
            int column = obj.GetColumn();
            m_Array[row, column] = null;
        }

    }

    void ClearBaseObjectsFromList()
    {
        if (m_SelectedObjects.Count >= 2)
        {
            foreach (BaseObject obj in m_SelectedObjects)
            {
                if (m_AnimatedObject != obj)
                {
                    obj.UpdateObjectState(BaseObject.ObjectState.Sleeping);
                    obj.UnLinkFoodAndPetIcon();
                }
            }
        }

        m_SelectedObjects.Clear();

    }

    void CheckBoardIsFull ()
    {
        int count = 0;
        bool isbreak = false;
        for (int i = 0; i < Constant.ROW; ++i)
        {
            for (int j = 0; j < Constant.COLUMN; ++j)
            {
                if (m_Array[i, j] != null)
                    ++count;
                else
                {
                    isbreak = true;
                    break;
                }
            }

            if (isbreak)
                break;
        }

        if (count == Constant.ROW * Constant.COLUMN)
            DelegateManager.BroadcastEndGame();
    }

    void NotifyBaseObjectDeselection(BaseObject caller)
    {
        //Stop timing
        DelegateManager.ShouldUpdateTime(false);

        //Reset Animated Object
        m_AnimatedObject = null;

        // a lot of logic will be done in here
        Transform trans = caller.GetTransformation();
        if (CheckValidPosition(trans.position.x, trans.position.y))
        {
            int column = ConvertToColumnIndex(trans.position.x);
            int row = ConvertToRowIndex(trans.position.y);

            if (Constant.IsDebug)
                Debug.Log("Row index: " + row + " and Column index: " + column);

            if (m_Array[row, column] == null)
            {
                m_Array[row, column] = caller;

                //Update Object in Matrix and world then freeze this object
                UpdateObjectInMatrixAndWorld(caller, row, column);
                caller.UpdateObjectState(BaseObject.ObjectState.Frozen);

                //Update matching logic
                HandleNewObject(caller);

                //Notify to remove current object and create new object in the list
                //DelegateManager.RemoveObjectFromList(caller);

                ClearBaseObjectsFromList();

                //Update other objects' state
                if (m_AnimatedObject == null)
                {
                    DelegateManager.NotifyOtherObjectState(caller, BaseObject.ObjectState.CanMoved);
                    DelegateManager.RemoveObjectFromList(caller);
                    DelegateManager.ShouldUpdateTime(true);
                }
                //else
                //{
                //    DelegateManager.MakeBaseObjectMoveToPet(m_AnimatedObject);
                //}

                //Check should refresh quests if that has been complete
                //DelegateManager.ShouldRefreshQuests();

            }
            else
            {
                //caller.SetTarget(caller.GetCachedPosition());
                //StartCoroutine(caller.RunToTarget(caller.GetCachedPosition(), false, 0));
                caller.SetUpForwardAnimation(caller.GetCachedPosition(), false);
                //caller.ReturnToCachedPosition();
                //DelegateManager.NotifyOtherObjectState(null, BaseObject.ObjectState.CanMoved);
            }
        }
        else
        {
            //caller.SetTarget(caller.GetCachedPosition());
            //StartCoroutine(caller.RunToTarget(caller.GetCachedPosition(), false, 0));
            caller.SetUpForwardAnimation(caller.GetCachedPosition(), false);
            //caller.ReturnToCachedPosition();
            //DelegateManager.NotifyOtherObjectState(null, BaseObject.ObjectState.CanMoved);
            //caller.UpdateObjectState (BaseObject.ObjectState.CanMoved);
        }

        //Check if the board is full --> broadcast endgame
        CheckBoardIsFull();
    }

    void OnEnable()
    {
        DelegateManager.NotifyBaseObjectDeselect += this.NotifyBaseObjectDeselection;
        DelegateManager.StartGame += this.StartGame;
        DelegateManager.ShouldContinueToMerge += ContinueToMerge;
    }

    void OnDisable()
    {
        DelegateManager.NotifyBaseObjectDeselect -= this.NotifyBaseObjectDeselection;
        DelegateManager.StartGame -= this.StartGame;
        DelegateManager.ShouldContinueToMerge -= ContinueToMerge;
    }

}
