using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constant{
	public static readonly int ROW = 5;
	public static readonly int COLUMN = 5;

	public static readonly float HALF_ARRAY_WIDTH = 2.5f;
	public static readonly float HALF_ARRAY_HEIGHT = 2.5f;

    public const float UNIT = 2f;
	public const float OFFSET_FROM_MATRIX = 1.0f;

	public static readonly bool IsDebug = false;

	public static readonly float WIDTH = 20f;
	//public static readonly float HALF_WIDTH = 7.5f;
	public static readonly float OFFSET = 0.5f;
    //public const float HIGHT_GAP = 3f;

	public const int NUMBER_OBJECTS = 10;
	public const int MAX_QUESTS = 3;

    public const float SMOOTH_TIME = 0.1f;

	public const int CONSTANT_NUMBER = 123456;

	//object level
	public const int LEVEL_1 = 1;
	public const int LEVEL_2 = 2;
	public const int LEVEL_3 = 3;

	public const int LEVEL_MIN = Constant.LEVEL_1;
	public const int LEVEL_MAX = Constant.LEVEL_3;

    //Pet
    public const int PETS = 3;

    //Currently, hard code these params
    public const int EXPERIENCE_LEVEL_01 = 5;
    public const int EXPERIENCE_LEVEL_02 = 5;
    public const int EXPERIENCE_LEVEL_03 = 5;

    public const string LAYER_BASE_OBJECT = "BaseObject";
}
