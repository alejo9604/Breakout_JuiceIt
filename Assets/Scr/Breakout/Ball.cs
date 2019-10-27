using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    protected float initSpeed = 5;

    [Header("Visuals")]
    [SerializeField]
    private Color color = Color.yellow;


    //Components
    private Rigidbody2D rb;
    private SpriteRenderer render;


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


    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.render = GetComponent<SpriteRenderer>();

        this.velocity = Random.insideUnitCircle * initSpeed;
        this.rb.velocity = this.velocity;

        //Add events
        EventManager.Instance.AddListener<ChangeColorEvent>(this.OnChangeColor);
    }

    private void OnDestroy()
    {
        if (EventManager.HasInstance()) {
            EventManager.Instance.RemoveListener<ChangeColorEvent>(this.OnChangeColor);
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


    private void SetVelocity() {
        this.velocity = rb.velocity;
        float currentVelocity = this.Velocity;

        if (currentVelocity < Settings.BALL_MIN_VELOCITY)
            this.Velocity = Settings.BALL_MIN_VELOCITY;
        else if(currentVelocity > Settings.BALL_MAX_VELOCITY)
            this.Velocity -= currentVelocity * Settings.BALL_VELOCITY_LOSS * Time.deltaTime;

        this.rb.velocity = this.velocity;
    }


    #region Events
    private void OnChangeColor(ChangeColorEvent e)
    {
        this.render.color = Settings.EFFECT_SCREEN_COLORS ? this.color : Color.white;
    }
    #endregion Events

}
