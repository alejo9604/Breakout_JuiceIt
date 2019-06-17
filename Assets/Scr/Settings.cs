using UnityEngine;

public class Settings
{
    //Ball
    public static float BALL_MIN_VELOCITY = 8;
    public static float BALL_MAX_VELOCITY = 10;
    public static float BALL_VELOCITY_LOSS = .01f;

    //Color
    public static bool EFFECT_SCREEN_COLORS = false;
    

    public static Vector2 BallMinVelocity() {
        return new Vector2(BALL_MIN_VELOCITY, BALL_MIN_VELOCITY);
    }

    public static Vector2 BallMaxVelocity() {
        return new Vector2(BALL_MAX_VELOCITY, BALL_MAX_VELOCITY);
    }
}
