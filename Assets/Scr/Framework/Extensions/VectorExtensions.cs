using UnityEngine;

public static class VectorExtensions 
{
    public static Vector2 Rotate(this Vector2 target, float angle)
    {
        Vector2 rotatedVector = new Vector2(target.x, target.y);
        rotatedVector.x = target.x * Mathf.Cos(angle * Mathf.Deg2Rad) - target.y * Mathf.Sin(angle * Mathf.Deg2Rad);
        rotatedVector.y = target.x * Mathf.Sin(angle * Mathf.Deg2Rad) + target.y * Mathf.Cos(angle * Mathf.Deg2Rad);
        return rotatedVector;
    }
}
