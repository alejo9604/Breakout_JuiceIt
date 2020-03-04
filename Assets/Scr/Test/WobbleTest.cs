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
    public Transform target;
    public Transform initWobble;
    public Transform wobblePos;
    public Transform midPos;
    [SerializeField] private Vector2 ballSpeed = new Vector2(0, 1);

    [Space(20)]
    public float STRENGHT = 10;
    public float MIN = 0.0001f;
    public float X_MUL = 0.95f;
    public float DELAY = 0.5f;

    [Space(20)]
    [SerializeField] private Vector2 pos_middle;
    [SerializeField] private float line_rotation;
    
    [Space(20)]
    [SerializeField] private Vector2 wobble_pos;
    [SerializeField] private Vector2 wobble_middle;
    [SerializeField] private Vector2 wobble_velocity;

    [Space(20)]
    [SerializeField] private float bounce_speed = 0.25f;
    [SerializeField] private float bounciness = 0.85f;

    [Space(20)]
    [SerializeField] private bool isUpdating = true;
    [SerializeField] private bool autoStart = true;
    [SerializeField] private bool useCoroutine = true;

    private void Start()
    {
        this.ResetLine();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            this.Init();
            if(this.autoStart)
                this.isUpdating = true;

            StopAllCoroutines();
            if (this.useCoroutine)
                StartCoroutine(this.StartUpdate());
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            this.isUpdating = false;
            StopAllCoroutines();
            this.ResetLine();
        }

        if (!this.useCoroutine && this.isUpdating)
            this.OnUpdate();
        
    }

    [Space(20)]
    [SerializeField] Vector3 delta;
    void Init()
    {
        delta = p2.position - p1.position;
        float length = delta.magnitude;
        delta = delta.normalized;
        this.line_rotation = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        delta *= (length * 0.5f);
        this.pos_middle = p1.position + delta;


        Vector2 point = new Vector2(this.ball.position.x + STRENGHT * this.ballSpeed.x, this.ball.position.y + STRENGHT * this.ballSpeed.y);
        this.Wobble(point);
        
    }

    void ResetLine()
    {
        this.curve.Draw(this.p1.position, this.p2.position);
    }


    IEnumerator StartUpdate()
    {
        while (true) {
            this.OnUpdate();
            yield return new WaitForSeconds(this.DELAY);
        }
    }

    void OnUpdate()
    {
        if(Mathf.Abs(this.wobble_middle.y) > MIN) {
            this.wobble_velocity.y += -this.bounce_speed * this.wobble_middle.y;
            this.wobble_velocity.y *= this.bounciness;
        }

        if (Mathf.Abs(this.wobble_middle.x) > MIN) {
            this.wobble_middle.x *= X_MUL;
        }

        this.wobble_middle += this.wobble_velocity;

        this.target.position = this.Middle();

        this.curve.Draw(this.p1.position, this.target.position, this.p2.position);
    }

    void Wobble(Vector2 point)
    {
        this.wobble_pos = point - pos_middle;

        this.initWobble.position = this.wobble_pos;

        //this.wobble_pos = this.wobble_pos * Quaternion.Euler(0, 0, this.line_rotation);
        this.wobble_middle = this.Rotate(this.wobble_pos, this.line_rotation);

        this.wobblePos.position = this.wobble_middle;
    }
    
    Vector2 Middle()
    {
        this.midPos.position = this.wobble_middle;
        Vector2 mid = this.Rotate(this.wobble_middle, -this.line_rotation);
        mid += this.pos_middle;
        return mid;
    }

    private Vector2 Rotate(Vector2 target, float angle)
    {
        Vector2 rotatedVector = new Vector2(target.x, target.y);
        rotatedVector.x = target.x * Mathf.Cos(angle * Mathf.Deg2Rad) - target.y * Mathf.Sin(angle * Mathf.Deg2Rad);
        rotatedVector.y = target.x * Mathf.Sin(angle * Mathf.Deg2Rad) + target.y * Mathf.Cos(angle * Mathf.Deg2Rad);
        return rotatedVector;
    }
}
