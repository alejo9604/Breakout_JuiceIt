using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BezierCurve))]
public class WobbleLine : MonoBehaviour
{

    private const float MIN_VALUE = 0.0001f;
    private const float BOUNCE_X_FACTOR = 0.95f;

    [SerializeField] private Vector2 initPoint;
    [SerializeField] private Vector2 endPoint;

    private BezierCurve curve;

    [SerializeField] private float wobbleStrenght;
    [SerializeField] private float bounceSpeed = 0.25f;
    [SerializeField] private float bounciness = 0.85f;

    private Vector2 wobbleMiddlePointOnCenter;
    private Vector2 wobblePointImpact;
    private Vector2 wobbleVelocity;
    private Vector2 middlePoint;

    //Control var.
    private float lineRotation;
    private Vector2 wobbleMiddlePoint;


    public Vector2 GetImpactPoint() { return this.wobblePointImpact; }
    public Vector2 GetImpactPointOnCenter() { return this.wobblePointImpact.Rotate(this.lineRotation); }
    public Vector2 GetMiddlePointOnCenter() { return this.wobbleMiddlePointOnCenter; }
    public Vector2 GetMiddlePoint() { return this.wobbleMiddlePoint; }


    private void Awake()
    {
        this.curve = this.GetComponent<BezierCurve>();
    }

    public void Init()
    {
        Vector2 delta = this.endPoint - this.initPoint;
        float length = delta.magnitude;
        delta = delta.normalized;
        this.lineRotation = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        delta *= (length * 0.5f);
        this.middlePoint = this.initPoint + delta;

        this.curve.Draw(this.initPoint, this.endPoint);
    }

    public void OnReset()
    {
        this.Init();
    }

    void Update()
    {
        this.UpdateWobble();
    }

    private void UpdateWobble()
    {
        if (Mathf.Abs(this.wobbleMiddlePointOnCenter.y) > MIN_VALUE) {
            this.wobbleVelocity.y += -this.bounceSpeed * this.wobbleMiddlePointOnCenter.y;
            this.wobbleVelocity.y *= this.bounciness;
        }

        if (Mathf.Abs(this.wobbleMiddlePointOnCenter.x) > MIN_VALUE) {
            this.wobbleMiddlePointOnCenter.x *= BOUNCE_X_FACTOR;
        }

        this.wobbleMiddlePointOnCenter += this.wobbleVelocity;
        this.wobbleMiddlePoint = this.GetMiddleWobblePoint();
        this.curve.Draw(this.initPoint, this.wobbleMiddlePoint, this.endPoint);
    }

    private void SetWobbleInitPoint(Vector2 point)
    {
        this.wobblePointImpact = point - this.middlePoint;
        this.wobbleMiddlePointOnCenter = this.wobblePointImpact.Rotate(this.lineRotation);
    }

    private Vector2 GetMiddleWobblePoint()
    {
        Vector2 mid = this.wobbleMiddlePointOnCenter.Rotate(-this.lineRotation);
        mid += this.middlePoint;
        return mid;
    }


    public void OnImpact(Vector2 point, Vector2 velocity)
    {
        Vector3 velNormalize = velocity.normalized;
        this.SetWobbleInitPoint( new Vector2(point.x + this.wobbleStrenght * velNormalize.x, point.y + this.wobbleStrenght * velNormalize.y));
    }


}
