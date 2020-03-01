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

    protected virtual void Start()
    {
        this.render = GetComponent<SpriteRenderer>();

        this.initPosition = this.transform.position;
        this.initScale = this.transform.localScale;
    }

    public virtual void Init(){ }

    public virtual void ResetElement() { }

    protected virtual void ResetLocalScale()
    {
        this.transform.localScale = this.initScale;
    }

    public virtual void ChangeColor(Color color)
    {
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

    Sequence seq;

    public virtual void OnJellyEffect(float strength = 0.2f, float delay = 0)
    {
        if(seq != null && seq.IsActive() && seq.IsPlaying()) {
            seq.Complete();
            seq.Kill();
            seq = null;
        }

        seq = DOTween.Sequence();
        if (delay > 0)
            seq.AppendInterval(delay);
        seq.Append(this.transform.DOScaleX(this.initScale.x + strength, 0.05f).SetEase(Ease.InOutQuad));
        seq.Append(this.transform.DOScaleX(this.initScale.x, 0.6f).SetEase(Ease.OutElastic));

        seq.Insert(0.05f + delay, this.transform.DOScaleY(this.initScale.y + strength, 0.05f).SetEase(Ease.InOutQuad));
        seq.Insert(0.1f + delay, this.transform.DOScaleY(this.initScale.y, 0.6f).SetEase(Ease.OutElastic));

        seq.AppendCallback(this.ResetLocalScale);
    }
}
