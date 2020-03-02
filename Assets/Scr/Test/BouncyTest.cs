using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyTest : MonoBehaviour
{
    public Transform target;
    public bool scale = false;
    public bool normalBounce = false;

    [Space(20)]
    public float bounceSpeed = 0.25f;
    public float bounciness = 0.85f;

    [Space(20)]
    public Vector2 value = Vector2.zero;
    public Vector2 targetValue = Vector2.one;
    public Vector2 velocity = Vector2.zero;
    
    [Space(20)]
    public Vector2 targetVel = Vector2.one;

    [Space(20)]
    public float minValue = 0.001f;

    private Vector3 initPosition;
    private Vector3 initScale;

    [Space(20)]
    public AnimationCurve valueCurve;
    public AnimationCurve velCurve;
    private float time;

    void Start()
    {
        this.initPosition = this.target.position;
        this.initScale = this.target.localScale;
        this.valueCurve = new AnimationCurve();
        this.velCurve = new AnimationCurve();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space)) {
            this.value = this.targetValue;
            this.valueCurve = new AnimationCurve();
            this.velCurve = new AnimationCurve();
            if (!this.normalBounce)
                this.velocity = this.targetVel;
        }

        if (normalBounce)
            this.Bounce();
        else
            this.Bounce2();


        this.target.localScale = this.initScale + (!this.scale ? Vector3.zero :  (Vector3) this.value);
        this.target.position = this.initPosition + (this.scale ? Vector3.zero : (Vector3) this.value);

        if (Mathf.Abs(value.x) >= minValue) {
            this.valueCurve.AddKey(this.time, this.value.x);
            this.velCurve.AddKey(this.time, this.velocity.x);
        }
    }


    private void Bounce()
    {
        //dx = (dx - a * x) * b
        //x = x + dx

        if (Mathf.Abs(value.x) >= minValue) {

            this.velocity += -this.bounceSpeed * value;
            this.velocity *= this.bounciness;

            this.time += Time.deltaTime;
        }
        else {
            this.value = Vector2.zero;
            this.velocity = Vector2.zero;
            this.time = 0;
        }

        this.value += this.velocity;
    }

    private void Bounce2()
    {
        if (Mathf.Abs(value.x) >= minValue) {

            this.velocity += -this.bounceSpeed * value / Time.deltaTime;
            this.velocity -= this.bounciness * this.velocity;

            this.time += Time.deltaTime;
        }
        else {
            this.value = Vector2.zero;
            //this.velocity = Vector2.zero;
            this.time = 0;
        }

        this.value += Time.deltaTime * this.velocity;
    }
}
