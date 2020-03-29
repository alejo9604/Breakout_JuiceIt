using UnityEngine;
using DG.Tweening;

public class BreakoutElement : MonoBehaviour
{
    //Components
    protected SpriteRenderer render;

    //Control vars
    protected Vector3 initPosition;
    protected Vector3 initScale;

    //Glow
    protected Tween glowTween;

    //Jelly
    protected Sequence jellySequence;

    protected virtual void Start()
    {
        this.render = GetComponent<SpriteRenderer>();

        this.initPosition = this.transform.position;
        this.initScale = this.transform.localScale;
    }

    public virtual void Init(){ }

    public virtual void ResetElement() { }

    public virtual void OnCollision() { }

    public virtual void OnCollision(Vector2 position, Vector2 velocity) { }

    protected virtual void ResetLocalScale()
    {
        this.transform.localScale = this.initScale;
    }

    public virtual void ChangeColor(Color color)
    {
        if (this.render == null)
            return;

        this.render.color = color;
    }

    public virtual void OnGlowColor(Color glowColor, Color initColor, float duration, Ease ease = Ease.InBack)
    {
        if (this.render == null)
            return;

        if(this.glowTween != null && (this.glowTween.IsActive() || this.glowTween.IsPlaying())) {
            this.glowTween.Complete();
            this.glowTween.Kill();
            this.glowTween = null;
        }

        this.glowTween = this.render.DOColor(initColor, duration).From(glowColor).SetEase(ease);
    }


    public virtual void OnJellyEffect(float strength = 0.2f, float delay = 0)
    {
        if(jellySequence != null && jellySequence.IsActive() && jellySequence.IsPlaying()) {
            jellySequence.Complete();
            jellySequence.Kill();
            jellySequence = null;
        }

        jellySequence = DOTween.Sequence();
        if (delay > 0)
            jellySequence.AppendInterval(delay);
        jellySequence.Append(this.transform.DOScaleX(this.initScale.x + strength, 0.05f).SetEase(Ease.InOutQuad));
        jellySequence.Append(this.transform.DOScaleX(this.initScale.x, 0.6f).SetEase(Ease.OutElastic));

        jellySequence.Insert(0.05f + delay, this.transform.DOScaleY(this.initScale.y + strength, 0.05f).SetEase(Ease.InOutQuad));
        jellySequence.Insert(0.1f + delay, this.transform.DOScaleY(this.initScale.y, 0.6f).SetEase(Ease.OutElastic));

        jellySequence.AppendCallback(this.ResetLocalScale);
    }
}
