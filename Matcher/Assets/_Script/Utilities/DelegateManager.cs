
#region Handle Input
public delegate void TouchBegin (UnityEngine.GameObject obj, UnityEngine.Vector3 pos);

public delegate void TouchMove (UnityEngine.Vector3 pos);

public delegate void TouchEnd ();
#endregion


#region Base object handler
public delegate void NotifyOtherObjectState (BaseObject mainObj, BaseObject.ObjectState state);

public delegate void NotifyBaseObjectDeselect (BaseObject obj);

public delegate void RemoveObjectFromList (BaseObject obj);

public delegate BaseObject GetRequiredBaseObject (int level, PetIconController.PetType petType, FoodController.FoodType foodType);
#endregion


#region Utilities

public delegate int GetRandomLevel (int min, int max);

public delegate float GetRandomFloat ();

public delegate void ShouldKeepMoving (bool isMoving);

public delegate void BroadcastGameStatus (bool pausedStatus);
#endregion

#region Game progressing
public delegate void StartGame();

public delegate void OnLoadGameData ();

public delegate void OnSaveGameData ();

public delegate void UpdateGame(float deltaTime);

public delegate void UpdateMaxLevel (int level);

public delegate int GetMaxLevel ();

public delegate void ShouldUpdateTime (bool updated);

public delegate void BroadcastEndGame ();

public delegate void UpdateDeadPet(int deadPet = 1);

public delegate void UpdateCombo(bool up, int delta = 1);

public delegate int GetCombo();

public delegate void UpdateGameCoins (int coin, int combo);

public delegate void CanHandleTouch (bool isactive);
#endregion


#region Quests handler
public delegate int RetrieveCompleteQuestsInLevel (int level);

public delegate void UpdateCompleteQuestsInLevel (int level);

public delegate void UpdateCompleteQuest (int level, /*BaseObject.ObjectType type*/ PetIconController.PetType petType);

public delegate void ShouldRefreshQuests ();

public delegate void DisplayQuests ();
#endregion

#region Pet handler
public delegate void ShouldUpdatePetExperience(int exp, PetIconController.PetType petType, FoodController.FoodType foodType, int level, ref BaseObject animatedObj, BaseObject sample);

public delegate void UpdateStatWithFactor (int factor);

public delegate void ShouldContinueToMerge (bool shouldContinue);

public delegate PetIconController GetPetIcon (int level, PetIconController.PetType type);

public delegate PetIconController.PetType GetRandomPetType (int level);

//public delegate void MakeBaseObjectMoveToPet (BaseObject caller);

#endregion

#region Food
public delegate FoodController GetFood(FoodController.FoodType type);

public delegate /*BaseObject.ObjectType*/FoodController.FoodType GetRandomFoodType();

public delegate void ResetFood ();

#endregion


public static class DelegateManager
{
    #region Handle Input
    public static TouchBegin TouchBegin;

    public static TouchMove TouchMove;

    public static TouchEnd TouchEnd;
    #endregion

    #region Base object handler
    public static NotifyOtherObjectState NotifyOtherObjectState;// = new NotifyTouch(null);

	public static NotifyBaseObjectDeselect NotifyBaseObjectDeselect;

	public static RemoveObjectFromList RemoveObjectFromList;

	public static GetRequiredBaseObject GetRequiredBaseObject;
    #endregion

    #region Utilities
	public static GetRandomLevel GetRandomLevel;

    public static ShouldKeepMoving ShouldKeepMoving;

    public static GetRandomFloat GetRandomFloat;

    public static BroadcastGameStatus BroadcastGameStatus;
    #endregion

    #region Game progressing
    public static StartGame StartGame;

    public static OnLoadGameData OnLoadGameData;

	public static OnSaveGameData OnSaveGameData;

    public static UpdateGame UpdateGame;

    public static UpdateMaxLevel UpdateMaxLevel;

    public static GetMaxLevel GetMaxLevel;

    public static ShouldUpdateTime ShouldUpdateTime;

    public static BroadcastEndGame BroadcastEndGame;

    public static UpdateDeadPet UpdateDeadPet;

    public static UpdateCombo UpdateCombo;

    public static GetCombo GetCombo;

    public static UpdateGameCoins UpdateGameCoins;

    public static CanHandleTouch CanHandleTouch;
    #endregion

    #region Quests handler
    public static RetrieveCompleteQuestsInLevel RetrieveCompleteQuestsInLevel;

	public static UpdateCompleteQuestsInLevel UpdateCompleteQuestsInLevel;

	public static UpdateCompleteQuest UpdateCompleteQuest;

	public static ShouldRefreshQuests ShouldRefreshQuests;

	public static DisplayQuests DisplayQuests;
    #endregion

    #region Pet Handler
    public static ShouldUpdatePetExperience ShouldUpdatePetExperience;

    public static UpdateStatWithFactor UpdateStatWithFactor;

    public static ShouldContinueToMerge ShouldContinueToMerge;

    public static GetPetIcon GetPetIcon;

    public static GetRandomPetType GetRandomPetType;

    //public static MakeBaseObjectMoveToPet MakeBaseObjectMoveToPet;

    #endregion

    #region Food
    public static GetRandomFoodType GetRandomFoodType;
    public static GetFood GetFood;
    public static ResetFood ResetFood;
    #endregion
};