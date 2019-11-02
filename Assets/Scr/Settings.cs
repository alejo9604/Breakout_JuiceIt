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
    public static bool TWEENING_Y_AT_START = false;
    public static bool TWEENING_ROTATION_AT_START = false;
    public static bool TWEENING_SCALE_AT_START = false;

    public const float TWEENING_ENTER_TIME = 0.45f;
    public const float TWEENING_ROTATION_MAX_ANGLE = 120;
    public const float TWEENING_ROTATION_MIN_ANGLE = 20f;
    public const float TWEENING_SCALE_INIT_FACTOR = 0.15f;

    public static Vector2 BallMinVelocity() {
        return new Vector2(BALL_MIN_VELOCITY, BALL_MIN_VELOCITY);
    }

    public static Vector2 BallMaxVelocity() {
        return new Vector2(BALL_MAX_VELOCITY, BALL_MAX_VELOCITY);
    }
}
