using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobbleTest : MonoBehaviour
{
    public BezierCurve curve;

    [Space(20)]
    public Transform p1;
    public Transform p2;
    public Transform ball;
    public Transform wobbleImpactPoint;
    public Transform wobbleImpactPointCenter;
    public Transform wobbleMiddlePoint;
    public Transform wobbleMiddlePointCenter;
    [SerializeField] private Vector2 ballSpeed = new Vector2(0, 1);


    public WobbleLine wobbleLine;

    private void Start()
    {
        this.wobbleLine.Init();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            this.Init();
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            this.ResetLine();
        }


        this.wobbleImpactPoint.position = this.wobbleLine.GetImpactPoint();
        this.wobbleImpactPointCenter.position = this.wobbleLine.GetImpactPointOnCenter();
        this.wobbleMiddlePointCenter.position = this.wobbleLine.GetMiddlePointOnCenter();
        this.wobbleMiddlePoint.position = this.wobbleLine.GetMiddlePoint();
    }

    void Init()
    {

        this.wobbleLine.OnImpact(this.ball.position, this.ballSpeed);

    }

    void ResetLine()
    {
        this.wobbleLine.OnReset();
    }

}
