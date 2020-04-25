using UnityEngine;
using DG.Tweening;

public class Brick : BreakoutElement
{

    private int id;
    private BoxCollider2D boxCollider;

    private Vector2 velocity = Vector2.zero;

    //Contorl var.
    private bool hasCollided = false;
    private Sequence enterSequence;
    private Sequence destructionSequence;

    public int GetBrickId()
    {
        return this.id;
    }

    public void SetBrickId(int id)
    {
        this.id = id;
    }

    public override void Init()
    {
        base.Init();
        this.boxCollider = this.GetComponent<BoxCollider2D>();
    }

    public override void ResetElement()
    {
        base.ResetElement();

        this.KillTweens();

        this.boxCollider.enabled = true;
        this.gameObject.SetActive(true);

        this.velocity = Vector2.zero;
        this.hasCollided = false;

        if (!Settings.IS_TWEENING_ENABLE) {
            this.transform.position = this.initPosition;
            this.transform.eulerAngles = Vector3.zero;
            this.transform.localScale = this.initScale;
        }
        else {
            this.PlayEnterTween();
        }
    }


    private void Update()
    {
        this.transform.position += (Vector3) this.velocity * Time.deltaTime;

        if(Settings.BRICK_GRAVITY_ON_COLLISION && this.hasCollided) {
            this.velocity.y += Settings.BRICK_GRAVITY * Time.deltaTime;
        }
    }


    public override void OnCollision(Vector2 position, Vector2 velocity)
    {
        base.OnCollision(position, velocity);

        this.KillTweens();

        this.boxCollider.enabled = false;

        this.hasCollided = true;


        if (!this.HasEffectOnCollision()) {
            this.gameObject.SetActive(false);
            return;
        }

        if (Settings.BRICK_PUSH_ON_COLLISION) {

            Vector2 brickPosition = new Vector2(this.transform.position.x, this.transform.position.y);
            Vector2 point = position - brickPosition;
            point = point.Normalize(velocity.magnitude) * -0.5f;
            this.velocity += point;
        }

        this.PlayDestructionTween();
    }

    private bool HasEffectOnCollision()
    {
        return Settings.BRICK_SCALE_ON_COLLISION || Settings.BRICK_GRAVITY_ON_COLLISION || Settings.BRICK_PUSH_ON_COLLISION;
    }



    private void KillTweens()
    {
        if (this.enterSequence != null && this.enterSequence.IsActive()) {
            this.enterSequence.Complete();
            this.enterSequence.Kill();
            this.enterSequence = null;
        }

        if (this.destructionSequence != null && this.destructionSequence.IsActive()) {
            this.destructionSequence.Complete();
            this.destructionSequence.Kill();
            this.destructionSequence = null;
        }
    }


    //On Reset Tweening
    public void PlayEnterTween()
    {
        this.enterSequence = DOTween.Sequence();

        if (Settings.TWEENING_DELAY_VALUE > 0) {
            this.enterSequence.AppendCallback(() => this.gameObject.SetActive(false));
            this.enterSequence.AppendInterval(Random.Range(0, Settings.TWEENING_DELAY_VALUE));
            this.enterSequence.AppendCallback(() => this.gameObject.SetActive(true));
            this.enterSequence.AppendInterval(0.05f);
        }

        if (Settings.TWEENING_Y_AT_START)
            this.enterSequence.Join(this.PlayEnterAxisYTween());
        else
            this.transform.position = this.initPosition;

        if (Settings.TWEENING_ROTATION_AT_START)
            this.enterSequence.Join(this.PlayEnterRotationTween());
        else
            this.transform.eulerAngles = Vector3.zero;

        if (Settings.TWEENING_SCALE_AT_START)
            this.enterSequence.Join(this.PlayEnterScaleTween());
        else
            this.transform.localScale = this.initScale;
    }


    private Tween PlayEnterAxisYTween()
    {
        Vector3 offset = Vector3.up * (Breakout.Instance.GetCameraWorldSize().y + Breakout.Instance.GetCameraPosition().y / 2);
        return this.transform.DOMove(this.initPosition, Settings.TWEENING_ENTER_TIME).From(offset + this.initPosition, true).SetEase(Settings.TWEENING_EASE);
    }

    private Tween PlayEnterRotationTween()
    {
        float rotation = Random.Range(-Settings.TWEENING_ROTATION_MAX_ANGLE, Settings.TWEENING_ROTATION_MAX_ANGLE);
        if (Mathf.Abs(rotation) < Settings.TWEENING_ROTATION_MIN_ANGLE)
            rotation = Mathf.Sign(rotation) * Settings.TWEENING_ROTATION_MIN_ANGLE;

        return this.transform.DORotate(Vector3.zero, Settings.TWEENING_ENTER_TIME).From(Vector3.back * rotation).SetEase(Settings.TWEENING_EASE);
    }

    private Tween PlayEnterScaleTween()
    {
        return this.transform.DOScale(this.initScale, Settings.TWEENING_ENTER_TIME).From(this.initScale * Settings.TWEENING_SCALE_INIT_FACTOR).SetEase(Settings.TWEENING_EASE);
    }


    //On Collision Tweening
    public void PlayDestructionTween() {

        this.destructionSequence = DOTween.Sequence();

        if (Settings.BRICK_SCALE_ON_COLLISION) {
            this.destructionSequence.Append(this.PlayDestructionScaleTween());
        }

        this.destructionSequence.InsertCallback(Settings.BRICK_DESTRUCTION_TIME, () => this.gameObject.SetActive(false));

    }

    private Tween PlayDestructionScaleTween()
    {
        return this.transform.DOScale(Vector3.zero, Settings.BRICK_DESTRUCTION_TIME).From(this.transform.localScale).SetEase(Ease.OutQuart);
    }
}
