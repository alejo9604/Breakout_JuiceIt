using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : BreakoutElement
{

    private const float INIT_POS_Y = -1.77f;

    [Header("Movement")]
    [SerializeField]
    protected float initSpeed = 5;

    [Header("Visuals")]
    [SerializeField]
    private Color color = Color.yellow;


    //Components
    private Rigidbody2D rb;


    //Control var.
    private Vector2 velocity;


    public float Velocity
    {
        get
        {
            return velocity.magnitude;
        }
        set
        {
            float ratio = value / this.Velocity;
            velocity *= ratio;
        }
    }


    protected override void Start()
    {
        base.Start();

        this.rb = GetComponent<Rigidbody2D>();

        this.ResetElement();

        //Add events
        EventManager.Instance.AddListener<ChangeColorEvent>(this.OnChangeColor);
        EventManager.Instance.AddListener<InputResetLevelEvent>(this.OnInputResetLevel);
    }

    private void OnDestroy()
    {
        if (EventManager.HasInstance()) {
            EventManager.Instance.RemoveListener<ChangeColorEvent>(this.OnChangeColor);
            EventManager.Instance.RemoveListener<InputResetLevelEvent>(this.OnInputResetLevel);
        }
    }


    private void FixedUpdate() {
        this.SetVelocity();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.tag == "Brick") {
            //TODO: Use Brick script or usr Event
            collision.collider.gameObject.SetActive(false);
        }
    }

    
    public override void ResetElement()
    {
        base.ResetElement();

        this.transform.position = new Vector3(0, INIT_POS_Y, 0);
        this.velocity = Random.insideUnitCircle * initSpeed;
        this.rb.velocity = this.velocity;
    }

    private void SetVelocity() {
        this.velocity = rb.velocity;
        float currentVelocity = this.Velocity;

        if (currentVelocity < Settings.BALL_MIN_VELOCITY)
            this.Velocity = Settings.BALL_MIN_VELOCITY;
        else if(currentVelocity > Settings.BALL_MAX_VELOCITY)
            this.Velocity -= currentVelocity * Settings.BALL_VELOCITY_LOSS * Time.deltaTime;

        if(Mathf.Abs(this.velocity.x) < Settings.BALL_VELOCITY_MIN_AXIS_VALUE) {
            this.velocity.x += Mathf.Sign(this.velocity.x) * Settings.BALL_VELOCITY_MIN_AXIS_VALUE * Time.deltaTime;
        }
        else if (Mathf.Abs(this.velocity.y) < Settings.BALL_VELOCITY_MIN_AXIS_VALUE) {
            this.velocity.y += Mathf.Sign(this.velocity.y) * Settings.BALL_VELOCITY_MIN_AXIS_VALUE * Time.deltaTime;
        }

        this.rb.velocity = this.velocity;
    }





    #region Events
    private void OnChangeColor(ChangeColorEvent e)
    {
        this.ChangeColor(Settings.EFFECT_SCREEN_COLORS ? this.color : Color.white);
    }

    private void OnInputResetLevel(InputResetLevelEvent e)
    {
        this.ResetElement();
    }
    #endregion Events

}
