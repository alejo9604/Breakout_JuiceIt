using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : BreakoutElement
{
    //Extra scale on hit
    const float EXTRA_SCALE_ON_HIT = .4f;
    const float MAX_EXTRA_SCALE_ON_HIT = 1.2f;
    const float EXTRA_SCALE_DECREASE_FACTOR = 15f;
    //Strech
    const float STRECH_FACTOR = 0.15f;
    const float EXTRA_STRECH_Y = 0.02f;
    const float MAX_STRECH_PLUS = 0.2f;
    //Punch
    const float PUNCH_MIN_VALUE = 0.001f;
    const float ANIMATION_STRECH_PLUS = 0.5f;
    //Glow
    const float GLOW_DURATION = 0.2f;
    //Hit-Sound
    const float SOUND_TIME = 0.5f;

    [Header("Movement")]
    [SerializeField]
    protected float initSpeed = 5;

    [Header("Visuals")]
    [SerializeField]
    private Color color = Color.yellow;
    [SerializeField]
    private Color glowColor = Color.white;


    //Child visual
    private Transform childTransform;
    private Vector3 childInitScale;

    //Components
    private Rigidbody2D rb;


    //Control var.
    private Vector2 velocity;
    private Vector3 scale;
    private Vector3 rotation = Vector3.zero;
    private float extraScale = 0;
    private float angle = 0;

    //Sound
    private int hitsCounts = 0;
    private float hitsTimer = 0;

    //private Tween punchTween;
    private float punch;
    private float punchShakiness = 0.85f;
    private float punchSpeed = 0.25f;
    private float punchVelocity;


    protected Color GetColor()
    {
        return Settings.EFFECT_SCREEN_COLORS ? this.color : Color.white;
    }

    public float Velocity
    {
        get { return velocity.magnitude; }
        set {
            float ratio = value / this.Velocity;
            velocity *= ratio;
        }
    }


    protected override void Start()
    {
        base.Start();

        this.rb = GetComponent<Rigidbody2D>();

        //TODO: Create it if don't exist
        this.childTransform = this.transform.GetChild(0);
        this.childInitScale = this.childTransform.localScale;

        //Override render
        this.render = this.childTransform.GetComponent<SpriteRenderer>();

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

    private void Update()
    {
        this.UpdateElement();
    }

    private void FixedUpdate() {
        this.SetVelocity();
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        BreakoutElement otherCollider;
        if (!collision.collider.TryGetComponent<BreakoutElement>(out otherCollider))
            return;

        //TODO: Use Brick script or usr Event?
        if (otherCollider is Wall)
            otherCollider.OnCollision(collision.contacts[0].point, this.velocity);
        else {
            otherCollider.OnCollision();

            if (otherCollider is Brick) {
                this.hitsCounts = this.hitsTimer >= SOUND_TIME ? 0 : this.hitsCounts + 1;
                this.hitsTimer = 0;

                if (Settings.SOUND_BRICK)
                    AudioManager.Instance.PlayBrickClip(hitsCounts);
            }
        }


        EventManager.Instance.Trigger(new BallCollisionEvent());
        this.OnHitCollision();
    }

    
    public override void ResetElement()
    {
        base.ResetElement();

        this.transform.position = this.initPosition;
        this.transform.localScale = this.initScale;
        this.velocity = Random.insideUnitCircle * initSpeed;
        this.rb.velocity = this.velocity;

        this.childTransform.localScale = this.childInitScale;

        this.hitsCounts = 0;
        this.hitsTimer = 0;
    }


    private void UpdateElement()
    {
        //Scale
        this.scale = this.childInitScale;


        //Rotation && Strech
        if (Settings.BALL_ROTATION_AND_STRECH) {
            this.angle = Mathf.Atan2(this.velocity.y, this.velocity.x) * Mathf.Rad2Deg - 90;
            this.rotation.z = angle;

            this.scale.y += Mathf.Abs(this.velocity.y) * STRECH_FACTOR;
            this.scale.y = Mathf.Clamp(this.scale.y, this.childInitScale.y, this.childInitScale.y + MAX_STRECH_PLUS);
            this.scale.x -= (this.scale.y - this.childInitScale.y);

            this.scale.y += EXTRA_STRECH_Y;
        }
        else {
            this.rotation = Vector3.zero;
        }

        //Punch/Animation Strech
        if (Mathf.Abs(this.punch) >= PUNCH_MIN_VALUE) {
            this.punchVelocity += -this.punchSpeed * this.punch;
            this.punchVelocity *= this.punchShakiness;
        }
        else {
            this.punch = 0;
            this.punchVelocity = 0;
        }
        this.punch += this.punchVelocity;


        if (Settings.BALL_STRECH_ON_HIT) {
            this.scale.x += this.punch;
            this.scale.y += this.punch;
        }

        //Extra scale on Hit
        this.scale += Vector3.one * this.extraScale;
        if (this.extraScale > 0.01f)
            this.extraScale -= Time.deltaTime * this.extraScale * EXTRA_SCALE_DECREASE_FACTOR;
        else
            this.extraScale = 0;



        this.childTransform.localScale = this.scale;
        this.childTransform.localEulerAngles = this.rotation;


        //Hits-Sound
        this.hitsTimer += Time.deltaTime;
    }

    private void SetVelocity() {
        this.velocity = rb.velocity;
        float currentVelocity = this.Velocity;

        this.velocity.y -= Settings.BALL_GRAVITY * Time.deltaTime;

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


    private void OnHitCollision()
    {
        if (Settings.BALL_EXTRA_SCALE_ON_HIT) {
            this.extraScale += EXTRA_SCALE_ON_HIT;
            this.extraScale = Mathf.Clamp(this.extraScale, 0, MAX_EXTRA_SCALE_ON_HIT);
        }

        this.punch = ANIMATION_STRECH_PLUS;

        if (Settings.BALL_GLOW_ON_HIT)
            this.OnGlowColor(this.glowColor, this.GetColor(), GLOW_DURATION, Ease.InCubic);
    }


    #region Events
    private void OnChangeColor(ChangeColorEvent e)
    {
        this.ChangeColor(this.GetColor());
    }

    private void OnInputResetLevel(InputResetLevelEvent e)
    {
        this.ResetElement();
    }
    #endregion Events

}
