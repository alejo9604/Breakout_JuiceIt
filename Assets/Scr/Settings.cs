using DG.Tweening;
using UnityEngine;

public class Settings
{
    //Ball
    public static float BALL_MIN_VELOCITY = 8;
    public static float BALL_MAX_VELOCITY = 10;
    public static float BALL_VELOCITY_LOSS = .01f;
    public static float BALL_VELOCITY_MIN_AXIS_VALUE = 0.5f;

    //Color
    public static bool EFFECT_SCREEN_COLORS = false;

    //Tweening
    public static bool IS_TWEENING_ENABLE = false;
    public static bool TWEENING_Y_AT_START = true;
    public static bool TWEENING_ROTATION_AT_START = false;
    public static bool TWEENING_SCALE_AT_START = false;
    public static float TWEENING_DELAY_VALUE = 0.1f; //[0, 1]
    public static float TWEENING_ENTER_TIME = 0.5f;
    public static Ease TWEENING_EASE = Ease.Linear;

    public const float TWEENING_ROTATION_MAX_ANGLE = 120;
    public const float TWEENING_ROTATION_MIN_ANGLE = 20f;
    public const float TWEENING_SCALE_INIT_FACTOR = 0.15f;

    //Strech
    public static bool PADDLE_STRECH = false;
    public static bool BALL_EXTRA_SCALE_ON_HIT = false;
    public static bool BALL_ROTATION_AND_STRECH = false;
    public static bool BALL_STRECH_ON_HIT = false;

    public static readonly Vector3 PADDLE_STRECH_FACTOR = new Vector3(1.5f, -0.75f);

    //Util
    public static Vector2 BallMinVelocity() {
        return new Vector2(BALL_MIN_VELOCITY, BALL_MIN_VELOCITY);
    }

    public static Vector2 BallMaxVelocity() {
        return new Vector2(BALL_MAX_VELOCITY, BALL_MAX_VELOCITY);
    }

    public static Ease GetEaseByIndex(int index)
    {
        switch (index) {
            case 0:
                return Ease.Linear;
            case 1:
                return Ease.OutCubic;
            case 2:
                return Ease.OutBack;
            case 3:
                return Ease.OutBounce;
        }

        return Ease.Unset;
    }

    //TODO: Looks how to do this better
    public static int EaseToIndex(Ease ease)
    {
        switch (ease) {
            case Ease.Linear:
                return 0;
            case Ease.OutCubic:
                return 1;
            case Ease.OutBack:
                return 2;
            case Ease.OutBounce:
                return 3;
        }

        return 0;
    }
}
